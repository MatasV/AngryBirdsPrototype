using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

[System.Serializable]
public struct StageData
{
    public string stageName;
    public float stageCompletePercentage;
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

            Debug.Log(data[0].stageName);
            fs.Close();

            return data;
        } else
        {
            return null;
        }

    }

    public static void SaveStageData(StageData data)
    {
        List<StageData> currentStageData = (List<StageData>)LoadStageData();

        if(currentStageData != null)
        {
            currentStageData.RemoveAll(x => x.stageName == data.stageName);
            currentStageData.Add(data);
        } else
        {
            var newData = new List<StageData>() { data };
            BinaryFormatter bf_new = new BinaryFormatter();
            FileStream fs_new = File.Create(Application.persistentDataPath + "/LevelData.dat");

            bf_new.Serialize(fs_new, newData);
            fs_new.Close();

            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = File.Create(Application.persistentDataPath + "/LevelData.dat");


        bf.Serialize(fs, currentStageData);
        fs.Close();
    }
}
