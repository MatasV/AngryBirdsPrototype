using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class StageData
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
    public static void WriteToBinaryFile<T>(string filePath, T objectToWrite, bool append = false)
    {
        using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
        {
            var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            binaryFormatter.Serialize(stream, objectToWrite);
        }
    }

    public static T ReadFromBinaryFile<T>(string filePath)
    {
        try
        {
            using (Stream stream = File.Open(filePath, FileMode.Open))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                return (T)binaryFormatter.Deserialize(stream);
            }
        }
        catch (Exception e)
        {
            Debug.Log("File not found: " + filePath);
            return default(T);
        }
        
    }

}
