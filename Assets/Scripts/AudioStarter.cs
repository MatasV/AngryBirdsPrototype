using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioStarter : MonoBehaviour
{
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private string AudioName;
    [SerializeField] private bool IsMusic;
    private void Start()
    {
        if(IsMusic) audioManager.PlayMusic(AudioName);
        else audioManager.PlaySound(AudioName);
    }
}
