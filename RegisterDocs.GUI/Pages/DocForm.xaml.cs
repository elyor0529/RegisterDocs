using RegisterDocs.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using System.Windows.Shapes;

namespace RegisterDocs.GUI.Pages
{
  /// <summary>
  /// Interaction logic for DocForm.xaml
  /// </summary>
  public partial class DocForm : Window
  {
    private readonly int _id;

    public DocForm(int id) : this()
    {
      _id = id; 
    }

    public DocForm()
    {
      InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      if (_id > 0)
      {

        buttonDelete.Visibility = Visibility.Visible;

        var db = new RegisterDocsDbContext();
        var doc = db.Docs.Find(_id);

        BugungiSana.SelectedDate = doc.BugungiSana;
        IshtirokchiXodim.Text = doc.IshtirokchiXodim;
        JismoniyShaxsTur.Text = doc.JismoniyShaxsTur?.ToString();
        KelibTushganSana.SelectedDate = doc.KelibTushganSana;
        MuddatiUzaytirilganSana.SelectedDate = doc.MuddatiUzaytirilganSana;
        MurojaatMazmun.Text = doc.MurojaatMazmun;
        QayerdanKelibTushgan.Text = doc.QayerdanKelibTushgan;
        TegishliBoychaOrganIdora.Text = doc.TegishliBoychaOrganIdora;
        TegishliBoychaOrganSana.SelectedDate = doc.TegishliBoychaOrganSana;
        YuridikShaxsTur.Text = doc.YuridikShaxsTur?.ToString();
      }
      else
      {
        buttonDelete.Visibility = Visibility.Hidden;

        BugungiSana.SelectedDate = DateTime.Today;
        KelibTushganSana.SelectedDate = DateTime.Today;
        MuddatiUzaytirilganSana.SelectedDate = DateTime.Now.AddMonths(1);
        TegishliBoychaOrganSana.SelectedDate = DateTime.Now.AddMonths(1);
      }
    }

    private void buttonSave_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        var db = new RegisterDocsDbContext();
        Docs doc;

        if (_id > 0)
        {
          //TODO: edit
          doc = db.Docs.Find(_id);
          doc.BugungiSana = BugungiSana.Text.AsDateTime();
          doc.IshtirokchiXodim = IshtirokchiXodim.Text;
          doc.JismoniyShaxsTur = JismoniyShaxsTur.Text.AsInt();
          doc.KelibTushganSana = KelibTushganSana.Text.AsDateTime();
          doc.MuddatiUzaytirilganSana = MuddatiUzaytirilganSana.Text.AsDateTime();
          doc.MurojaatMazmun = MurojaatMazmun.Text;
          doc.QayerdanKelibTushgan = QayerdanKelibTushgan.Text;
          doc.TegishliBoychaOrganIdora = TegishliBoychaOrganIdora.Text;
          doc.TegishliBoychaOrganSana = TegishliBoychaOrganSana.Text.AsDateTime();
          doc.YuridikShaxsTur = YuridikShaxsTur.Text.AsInt();

          doc.CalculateColourStatus();

          db.Docs.Attach(doc);
          db.Entry(doc).State = EntityState.Modified;
        }
        else
        {
          //TODO: add

          doc = new Docs
          {
            BugungiSana = BugungiSana.Text.AsDateTime(),
            IsActive = true,
            IshtirokchiXodim = IshtirokchiXodim.Text,
            JismoniyShaxsTur = JismoniyShaxsTur.Text.AsInt(),
            KelibTushganSana = KelibTushganSana.Text.AsDateTime(),
            MuddatiUzaytirilganSana = MuddatiUzaytirilganSana.Text.AsDateTime(),
            MurojaatMazmun = MurojaatMazmun.Text,
            QayerdanKelibTushgan = QayerdanKelibTushgan.Text,
            TegishliBoychaOrganIdora = TegishliBoychaOrganIdora.Text,
            TegishliBoychaOrganSana = TegishliBoychaOrganSana.Text.AsDateTime(),
            YuridikShaxsTur = YuridikShaxsTur.Text.AsInt()
          };

          doc.CalculateColourStatus();

          db.Docs.Add(doc);
          db.Entry(doc).State = EntityState.Added;
        }

        db.SaveChanges();

        DialogResult = true;
      }
      catch (Exception exception)
      {
        System.Windows.Forms.MessageBox.Show(exception.Message, @"Error", MessageBoxButtons.OK,
            MessageBoxIcon.Error);

        DialogResult = false;
      }

    }

    private void buttonDelete_Click(object sender, RoutedEventArgs e)
    {
      if (System.Windows.Forms.MessageBox.Show("Вы уверин?", @"Удалить", MessageBoxButtons.OKCancel,
            MessageBoxIcon.Asterisk) != System.Windows.Forms.DialogResult.OK)
        return;

      try
      {
        var db = new RegisterDocsDbContext();
        var doc = db.Docs.Find(_id);

        db.Docs.Remove(doc);
        db.Entry(doc).State = EntityState.Deleted;
        db.SaveChanges();

        DialogResult = true;
      }
      catch (Exception exception)
      {
        System.Windows.Forms.MessageBox.Show(exception.Message, @"Error", MessageBoxButtons.OK,
            MessageBoxIcon.Error);

        DialogResult = false;
      }
    }
  }
}
