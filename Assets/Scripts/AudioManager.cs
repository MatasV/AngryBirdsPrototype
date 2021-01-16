using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu]
public class AudioManager : ScriptableObject
{
    [SerializeField] private Audio[] music;
    [SerializeField] private Audio[] effects;
    
    [Header("References")]
    [SerializeField]
    private AudioMixer mixer;
    
    public void PlayMusic(string name)
    {
        for (int i = 0; i < music.Length; i++)
        {
            if (music[i].audioName == name)
            {
                if (GameObject.Find(name) != null) return;
                
                var go = Instantiate(new GameObject());
                DontDestroyOnLoad(go);

                go.name = name;
                
                var audioSource = go.AddComponent<AudioSource>();
                
                audioSource.GetComponent<AudioSource>().outputAudioMixerGroup = mixer.FindMatchingGroups("Music")[0];
                audioSource.GetComponent<AudioSource>().loop = music[i].audioLoop;
                audioSource.GetComponent<AudioSource>().volume = music[i].audioVolume;
                audioSource.GetComponent<AudioSource>().clip = music[i].audioClip;

                music[i].audioSource = audioSource;
                
                music[i].Play();

                if (!music[i].audioLoop) Destroy(go, music[i].audioClip.length + 0.1f);
                
                return;
            }
        }
    }
    
    public void PlaySound(string name)
    {
        for (int i = 0; i < effects.Length; i++)
        {
            if (effects[i].audioName == name)
            {
                var go = Instantiate(new GameObject());
                
                var audioSource = go.AddComponent<AudioSource>();
                
                audioSource.GetComponent<AudioSource>().outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
                audioSource.GetComponent<AudioSource>().loop = effects[i].audioLoop;
                audioSource.GetComponent<AudioSource>().volume = effects[i].audioVolume;
                audioSource.GetComponent<AudioSource>().clip = effects[i].audioClip;

                effects[i].audioSource = audioSource;
                
                effects[i].Play();

                if (!effects[i].audioLoop) Destroy(go, effects[i].audioClip.length + 0.1f);
                
                effects[i].Play();
                return;
            }
        }

        Debug.Log("AudioManager: " + name + " not found in list.");
    }
    
    public void StopMusic(string name)
    {
        for (int i = 0; i < music.Length; i++)
        {
            if (music[i].audioName == name)
            {
                music[i].Stop();
                return;
            }
        }

        Debug.Log("AudioManager: " + name + " not found in list.");
    }
    
    public void StopSound(string name)
    {
        for (int i = 0; i < effects.Length; i++)
        {
            if (effects[i].audioName == name)
            {
                effects[i].Stop();
                return;
            }
        }

        Debug.Log("AudioManager: " + name + " not found in list.");
    }
}
