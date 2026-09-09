using MercyEditor.Editors;
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
    private readonly string _templatePath = $@"..\..\..\MercyEditor\ProjectTemplates\";

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
          ValidateProjectPath();
          OnPropertyChanged( nameof( ProjectName ) );
        }
      }
    }

    private string _projectPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\Mercy\";
    public string ProjectPath
    {
      get => _projectPath;
      set
      {
        if ( _projectPath != value )
        {
          _projectPath = value;
          ValidateProjectPath();
          OnPropertyChanged( nameof( ProjectPath ) );
        }
      }
    }

    private bool _pathIsValid;
    public bool PathIsValid
    {
      get => _pathIsValid;
      set
      {
        if ( _pathIsValid != value )
        {
          _pathIsValid = value;
          OnPropertyChanged( nameof( PathIsValid ) );
        }
      }
    }

    private string _pathErrorMsg;
    public string PathErrorMsg
    {
      get => _pathErrorMsg;
      set
      {
        if ( _pathErrorMsg != value )
        {
          _pathErrorMsg = value;
          OnPropertyChanged( nameof( PathErrorMsg ) );
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
        var templateFiles = Directory.GetFiles(_templatePath, "template.xml", SearchOption.AllDirectories);
        Debug.Assert( templateFiles.Any() );

        // Populate templates
        foreach ( var file in templateFiles )
        {
          var template = Serializer.ReadFile<ProjectTemplate>(file);
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

      ValidateProjectPath();
    }

    private bool ValidateProjectPath()
    {
      // Build final path
      if ( !Path.EndsInDirectorySeparator( ProjectPath ) )
      {
        ProjectPath += @"\";
      }
      string path = ProjectPath;
      path += $@"{ProjectName}\";

      // Verify path is valid
      PathIsValid = false;
      // Name checks
      if ( string.IsNullOrEmpty( ProjectName.Trim() ) )
      {
        PathErrorMsg = "Please enter a project name.";
      }
      else if ( ProjectName.IndexOfAny( Path.GetInvalidFileNameChars() ) != -1 )
      {
        PathErrorMsg = "Invalid character(s) used in project name.";
      }
      // Path checks
      else if ( string.IsNullOrEmpty( ProjectPath.Trim() ) )
      {
        PathErrorMsg = "Please select a valid project folder.";
      }
      else if ( ProjectPath.IndexOfAny( Path.GetInvalidPathChars() ) != -1 )
      {
        PathErrorMsg = "Invalid character(s) used in project path.";
      }
      // Directory checks
      else if ( Directory.Exists( path ) && Directory.EnumerateFileSystemEntries( path ).Any() )
      {
        PathErrorMsg = "Selected project folder already exists and is not empty.";
      }
      // Everything is good
      else
      {
        PathErrorMsg = string.Empty;
        PathIsValid = true;
      }

      return PathIsValid;
    }

    public string CreateProject( ProjectTemplate template )
    {
      // Double check path is good
      if ( !ValidateProjectPath() )
      {
        return string.Empty;
      }

      var path = $@"{ProjectPath}{ProjectName}\";

      try
      {
        if ( !Directory.Exists( path ) )
        {
          Directory.CreateDirectory( path );
        }

        foreach ( var folder in template.Folders )
        {
          Directory.CreateDirectory( Path.GetFullPath( Path.Combine( Path.GetDirectoryName( path ), folder ) ) );
        }
        var directoryInfo = new DirectoryInfo(path + @".Mercy\");
        directoryInfo.Attributes |= FileAttributes.Hidden;
        File.Copy( template.IconFilePath, Path.GetFullPath( Path.Combine( directoryInfo.FullName, "Icon.png" ) ) );
        File.Copy( template.ScreenshotFilePath, Path.GetFullPath( Path.Combine( directoryInfo.FullName, "Screenshot.png" ) ) );

        // Copy template xml and replace with name/path
        var projectXml = File.ReadAllText( template.ProjectFilePath );
        projectXml = string.Format( projectXml, ProjectName, path );
        var projectPath = Path.GetFullPath( Path.Combine( path, $"{ProjectName}{Project.Extension}" ) );
        File.WriteAllText( projectPath, projectXml );

        return path;
      }
      catch ( Exception e )
      {
        // TODO: Look at hooking this into spdlog
        Debug.WriteLine( e.Message );

        return string.Empty;
      }
    }
  } // NewProject
} // MercyEditor.Explorer
