using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public AudioSource[] Soundtracks = new AudioSource[2];
    public AudioClip currTrack;
    bool queued = true;
    void Awake() {
        if(instance == null) {
            instance = this;
            DontDestroyOnLoad(instance);
        } else {
            Destroy(gameObject);
        }
        currTrack = Soundtracks[0].clip;
        Soundtracks[0].volume = 0.2f; 
        Soundtracks[0].Play(); 
        StartCoroutine(FadeAudioSource.StartFade(Soundtracks[0], 2, 0.5f)); 
    }

    public void changeMusic(AudioClip newSoundtrack, float volume, int fadeInDuration, int fadeOutDuration) {
        if (newSoundtrack == currTrack) { return; }
        if (queued) {
            Soundtracks[1].volume = 0;
            Soundtracks[1].Play();
            StartCoroutine(FadeAudioSource.StartFade(Soundtracks[0], fadeOutDuration, 0));
            StartCoroutine(FadeAudioSource.StartFade(Soundtracks[1], fadeInDuration, volume));
            currTrack = newSoundtrack;
            queued = false;
        } else {
            Soundtracks[0].clip = newSoundtrack;
            Soundtracks[0].volume = 0;
            Soundtracks[0].Play();
            StartCoroutine(FadeAudioSource.StartFade(Soundtracks[1], fadeOutDuration, 0));
            StartCoroutine(FadeAudioSource.StartFade(Soundtracks[0], fadeInDuration, volume));
            queued = true;
        }
        // StartCoroutine(FadeAudioSource.StartCrossFade(Soundtracks[currid], Soundtracks[newid], 10, 0.5f, 5));
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
