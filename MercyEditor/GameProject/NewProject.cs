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
    public string ProjectType { get; set; } = null!;
    [DataMember]
    public string ProjectFile { get; set; } = null!;
    [DataMember]
    public List<string> Folders  { get; set; } = [];

    public byte[] Icon { get; set; } = null!;
    public string IconFilePath { get; set; } = null!;
    public byte[] Screenshot { get; set; } = null!;
    public string ScreenshotFilePath { get; set; } = null!;

    public string ProjectFilePath { get; set; } = null!;
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
                ValidateProjectPath();
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
                ValidateProjectPath();
                OnPropertyChanged(nameof(ProjectPath));
            }
        }
    }

    private bool _isValid = false;
    public bool IsValid
    {
        get => _isValid;
        set
        {
            if (_isValid != value)
            {
                _isValid = value;
                OnPropertyChanged(nameof(IsValid));
            }
        }
    }

    private string _errorMessage = string.Empty;
    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            if (_errorMessage != value)
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
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

            ValidateProjectPath();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            // TODO: properly log error
        }
    }

    private bool ValidateProjectPath()
    {
        // Set up the full project path
        string path = ProjectPath;
        if (!Path.EndsInDirectorySeparator(path))
        {
            path += @"\";
        }
        path += $@"{ProjectName}\";

        // Validate path location
        IsValid = false;
        if (string.IsNullOrEmpty(ProjectName.Trim()))
        {
            ErrorMessage = "Type in a project name.";
        }
        else if (ProjectName.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
        {
            ErrorMessage = "Invalid character(s) used in project name.";
        }
        else if (string.IsNullOrEmpty(ProjectPath.Trim()))
        {
            ErrorMessage = "Select a valid project folder.";
        }
        else if (ProjectPath.IndexOfAny(Path.GetInvalidPathChars()) != -1)
        {
            ErrorMessage = "Invalid character(s) used in project path.";
        }
        else if (Directory.Exists(path) && Directory.EnumerateFileSystemEntries(path).Any())
        {
            ErrorMessage = "Selected project folder already exists and is not empty.";
        }
        else
        {
            ErrorMessage = string.Empty;
            IsValid = true;
        }

        return IsValid;
    }

    public string CreateProject(ProjectTemplate template)
    {
        ValidateProjectPath();
        if (!IsValid)
        {
            return string.Empty;
        }

        // Set up the full project path
        if (!Path.EndsInDirectorySeparator(ProjectPath))
        {
            ProjectPath += @"\";
        }
        string path = $@"{ProjectPath}{ProjectName}\";

        try
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            // Creation of sub-folders
            string? directory = Path.GetDirectoryName(path);
            if (directory != null)
            {
                foreach (string folder in template.Folders)
                {
                    Directory.CreateDirectory(Path.GetFullPath(Path.Combine(directory, folder)));
                }
            }

            // Hide internal folder
            DirectoryInfo dirInfo = new DirectoryInfo(path + @".Mercy");
            dirInfo.Attributes |= FileAttributes.Hidden;

            // Copy photos for display
            File.Copy(template.IconFilePath, Path.GetFullPath(Path.Combine(dirInfo.FullName, "Icon.png")));
            File.Copy(template.ScreenshotFilePath, Path.GetFullPath(Path.Combine(dirInfo.FullName, "Screenshot.png")));

            // Project creation
            string projectXml = File.ReadAllText(template.ProjectFilePath);
            projectXml = string.Format(projectXml, ProjectName, ProjectPath);
            string projectPath = Path.GetFullPath(Path.Combine(path, $"{ProjectName}{Project.Extension}"));
            File.WriteAllText(projectPath, projectXml);

            return path;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            // TODO: properly log error
            return string.Empty;
        }
    }

} // class NewProject

// end namespace MercyEditor.GameProject