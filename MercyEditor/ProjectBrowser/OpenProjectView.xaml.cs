using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace MercyEditor.ProjectBrowser;

/// <summary>
/// Interaction logic for OpenProjectView.xaml
/// </summary>
public partial class OpenProjectView : UserControl
{
    public OpenProjectView()
    {
        InitializeComponent();
    }

    private void OnOpen_Button_Click(object sender, RoutedEventArgs e)
    {
        OpenSelectedProject();
    }

    private void OnListBoxItem_Mouse_DoubleClick(object sender, MouseButtonEventArgs e)
    {
        OpenSelectedProject();
    }

    private void OpenSelectedProject()
    {
        Project? project = OpenProject.Open(projectsListBox.SelectedItem as ProjectData);
        bool dialogResult = false;

        Window win = Window.GetWindow(this);

        // Create the project if it exists. If there is some issue, we want to handle gracefully and allow them to retry if possible
        if (project != null)
        {

            dialogResult = true;
            win.DataContext = project;
        }
        else
        {
            Debug.WriteLine("vm or template is null.");
            // TODO: Properly log error
        }

        win.DialogResult = dialogResult;
        win.Close();
    }
}
