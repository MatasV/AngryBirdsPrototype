using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class SceneLoader : ScriptableObject
{
    [SerializeField] private GameObject SceneFader;
    public void StartScene(string sceneName) => Instantiate(SceneFader).GetComponent<ScreenFader>().StartScene(sceneName);

}
