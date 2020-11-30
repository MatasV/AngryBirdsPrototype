using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance;

    public SpriteRenderer fadeMaterialSprite;

    public void StartScene(string sceneName) => StartCoroutine(nameof(ChangeScene), sceneName);

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        } else
        {
            instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }


    private IEnumerator ChangeScene(string sceneName)
    {
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

        yield return null;
    }
}
