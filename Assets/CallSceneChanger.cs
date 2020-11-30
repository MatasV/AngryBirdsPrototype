using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallSceneChanger : MonoBehaviour
{
    public void ChangeScene(string name)
    {
        SceneLoader.instance?.StartScene(name);
    }
}
