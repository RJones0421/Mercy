﻿using MercyEditor.Utilities;
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
using System.Windows.Input;

namespace MercyEditor.Editors
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

    public static UndoRedo UndoRedo { get; } = new UndoRedo();

    // --- Commands ----------------------------------------
    public ICommand UndoCommand {  get; private set; }
    public ICommand RedoCommand {  get; private set; }
    public ICommand SaveCommand {  get; private set; }

    public ICommand AddSceneCommand { get; private set; }
    public ICommand RemoveSceneCommand { get; private set; }

    // --- Methods ----------------------------------------
    public Project( string name, string path )
    {
      Name = name;
      Path = path;

      OnDeserialized( new StreamingContext() );
    }

    // --- Scene modifiers ---------------------------------
    private void AddSceneInternal( string sceneName )
    {
      Debug.Assert( !string.IsNullOrEmpty( sceneName.Trim() ) );
      _scenes.Add( new Scene( this, sceneName ) );
    }

    private void RemoveSceneInternal( Scene scene )
    {
      Debug.Assert( _scenes.Contains( scene ) );
      _scenes.Remove( scene );
    }

    // --- File IO -----------------------------------------
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

      // Setup add scene action
      AddSceneCommand = new RelayCommand<object>( x =>
      {
        AddSceneInternal( $"New Scene {_scenes.Count}" );
        Scene newScene = _scenes.Last();
        int sceneIndex = _scenes.Count - 1;

        UndoRedo.Add( new UndoRedoAction(
          $"Add {newScene.Name}",
          () => RemoveSceneInternal( newScene ),
          () => _scenes.Insert( sceneIndex, newScene )
          ) );
      } );

      // Setup remove scene action
      RemoveSceneCommand = new RelayCommand<Scene>( x =>
      {
        int sceneIndex = _scenes.IndexOf( x );
        RemoveSceneInternal( x );

        UndoRedo.Add( new UndoRedoAction(
          $"Remove {x.Name}",
          () => _scenes.Insert( sceneIndex, x ),
          () => RemoveSceneInternal( x )
          ) );
      },
      x => !x.IsActive );

      UndoCommand = new RelayCommand<object>( x => UndoRedo.Undo() );
      RedoCommand = new RelayCommand<object>( x => UndoRedo.Redo() );
      SaveCommand = new RelayCommand<object>( x => Save( this ) );
    }

    public void Unload()
    {

    }
  }
}
