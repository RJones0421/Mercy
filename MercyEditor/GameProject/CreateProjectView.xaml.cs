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

namespace MercyEditor.GameProject;

/// <summary>
/// Interaction logic for CreateProjectView.xaml
/// </summary>
public partial class CreateProjectView : UserControl
{
    public CreateProjectView()
    {
        InitializeComponent();
    }

    private void OnCreate_Button_Click(object sender, RoutedEventArgs e)
    {
        NewProject? vm = DataContext as NewProject;
        ProjectTemplate? template = templateListBox.SelectedItem as ProjectTemplate;
        bool dialogResult = false;

        // Create the project if it exists. If there is some issue, we want to handle gracefully and allow them to retry if possible
        if (vm != null && template != null)
        {
            string projectPath = vm.CreateProject(template);
            Window win = Window.GetWindow(this);
            if (!string.IsNullOrEmpty(projectPath))
            {
                dialogResult = true;
                Project? project = OpenProject.Open(new ProjectData() { ProjectName = vm.ProjectName, ProjectPath = projectPath });
                if (project != null)
                {
                    win.DataContext = project;
                }
            }
            win.DialogResult = dialogResult;
            win.Close();
        }
        else
        {
            Debug.WriteLine("vm or template is null.");
            // TODO: Properly log error
        }
    }
}
