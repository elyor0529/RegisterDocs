using RegisterDocs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RegisterDocs.GUI.Controls
{
  /// <summary>
  /// Interaction logic for BoshqaOrganQolganKunControl.xaml
  /// </summary>
  public partial class BoshqaOrganQolganKunControl : UserControl
  {
    public BoshqaOrganQolganKunControl()
    {
      InitializeComponent();
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
      var doc = (Docs)DataContext;
      if (doc != null)
      {
        var colour = doc.TegishliBoychaOrganQolganKun.GetColour();

        switch (colour)
        {
          case Enums.DocColours.White:
            Label1.Background = Brushes.White;
            break;
          case Enums.DocColours.Green:
            Label1.Background = Brushes.Green;
            break;
          case Enums.DocColours.Yellow:
            Label1.Background = Brushes.Yellow;
            break;
          case Enums.DocColours.Red:
            Label1.Background = Brushes.Red;
            break; 
        }

        Label1.Content = doc.TegishliBoychaOrganQolganKun;
      }
    }
  }
}
