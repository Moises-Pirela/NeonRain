using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SerializationManager
{
    public static bool Save(string saveName, object saveData)
    {
        BinaryFormatter formatter = GetBinaryFormatter();

        string dataPath = Application.persistentDataPath + "/saves";
        string saveExtension = string.Format("/{0}.save", saveName) ;

        if (!Directory.Exists(dataPath))
        {
            Directory.CreateDirectory(dataPath);
        }

        FileStream file = System.IO.File.Create(dataPath + saveExtension);
        
        formatter.Serialize(file, saveData);
        
        file.Close();

        return true;
    }

    public static object Load(string path)
    {
        if (!File.Exists(path))
        {
            return null;
        }

        BinaryFormatter formatter = GetBinaryFormatter();

        FileStream file = System.IO.File.Open(path, FileMode.Open);

        try
        {
            object save = formatter.Deserialize(file);
            
            file.Close();

            return save;
        }
        catch (Exception e)
        {
            Debug.LogErrorFormat("Failed to load file at {0} with exception {1}", path, e);
            
            file.Close();
            
            return null;
        }
    }

    public static BinaryFormatter GetBinaryFormatter()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        
        SurrogateSelector surrogateSelector = new SurrogateSelector();
        
        Vector3SerializationSurrogate vector3SerializationSurrogate = new Vector3SerializationSurrogate();
        QuaternionSerializationSurrogate quaternionSerializationSurrogate = new QuaternionSerializationSurrogate();
        
        surrogateSelector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), vector3SerializationSurrogate );
        surrogateSelector.AddSurrogate(typeof(Quaternion), new StreamingContext(StreamingContextStates.All), quaternionSerializationSurrogate );

        formatter.SurrogateSelector = surrogateSelector;
        
        return formatter;
    }
}
