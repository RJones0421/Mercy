using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MercyEditor.Utilities
{
  public interface IUndoRedo
  {
    string Name { get; }

    void Undo();
    void Redo();
  }

  // -------------------------------------------------------

  public class UndoRedoAction : IUndoRedo
  {
    private Action _redoAction;
    private Action _undoAction;

    public string Name { get; }
    public void Redo() => _redoAction();
    public void Undo() => _undoAction();

    // --- Constructors ------------------------------------
    public UndoRedoAction( string name )
    {
      Name = name;
    }

    public UndoRedoAction( string name, Action undo, Action redo )
      : this( name )
    {
      Debug.Assert( undo != null && redo != null );
      _undoAction = undo;
      _redoAction = redo;
    }
  }

  // -------------------------------------------------------

  public class UndoRedo
  {
    // --- Properties ------------------------------------
    private readonly ObservableCollection<IUndoRedo> _redoList = new ObservableCollection<IUndoRedo>();
    public ReadOnlyObservableCollection<IUndoRedo> RedoList { get; }

    private readonly ObservableCollection<IUndoRedo> _undoList = new ObservableCollection<IUndoRedo>();
    public ReadOnlyObservableCollection<IUndoRedo> UndoList { get; }

    // --- Methods -----------------------------------------
    public UndoRedo()
    {
      RedoList = new ReadOnlyObservableCollection<IUndoRedo>( _redoList );
      UndoList = new ReadOnlyObservableCollection<IUndoRedo>( _undoList );
    }

    public void Reset()
    {
      _redoList.Clear();
      _undoList.Clear();
    }

    public void Add( IUndoRedo command )
    {
      _undoList.Add( command );
      _redoList.Clear();
    }

    public void Undo()
    {
      if ( _undoList.Any() )
      {
        var command = _undoList.Last();
        _undoList.RemoveAt( _undoList.Count - 1 );
        command.Undo();
        _redoList.Insert( 0, command );
      }
    }

    public void Redo()
    {
      if ( _redoList.Any() )
      {
        var command = _redoList.First();
        _redoList.RemoveAt( 0 );
        command.Redo();
        _undoList.Insert( _undoList.Count, command );
      }
    }
  }
}
