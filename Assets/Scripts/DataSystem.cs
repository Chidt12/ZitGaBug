using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class DataSystem 
{
    public static void SavePlayer(Maps saveMap)
    {

        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/Maps.dat";

        using (FileStream file = new FileStream(path, FileMode.OpenOrCreate))
        {
            
            MapsData data = new MapsData(saveMap);
            formatter.Serialize(file, data);
        }
    }

    public static MapsData LoadPlayer()
    {

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/Maps.dat";
        if (File.Exists(path))
        {
            try
            {
                using (FileStream file = new FileStream(path, FileMode.Open))
                {
                    var data = (MapsData)formatter.Deserialize(file);
                    return data;
                }
            }
            catch
            {
                return null;
            }
        }
        else
        {
            return null;
        }
    }
}
