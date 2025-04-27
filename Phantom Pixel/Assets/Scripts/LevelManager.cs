using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class LevelManager
{
    private static bool[] levelCompleted = { false, false, false };


    public static void completeLevel(int level)
    {
        levelCompleted[level - 1] = true;
        SaveLevel();
    }

    public static void LoseLevel()
    {
        Debug.Log("You lost the level!");
        TimeManager.RestartTime();
    }

    public static void SaveLevel()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/Momentary.SaveData";
        FileStream stream = new FileStream(path, FileMode.Create);
        // This is probably me worrying again but im not sure if this "safe"
        // I'm not sure if this handles if there is already data written at that point
        // bu this is a tutorial so let me wait and see
        
        formatter.Serialize(stream, levelCompleted);
        stream.Close();

    }

    public static void LoadLevelData()
    {
        string path = Application.persistentDataPath + "/Momentary.SaveData";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            levelCompleted = formatter.Deserialize(stream) as bool[];
            stream.Close();
            
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
        }
    }

    public static int currentLevel()
    {
        for (int i = 0; i < levelCompleted.Length; i++)
        {
            if (!levelCompleted[i])
            {
                return i + 1;
            }
        }

        return 1;
    }

    public static void ResetSave()
    {
        string path = Application.persistentDataPath + "/Momentary.SaveData";
        if (File.Exists(path))
        {
            File.Delete(path);
            
        } else
        {
            Debug.LogError("No save data found to erase " + path);
        }
    }
}
