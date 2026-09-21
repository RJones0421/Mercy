using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MercyEditor.ProjectBrowser;

[DataContract]
public class Scene : ViewModelBase
{
    private string _name = string.Empty;
    [DataMember]
    public string Name
    {
        get => _name;
        set
        {
            if (_name != value)
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
    }

    private bool _isActive;
    [DataMember]
    public bool IsActive
    {
        get => _isActive;
        set
        {
            if (_isActive != value)
            {
                _isActive = value;
                OnPropertyChanged(nameof(IsActive));
            }
        }
    }

    [DataMember]
    public Project Project { get; private set; } = null!;

    public Scene(Project project, string name)
    {
        Debug.Assert(project != null);
        Project = project;
        Name = name;
    }
}
