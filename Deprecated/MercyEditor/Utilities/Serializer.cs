using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MercyEditor.Utilities
{
  internal static class Serializer
  {
    public static T ReadFile<T>( string path )
    {
      try
      {
        using var fs = new FileStream( path, FileMode.Open );
        var serializer = new DataContractSerializer( typeof(T) );
        T instance = (T)serializer.ReadObject( fs );

        return instance;
      }
      catch ( Exception e )
      {
        // TODO: Look at hooking this into spdlog
        Debug.WriteLine( e.Message );

        return default( T );
      }
    }

    public static void WriteFile<T>( T instance, string path )
    {
      try
      {
        using var fs = new FileStream( path, FileMode.Create );
        var serializer = new DataContractSerializer( typeof(T) );
        serializer.WriteObject( fs, instance );
      }
      catch ( Exception e )
      {
        // TODO: Look at hooking this into spdlog
        Debug.WriteLine( e.Message );
      }
    }
  }
}
