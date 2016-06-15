using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Timers;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Threading;
using System.Threading;
using RegisterDocs.Scheduler;

namespace RegisterDocs.GUI
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  { 
    public App()
    {

      Dispatcher.UnhandledException += OnDispatcherUnhandledException;
      AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;

      var uzCulture = new CultureInfo("uz-Cyrl-Uz");
      Thread.CurrentThread.CurrentCulture = uzCulture;
      Thread.CurrentThread.CurrentUICulture = uzCulture;
    } 
    
    private static void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
      MessageBox.Show(string.Format("An unhandled exception occurred: {0}", e.Exception.Message), "Error", MessageBoxButton.OK, MessageBoxImage.Error);

      e.Handled = true;
    }

    private static void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
      var ex = e.ExceptionObject as Exception;

      if (ex == null)
        return;

      MessageBox.Show(string.Format("An unhandled exception occurred: {0}", ex.Message), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }
  }
}
