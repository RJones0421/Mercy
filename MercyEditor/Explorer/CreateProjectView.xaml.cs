﻿using MercyEditor.Editors;
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

namespace MercyEditor.Explorer
{
  /// <summary>
  /// Interaction logic for CreateProjectView.xaml
  /// </summary>
  public partial class CreateProjectView : UserControl
  {
    public CreateProjectView()
    {
      InitializeComponent();
    }

    private void OnBrowse_Button_Click( object sender, RoutedEventArgs e )
    {
      // TODO: Add folder browsing
    }

    private void OnCreate_Button_Click( object sender, RoutedEventArgs e )
    {
      // Create the project
      NewProject viewModel = DataContext as NewProject;
      string projectPath = viewModel.CreateProject( templateListBox.SelectedItem as ProjectTemplate );

      // Verify successful creation
      bool dialogResult = false;
      Window window = Window.GetWindow( this );
      if ( !string.IsNullOrEmpty( projectPath ) )
      {
        dialogResult = true;
        Project project = OpenProject.Open( new ProjectData()
        {
          ProjectName = viewModel.ProjectName,
          ProjectPath = $@"{viewModel.ProjectPath}{viewModel.ProjectName}\"
        } );
        window.DataContext = project;
      }

      // Close if created
      window.DialogResult = dialogResult;
      window.Close();
    }
  }
}
