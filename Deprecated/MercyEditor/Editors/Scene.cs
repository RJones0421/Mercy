using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MercyEditor.Editors
{
  [DataContract]
  internal class Scene : ViewModelBase
  {
    // --- Properties -------------------------------------
    private string _name;
    [DataMember]
    public string Name
    {
      get => _name;
      set
      {
        if ( _name != value )
        {
          _name = value;
          OnPropertyChanged( nameof( Name ) );
        }
      }
    }
    [DataMember]
    public Project Project { get; private set; }

    private bool _isActive;
    [DataMember]
    public bool IsActive
    {
      get => _isActive;
      set
      {
        if ( _isActive != value )
        {
          _isActive = value;
          OnPropertyChanged( nameof( IsActive ) );
        }
      }
    }

    // --- Methods ----------------------------------------
    public Scene( Project project, string name )
    {
      Debug.Assert( project != null );

      Project = project;
      Name = name;
    }

  } // Scene
} // MercyEditor.Editor
