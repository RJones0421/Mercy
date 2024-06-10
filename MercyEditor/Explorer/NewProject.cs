using MercyEditor.Common;
using MercyEditor.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MercyEditor.Explorer
{
  [DataContract]
  internal class ProjectTemplate
  {
    [DataMember]
    public string ProjectType { get; set; }
    [DataMember]
    public string ProjectFile { get; set; }
    public string ProjectFilePath { get; set; }
    [DataMember]
    public List<string> Folders { get; set; }

    public byte[] Icon { get; set; }
    public string IconFilePath { get; set; }
    public byte[] Screenshot { get; set; }
    public string ScreenshotFilePath { get; set; }
  }

  internal class NewProject : ViewModelBase
  {
    // TODO: get the path from the installation location
    private readonly string _templatePath = $@"..\..\..\MercyEditor\ProjectTemplates";

    // --- Properties ----------------------------------------------------------
    private string _projectName = "NewProject";
    public string ProjectName
    {
      get => _projectName;
      set
      {
        if ( _projectName != value )
        {
          _projectName = value;
          OnPropertyChanged( nameof( ProjectName ) );
        }
      }
    }

    private string _projectPath = $@"{ Environment.GetFolderPath( Environment.SpecialFolder.MyDocuments ) }\Mercy\";
    public string ProjectPath
    {
      get => _projectPath;
      set
      {
        if ( _projectPath != value )
        {
          _projectPath = value;
          OnPropertyChanged( nameof( ProjectPath ) );
        }
      }
    }

    private ObservableCollection<ProjectTemplate> _projectTemplates = new ObservableCollection<ProjectTemplate>();
    public ReadOnlyObservableCollection<ProjectTemplate> ProjectTemplates
    { get; }

    // --- Methods ------------------------------------------------------------
    public NewProject()
    {
      ProjectTemplates = new ReadOnlyObservableCollection<ProjectTemplate>( _projectTemplates );

      // Read in and assign templates
      try
      {
        var templateFiles = Directory.GetFiles( _templatePath, "template.xml", SearchOption.AllDirectories );
        Debug.Assert( templateFiles.Any() );

        foreach ( var file in templateFiles )
        {
          var template = Serializer.ReadFile<ProjectTemplate>( file );
          template.ProjectFilePath = Path.GetFullPath( Path.Combine( Path.GetDirectoryName( file ), template.ProjectFile ) );
          template.IconFilePath = Path.GetFullPath( Path.Combine( Path.GetDirectoryName( file ), "Icon.png" ) );
          template.Icon = File.ReadAllBytes( template.IconFilePath );
          template.ScreenshotFilePath = Path.GetFullPath( Path.Combine( Path.GetDirectoryName( file ), "Screenshot.png" ) );
          template.Screenshot = File.ReadAllBytes( template.ScreenshotFilePath );

          _projectTemplates.Add( template );
        }
      }
      catch ( Exception e )
      {
        // TODO: Look at hooking this into spdlog
        Debug.WriteLine( e.Message );
      }
    }
  }
}
