using MercyEditor.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MercyEditor.Editor
{
  [DataContract( Name = "Game" )]
  internal class Project : ViewModelBase
  {
    public static readonly string Extension = ".mercy";

    // --- Properties -------------------------------------
    [DataMember]
    public string Name { get; private set; }
    [DataMember]
    public string Path { get; private set; }
    public string FullPath => $"{Path}{Name}{Extension}";

    [DataMember( Name = "Scenes" )]
    private ObservableCollection<Scene> _scenes = new ObservableCollection<Scene>();
    private ReadOnlyCollection<Scene> Scenes { get; }

    // --- Methods ----------------------------------------
    public Project( string name, string path )
    {
      Name = name;
      Path = path;

      _scenes.Add( new Scene( this, "Default Scene" ) );
    }
  }
}
