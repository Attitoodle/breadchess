using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string path = "";

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        path = Path.Combine(dataDirPath, dataFileName);
    }

    public GameData Load()
    {
        GameData loadedData = null;
        if (File.Exists(path))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(path, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                // deserialize data from JSON back to C# object
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to load data from file: " + path + "\n" + e);
            }
        }
        return loadedData;
    }

    public void Save(GameData data)
    {
        try
        {
            // create the directory path
            Directory.CreateDirectory(Path.GetDirectoryName(path));

            // serialize C# game data to JSON
            string dataToStore = JsonUtility.ToJson(data, true);

            // write to file system
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                    Debug.Log("test " + path + " " + dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file: " + path + "\n" + e);
        }
    }
}
