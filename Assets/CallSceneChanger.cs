using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallSceneChanger : MonoBehaviour
{
    [SerializeField] private SceneLoader sceneLoader;
    public void ChangeScene(string name)
    {
        sceneLoader.StartScene(name);
    }
}
