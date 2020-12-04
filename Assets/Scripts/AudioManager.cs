using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager: MonoBehaviour
{
    public static AudioManager audioManager;

    [SerializeField]
    GameObject audioSource;

    private void Awake()
    {
        if(audioManager == null)
        {
            audioManager = this;
        }
    }

    public void PlaySound(AudioClip clip, Vector3 location)
    {
        if (audioSource == null || audioSource.GetComponent<AudioSource>()) return;
        AudioSource source = Instantiate(audioSource, location, Quaternion.identity).GetComponent<AudioSource>();
        source.clip = clip;
        source.Play();
        Destroy(source, clip.length);
    }
}
