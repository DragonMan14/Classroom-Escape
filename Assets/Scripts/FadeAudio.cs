using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class FadeAudioSource {
    public static IEnumerator StartFade(AudioSource AudioSource, float duration, float targetVolume) {
        float currentTime = 0;
        float start = AudioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            AudioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
    }
    
    public static IEnumerator StartCrossFade(AudioSource AudioSource, AudioSource AudioSource2, float duration, float targetVolume, float whenToFade)
    {
        float currentTime = 0;
        float start = AudioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            StartFade(AudioSource, duration, 0);
            if (currentTime == whenToFade && AudioSource2 != null) {
                StartFade(AudioSource2, duration, targetVolume);
            }
            yield return null;
        }
    }
}
