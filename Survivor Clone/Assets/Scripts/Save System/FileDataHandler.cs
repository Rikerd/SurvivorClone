using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class FileDataHandler
{
    public static AccountData LoadData()
    {
        string path = Path.Combine(Application.persistentDataPath, "accountData.data");
        AccountData loadedData = null;

        if (File.Exists(path))
        {
            try
            {
                // load the binary data from file
                using (FileStream stream = new FileStream(path, FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    loadedData = formatter.Deserialize(stream) as AccountData;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error occurred when trying to load data to file: " + path + "\n" + e);
            }
        }

        return loadedData;
    }

    public static void SaveData(AccountData data)
    {
        string path = Path.Combine(Application.persistentDataPath, "accountData.data");
        try
        {
            // create the directory the file will be written to if it doesn't exist
            Directory.CreateDirectory(Path.GetDirectoryName(path));

            // write the data to binary file
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, data);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occurred when trying to save data to file: " + path + "\n" + e);
        }
    }
}
