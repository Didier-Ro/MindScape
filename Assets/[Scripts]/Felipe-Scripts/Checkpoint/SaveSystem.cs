using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

public class SaveSystem : MonoBehaviour
{
    // es mas que nada para poder guardar los datos de si trae esto o aquello xD, ya si no funciona o era chamba de alguien le decimos adi�s
    // Es un m�todo para guardar los datos del juego en un archivo
    public static void SaveGame(playerController player)
    {
        // la neta no le entend� muy bien pero funciona, espero
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/gameData.dat";
        FileStream stream = new FileStream(path, FileMode.Create);

        GameData data = new GameData();
        data.playerPosition = player.transform.position;
        data.inventory = player.inventory;

        formatter.Serialize(stream, data);
        stream.Close();
    }

    // El metodo para cargar los datos del juego desde un archivo
    public static GameData LoadGame()
    {
        string path = Application.persistentDataPath + "/gameData.dat";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameData data = formatter.Deserialize(stream) as GameData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}

// Clase para almacenar los datos del juego que se guardar�n y cargar�n
[Serializable]
public class GameData
{
    public Vector3 playerPosition;
    public List<string> inventory = new List<string>();
}