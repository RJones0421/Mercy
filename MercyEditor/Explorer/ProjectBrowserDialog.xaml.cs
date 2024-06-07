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
using System.Windows.Shapes;

namespace MercyEditor.Explorer
{
  /// <summary>
  /// Interaction logic for ProjectBrowserDialog.xaml
  /// </summary>
  public partial class ProjectBrowserDialog : Window
  {
    public ProjectBrowserDialog()
    {
      InitializeComponent();
    }

    private void OnToggleButton_Click( object sender, RoutedEventArgs e )
    {
      if ( sender == openProjectButton )
      {
        HandleOpenProject();
      }
      else // sender == createProjectButton
      {
        HandleCreateProject();
      }
    }

    private void HandleOpenProject()
    {
      // Make sure button stays clicked before early out
      openProjectButton.IsChecked = true;

      if ( createProjectButton.IsChecked == false )
      {
        // Open project panel already open
        return;
      }

      createProjectButton.IsChecked = false;

      browserContent.Margin = new Thickness( 0 );
    }

    private void HandleCreateProject()
    {
      // Make sure button stays clicked before early out
      createProjectButton.IsChecked = true;

      if ( openProjectButton.IsChecked == false )
      {
        // Create project panel already open
        return;
      }

      openProjectButton.IsChecked = false;

      browserContent.Margin = new Thickness( -800, 0, 0, 0 );
    }
  }
}
