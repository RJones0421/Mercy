using MercyEditor.Common;
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
using System.Windows;

namespace MercyEditor.Editor
{
  [DataContract( Name = "Game" )]
  internal class Project : ViewModelBase
  {
    public static readonly string Extension = ".mercy";

    // --- Properties -------------------------------------
    [DataMember]
    public string Name { get; private set; } = "New Project";
    [DataMember]
    public string Path { get; private set; }
    public string FullPath => $"{Path}{Name}{Extension}";

    [DataMember( Name = "Scenes" )]
    private ObservableCollection<Scene> _scenes = new ObservableCollection<Scene>();
    public ReadOnlyCollection<Scene> Scenes { get; private set; }
    private Scene _activeScene;
    public Scene ActiveScene
    {
      get => _activeScene;
      set
      {
        if ( _activeScene !=  value )
        {
          _activeScene = value;
          OnPropertyChanged( nameof( ActiveScene ) );
        }
      }
    }

    public static Project Current = Application.Current.MainWindow.DataContext as Project;

    // --- Methods ----------------------------------------
    public Project( string name, string path )
    {
      Name = name;
      Path = path;

      OnDeserialized( new StreamingContext() );
    }

    public static void Save( Project project )
    {
      Serializer.WriteFile( project, project.FullPath );
    }

    public static Project Load( string path )
    {
      Debug.Assert( File.Exists( path ) );
      return Serializer.ReadFile<Project>( path );
    }

    [OnDeserialized]
    private void OnDeserialized( StreamingContext context )
    {
      if ( _scenes != null )
      {
        Scenes = new ReadOnlyObservableCollection<Scene>( _scenes );
        OnPropertyChanged( nameof( Scenes ) );
      }

      ActiveScene = Scenes.FirstOrDefault( x => x.IsActive );
    }

    public void Unload()
    {

    }
  }
}
