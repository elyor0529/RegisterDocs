using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RegisterDocs.GUI.Pages;
using RegisterDocs.GUI.ViewModels;
using RegisterDocs.Models;
using System.Data;
using RegisterDocs.Helpers;
using System.Threading;
using RegisterDocs.Scheduler;
using System.Timers;
using RegisterDocs.Enums;

namespace RegisterDocs.GUI
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {

    private readonly BackgroundWorker _worker;
    private readonly System.Timers.Timer _timer;
    private IEnumerable<Docs> _items;
    private const string SELECT_OPTIONAL_VALUE = "[...]";
    private int _topRowIndex;

    private void FillFilterFields()
    {

      var db = new RegisterDocsDbContext();

      if (ComboBoxKelibTushgan.HasItems)
        ComboBoxKelibTushgan.Items.Clear();

      var items = db.Docs.Select(s => s.QayerdanKelibTushgan).Distinct();
      ComboBoxKelibTushgan.Items.Add(SELECT_OPTIONAL_VALUE);
      foreach (var item in items)
        ComboBoxKelibTushgan.Items.Add(item);

    }

    private void Run()
    {
      if (_worker.IsBusy)
        return;

      ExportButton.IsEnabled = false;
      DataGrid1.Cursor = System.Windows.Input.Cursors.Wait;

      var filterModel = new FilterViewModel
      {
        IsActive = CheckBoxStatus.IsChecked,
        Colour = ComboBoxColour.SelectedIndex == -1 ? null : ComboBoxColour.SelectedValue == SELECT_OPTIONAL_VALUE ? null : (DocColours?)ComboBoxColour.SelectedValue,
        KelibTushgan = ComboBoxKelibTushgan.SelectedIndex == -1 ? null : ComboBoxKelibTushgan.SelectedValue == SELECT_OPTIONAL_VALUE ? null : (string)ComboBoxKelibTushgan.SelectedValue
      };

      _worker.RunWorkerAsync(filterModel);
    }

    public MainWindow()
    {

      InitializeComponent();

      #region Worker

      _worker = new BackgroundWorker
      {
        WorkerSupportsCancellation = true
      };
      _worker.DoWork += WorkerOnDoWork;
      _worker.RunWorkerCompleted += WorkerOnRunWorkerCompleted;

      #endregion

      #region Timer

      _timer = new System.Timers.Timer
      {
        AutoReset = true,
        Enabled = true,
        Interval = 60 * 60 * 1000
      };
      _timer.Elapsed += Timer_Elapsed;
      _timer.Start();

      #endregion

      #region Pager

      var changedIndex = new RoutedUICommand("ChangedIndex", "ChangedIndex", Paging1.GetType());
      var binding = new CommandBinding
      {
        Command = changedIndex
      };
      binding.Executed += OnChangeIndexCommandHandler;
      CommandBindings.Add(binding);
      Paging1.ChangedIndexCommand = changedIndex;

      #endregion

    }

    private async void Timer_Elapsed(object sender, ElapsedEventArgs e)
    {

      await DocTrigger.Execute();

      Dispatcher.Invoke(() =>
      {
        Run();
      });

      _timer.Close();
      _timer.Start();
    }

    private void OnChangeIndexCommandHandler(object sender, ExecutedRoutedEventArgs e)
    {
      Run();
    }

    private void WorkerOnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
    {
      if (runWorkerCompletedEventArgs.Cancelled)
      {
        System.Windows.Forms.MessageBox.Show(@"Operation is cancalled", @"Cancel", MessageBoxButtons.OK,
            MessageBoxIcon.Warning);
      }
      else if (runWorkerCompletedEventArgs.Error != null)
      {
        System.Windows.Forms.MessageBox.Show(runWorkerCompletedEventArgs.Error.Message, @"Error", MessageBoxButtons.OK,
            MessageBoxIcon.Error);
      }
      else
      {
        var items = (IEnumerable<Docs>)runWorkerCompletedEventArgs.Result;
        var pageIndex = Paging1.PageIndex;
        var pageSize = Paging1.PageSize;
        _topRowIndex = (pageIndex - 1) * pageSize;
        _items = items.Skip(_topRowIndex).Take(pageSize);

        DataGrid1.Items.Clear();
        foreach (var item in _items)
        {
          DataGrid1.Items.Add(item);
        }

        Paging1.TotalCount = items.Count();
      }

      ExportButton.IsEnabled = true;
      DataGrid1.Cursor = Cursor;
    }

    private void WorkerOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
    {
      var db = new RegisterDocsDbContext();
      var filterModel = (FilterViewModel)doWorkEventArgs.Argument;
      var query = db.Docs.Where(w => w.IsActive == filterModel.IsActive);

      if (filterModel.Colour != null)
      {
        query = query.AsEnumerable()
          .Where(w => filterModel.Colour.CheckColour(w.TegishliBoychaOrganQolganKun) || filterModel.Colour.CheckColour(w.HalEtishMuddat))
          .AsQueryable();
      }

      if (!string.IsNullOrWhiteSpace(filterModel.KelibTushgan))
      {
        query = query.AsEnumerable()
          .Where(w => w.QayerdanKelibTushgan.CompareTo(filterModel.KelibTushgan) == 0)
          .AsQueryable();
      }

      var models = query.OrderBy(o => new
      {
        o.HalEtishMuddat,
        o.TegishliBoychaOrganQolganKun
      }).ToArray();

      doWorkEventArgs.Result = models;
    }

    private void ExportButton_Click(object sender, RoutedEventArgs e)
    {

      try
      {
        var filePath = ExcelHelper.Export(_items);

        Process.Start(filePath);
      }
      catch (Exception exception)
      {
        System.Windows.Forms.MessageBox.Show(exception.Message, @"Error", MessageBoxButtons.OK,
            MessageBoxIcon.Error);
      }
    }

    private void DataGrid_OnLoadingRow(object sender, DataGridRowEventArgs e)
    {
      e.Row.Header = (_topRowIndex + e.Row.GetIndex() + 1).ToString();
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
      Close();
    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
      var form = new DocForm();

      if (form.ShowDialog() != true)
        return;
      else
      {
        FillFilterFields();

        Run();
      }
    }

    private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      if (DataGrid1.SelectedCells.Count == 0)
        return;

      var item = (Docs)DataGrid1.SelectedCells[0].Item;

      if (item == null)
        return;

      var form = new DocForm(item.Id);

      if (form.ShowDialog() != true)
        return;
      else
      {
        FillFilterFields();

        Run();
      }
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      CheckBoxStatus.IsChecked = true;

      ComboBoxColour.Items.Add(SELECT_OPTIONAL_VALUE);
      ComboBoxColour.Items.Add(DocColours.White);
      ComboBoxColour.Items.Add(DocColours.Green);
      ComboBoxColour.Items.Add(DocColours.Yellow);
      ComboBoxColour.Items.Add(DocColours.Red);

      FillFilterFields();

      Run();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
      _worker.CancelAsync();
    }

    private void CheckBoxStatus_Clicked(object sender, RoutedEventArgs e)
    {
      Run();
    }

    private void ComboBoxColour_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (ComboBoxColour.SelectedIndex == -1)
        return;

      Run();
    }

    private void ComboBoxKelibTushgan_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (ComboBoxKelibTushgan.SelectedIndex == -1)
        return;

      Run();
    }

  }
}
