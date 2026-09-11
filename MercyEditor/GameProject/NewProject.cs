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
public class ProjectTemplate
{
    [DataMember]
    public string ProjectType { get; set; }
    [DataMember]
    public string ProjectFile { get; set; }
    [DataMember]
    public List<string> Folders  { get; set; }

    public byte[] Icon { get; set; }
    public string IconFilePath { get; set; }
    public byte[] Screenshot { get; set; }
    public string ScreenshotFilePath { get; set; }

    public string ProjectFilePath { get; set; }
}

internal class NewProject : ViewModelBase
{
    // TODO: get the path from the installation location
    private readonly string _templatePath = @"..\..\MercyEditor\ProjectTemplates";

    private string _projectName = "NewProject";
    public string ProjectName
    {
        get => _projectName;
        set
        {
            if (_projectName != value)
            {
                _projectName = value;
                OnPropertyChanged(nameof(ProjectName));
            }
        }
    }

    private string _projectPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\Mercy\";
    public string ProjectPath
    {
        get => _projectPath;
        set
        {
            if (_projectPath != value)
            {
                _projectPath = value;
                OnPropertyChanged(nameof(ProjectPath));
            }
        }
    }

    private ObservableCollection<ProjectTemplate> _projectTemplates = new ObservableCollection<ProjectTemplate>();
    public ReadOnlyObservableCollection<ProjectTemplate> ProjectTemplates { get; }

    public NewProject()
    {
        ProjectTemplates = new ReadOnlyObservableCollection<ProjectTemplate>(_projectTemplates);

        try
        {
            string[] templatesFiles = Directory.GetFiles(_templatePath, "template.xml", SearchOption.AllDirectories);
            Debug.Assert(templatesFiles.Any());
            foreach (string file in templatesFiles)
            {
                ProjectTemplate? template = Serializer.FromFile<ProjectTemplate>(file);
                if (template != null)
                {
                    string? directoryName = Path.GetDirectoryName(file);
                    if (directoryName != null)
                    {
                        template.IconFilePath = Path.GetFullPath(Path.Combine(directoryName, "Icon.png"));
                        template.Icon = File.ReadAllBytes(template.IconFilePath);
                        template.ScreenshotFilePath = Path.GetFullPath(Path.Combine(directoryName, "Screenshot.png"));
                        template.Screenshot = File.ReadAllBytes(template.ScreenshotFilePath);

                        template.ProjectFilePath = Path.GetFullPath(Path.Combine(directoryName, template.ProjectFile));
                    }
                    _projectTemplates.Add(template);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            // TODO: properly log error
        }
    }

} // class NewProject

// end namespace MercyEditor.GameProject