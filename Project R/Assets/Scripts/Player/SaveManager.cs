using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveManager 
{
    public static void SavePlayer(PlayerStats player)
    {
        BinaryFormatter formatter = new BinaryFormatter();//initialize binary formatter
        string path = Path.Combine(Application.persistentDataPath, "player.data");//path for save file
        Debug.Log(path);
        FileStream stream = new FileStream(path, FileMode.Create);//opens the path to write in

        PlayerData data = new PlayerData(player);//the actual data

        formatter.Serialize(stream, data);//saves the data
        stream.Close();
    }

    public static PlayerData LoadPlayer()
    {
        string path = Path.Combine(Application.persistentDataPath, "player.data");//path for save file
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);//filestream just to open the file

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();


            return data;
        }
        else
        {
            Debug.LogError("Save file does not exist in " + path);
            return null;
        }
    }
}
