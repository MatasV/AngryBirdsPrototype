using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenFader : MonoBehaviour
{
    private SpriteRenderer fadeMaterialSprite;
    public GameObject SceneChangeGFXObject;

    private void OnEnable()
    {
        
    }

    public void StartScene(string name)
    {
        StartCoroutine(nameof(ChangeScene), name);
    }
    private IEnumerator ChangeScene(string sceneName)
    {
        var clone =Instantiate(SceneChangeGFXObject);
        DontDestroyOnLoad(clone);
        DontDestroyOnLoad(gameObject);
        fadeMaterialSprite = clone.GetComponentInChildren<SpriteRenderer>();
        
        for(float i = 1; i >= -0.1f; i -= 0.1f)
        {
            fadeMaterialSprite.material.SetFloat("_SliceAmount", i);
            yield return null;
        }

        var sceneInfo = SceneManager.LoadSceneAsync(sceneName);

        yield return new WaitUntil(() => sceneInfo.isDone);

        for (float i = 0; i <= 1.1f; i += 0.1f)
        {
            fadeMaterialSprite.material.SetFloat("_SliceAmount", i);
            yield return null;
        }

        Destroy(clone);
        Destroy(gameObject);
    }
}
