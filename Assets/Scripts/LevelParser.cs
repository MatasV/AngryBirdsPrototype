using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct StageData
{
    public string stageName;
    public float stageCompletePercentage;
    public int maxScore;

    public StageData(string stageName, float stageCompletePercentage, int maxScore)
    {
        this.stageName = stageName;
        this.stageCompletePercentage = stageCompletePercentage;
        this.maxScore = maxScore;
    }
}

public class LevelParser
{
    public static IEnumerable<StageData> LoadStageData()
    {
        if (File.Exists(Application.persistentDataPath + "/LevelData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(Application.persistentDataPath + "/LevelData.dat", FileMode.Open);
            StageData[] data = bf.Deserialize(fs) as StageData[];
            fs.Close();
            return data;
        } else
        {
            return null;
        }
    }

    public static void SaveStageData(List<StageData> data)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = File.Create(Application.persistentDataPath + "/LevelData.dat");
        bf.Serialize(fs, data);
        fs.Close();
    }
}
