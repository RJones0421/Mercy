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

namespace MercyEditor.GameProject;

[DataContract]
public class ProjectData
{
    [DataMember]
    public string ProjectName { get; set; } = null!;
    [DataMember]
    public string ProjectPath { get; set; } = null!;
    [DataMember]
    public DateTime Date {  get; set; }

    public string FullPath { get => $"{ProjectPath}{ProjectName}{Project.Extension}"; }
    public byte[] Icon { get; set; } = null!;
    public byte[] Screenshot { get; set; } = null!;
}

[DataContract]
public class ProjectDataList
{
    [DataMember]
    public List<ProjectData> Projects { get; set; } = null!;
}

internal class OpenProject
{
    private static readonly string _applicationDataPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\MercyEditor\";
    private static readonly string _projectDataPath = String.Empty;

    private static readonly ObservableCollection<ProjectData> _projects = new ObservableCollection<ProjectData>();
    public static ReadOnlyObservableCollection<ProjectData> Projects { get; }

    static OpenProject()
    {
        Projects = new ReadOnlyObservableCollection<ProjectData>(_projects);

        try
        {
            if (!Directory.Exists(_applicationDataPath))
            {
                Directory.CreateDirectory(_applicationDataPath);
            }

            _projectDataPath = $@"{_applicationDataPath}ProjectData.xml";

            ReadProjectData();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            // TODO: properly log error
        }
    }

    private static void ReadProjectData()
    {
        if (File.Exists(_projectDataPath))
        {
            var projectDataList = Serializer.FromFile<ProjectDataList>(_projectDataPath);

            if (projectDataList != null)
            {
                IOrderedEnumerable<ProjectData> projects = projectDataList.Projects.OrderByDescending(x => x.Date);
                _projects.Clear();
                foreach (var project in projects)
                {
                    if (File.Exists(project.FullPath))
                    {
                        project.Icon = File.ReadAllBytes($@"{project.ProjectPath}\.Mercy\Icon.png");
                        project.Screenshot = File.ReadAllBytes($@"{project.ProjectPath}\.Mercy\Screenshot.png");
                        _projects.Add(project);
                    }
                }
            }
        }
    }

    private static void WriteProjectData()
    {
        List<ProjectData> projects = _projects.OrderBy(x => x.Date).ToList();
        Serializer.ToFile(new ProjectDataList() { Projects = projects }, _projectDataPath);
    }

    public static Project? Open(ProjectData? data)
    {
        if (data == null)
        {
            return null;
        }

        ReadProjectData();
        ProjectData? project = _projects.FirstOrDefault(x => x.FullPath == data.FullPath);

        if (project != null)
        {
            project.Date = DateTime.Now;
        }
        else
        {
            project = data;
            project.Date = DateTime.Now;
            _projects.Add(project);
        }

        WriteProjectData();

        return Project.Load(project.FullPath);
    }
}
