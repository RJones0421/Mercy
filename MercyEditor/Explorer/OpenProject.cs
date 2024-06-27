using MercyEditor.Editor;
using MercyEditor.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MercyEditor.Explorer
{
  // Container for basic info about each project for reference
  [DataContract]
  internal class ProjectData
  {
    [DataMember]
    public string   ProjectName { get; set; }
    [DataMember]
    public string   ProjectPath { get; set; }
    [DataMember]
    public DateTime Date        { get; set; }

    public string   FullPath    { get => $@"{ProjectPath}{ProjectName}{Project.Extension}"; }
    public byte[]   Icon        { get; set; }
    public byte[]   Screenshot  { get; set; }
  }

  // List of all projects created in editor
  [DataContract]
  internal class ProjectDataList
  {
    [DataMember]
    public List<ProjectData> Projects { get; set; }
  }

  internal class OpenProject
  {
    // --- Constants ----------------------------------------------------------
    private static readonly string _applicationDataPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\MercyEditor\";
    private static readonly string _projectDataPath;

    // --- Properties ---------------------------------------------------------
    private static readonly ObservableCollection<ProjectData> _projects = new ObservableCollection<ProjectData>();
    public static ReadOnlyCollection<ProjectData> Projects { get; }

    // --- Methods ------------------------------------------------------------
    static OpenProject()
    {
      try
      {
        if ( !Directory.Exists( _applicationDataPath ) )
        {
          Directory.CreateDirectory( _applicationDataPath );
        }

        _projectDataPath = $@"{_applicationDataPath}ProjectData.xml";

        Projects = new ReadOnlyCollection<ProjectData>( _projects );
        ReadProjectData();
      }
      catch (Exception e)
      {
        // TODO: Look at hooking this into spdlog
        Debug.WriteLine( e.Message );
      }
    }

    private static void ReadProjectData()
    {
      if ( File.Exists( _projectDataPath) )
      {
        var projects = Serializer.ReadFile<ProjectDataList>( _projectDataPath ).Projects.OrderByDescending( x => x.Date );
        _projects.Clear();
        foreach ( var project in projects )
        {
          // If project is not found, move forward
          // TODO: Remove project from file if not found?
          if ( !File.Exists( project.FullPath ) )
          {
            continue;
          }

          // Load project info
          project.Icon = File.ReadAllBytes( $@"{project.ProjectPath}\.Mercy\Icon.png" );
          project.Screenshot = File.ReadAllBytes( $@"{project.ProjectPath}\.Mercy\Screenshot.png" );
          _projects.Add( project );
        }
      }
    }

    private static void WriteProjectData()
    {
      // Order by date then save
      var projects = _projects.OrderBy( x => x.Date ).ToList();
      Serializer.WriteFile( new ProjectDataList() { Projects = projects }, _projectDataPath );
    }

    public static Project Open( ProjectData projectData )
    {
      // Read again in case another instance updates the list
      ReadProjectData();

      ProjectData project = _projects.FirstOrDefault( x => x.FullPath == projectData.FullPath );
      if ( project == null ) // No project found
      {
        project = projectData;
        project.Date = DateTime.Now;
        _projects.Add( project );
      }
      else // Project found in list
      {
        project.Date = DateTime.Now;
      }

      WriteProjectData();

      return Project.Load( project.FullPath );
    }
  } // OpenProject
} // MercyEditor.Explorer
