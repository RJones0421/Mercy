using MercyEditor.Editor;
using MercyEditor.Explorer;
using System;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MercyEditor
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();

      Loaded += OnMainWindowLoaded;

      Closing += OnMainWindowClosing;
    }

    private void OnMainWindowLoaded(object sender, RoutedEventArgs e)
    {
      Loaded -= OnMainWindowLoaded;

      OpenProjectBrowserDialog();
    }

    private void OnMainWindowClosing( object sender, CancelEventArgs e )
    {
      Closing -= OnMainWindowClosing;
      Project.Current?.Unload();
    }

    private void OpenProjectBrowserDialog()
    {
      var projectBrowser = new ProjectBrowserDialog();

      if ( projectBrowser.ShowDialog() == false || projectBrowser.DataContext == null )
      {
        Application.Current.Shutdown();
      }
      else
      {
        // Unload if switching projects
        Project.Current?.Unload();

        // Open the project
        DataContext = projectBrowser.DataContext;
      }
    }
  }
}