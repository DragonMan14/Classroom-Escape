using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSwitchTrigger : MonoBehaviour
{
    public AudioManager AudioManager;
    public AudioClip newSoundtrack;
    public float volume;
    public int fadeInDuration;
    public int fadeOutDuration;
    public bool firstTime = true;
    
    // Start is called before the first frame update
    void Start()
    {
        AudioManager = FindObjectOfType<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.HasTag("Player") || !firstTime)
        {
            return;
        }
        if (newSoundtrack != null)
        {
            firstTime = false;
            AudioManager.changeMusic(newSoundtrack, volume, fadeInDuration, fadeOutDuration);
        }
    }
}
