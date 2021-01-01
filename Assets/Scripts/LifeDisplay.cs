using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LifeDisplay : MonoBehaviour
{
    public TMP_Text healthDisplayText;
    public Transform visualDisplayHolder;

    private void Start()
    {
        StageManager.onBirdCountChanged += SetHealthText;
        StageManager.onBirdCountChanged += SetupImages;
    }

    private void SetHealthText(Queue<GameObject> healthRemaining)
    {
        healthDisplayText.text = "X" + healthRemaining.Count.ToString();
    }

    private void OnDestroy()
    {
        StageManager.onBirdCountChanged -= SetHealthText;
        StageManager.onBirdCountChanged -= SetupImages;
    }

    private void SetupImages(Queue<GameObject> healthRemaining)
    {
        if (visualDisplayHolder != null)
        {
            foreach (Transform child in visualDisplayHolder.transform)
            {
                Destroy(child.gameObject);
            }
        }

        var birdArray = healthRemaining.ToArray();

        for(int i = 0; i< birdArray.Length; i++)
        {
            var bird = birdArray[i];

            Sprite birdSprite = bird.GetComponent<SpriteRenderer>().sprite;

            GameObject obj = new GameObject(bird.name);
            Image birdImage = obj.AddComponent<Image>();

            birdImage.sprite = birdSprite;
            birdImage.preserveAspect = true;

            Color color = new Color(birdImage.color.r, birdImage.color.g, birdImage.color.b, (float)i / (float)birdArray.Length + 0.1f);
            birdImage.color = color;

            birdImage.transform.SetParent(visualDisplayHolder);

            birdImage.transform.localScale = Vector3.one;
        }

    }
    
}
