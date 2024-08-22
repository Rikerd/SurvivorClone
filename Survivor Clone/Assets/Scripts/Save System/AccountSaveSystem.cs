using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class AccountSaveSystem
{
    public static void SaveCoins(int coins)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/coins.lol";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, coins);
        stream.Close();
    }
}