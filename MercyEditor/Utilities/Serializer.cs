using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MercyEditor.Utilities;

public static class Serializer
{
    public static void ToFile<T>(T instance, string path)
    {
        try
        {
            using FileStream fs = new FileStream(path, FileMode.Create);
            DataContractSerializer serializer = new DataContractSerializer(typeof(T));
            serializer.WriteObject(fs, instance);
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
            // TODO: properly log error
        }
    }

    public static T? FromFile<T>(string path)
    {
        try
        {
            using FileStream fs = new FileStream(path, FileMode.Open);
            DataContractSerializer serializer = new DataContractSerializer(typeof(T));
            T? instance = (T?)serializer.ReadObject(fs);
            return instance;
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
            // TODO: properly log error
            return default(T);
        }
    }
}
