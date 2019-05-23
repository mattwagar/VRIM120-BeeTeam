using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeAudioManager : MonoBehaviour
{
    
    public AudioSource audioSource;
    public float volumeChangeSpeed = 0.5f;

    private Coroutine lowerRoutine;
    private Coroutine raiseRoutine;

    private IEnumerator LowerAudioSource()
    {
        float startVol = 1f;
        while(startVol > 0)
        {
            startVol += Time.deltaTime * volumeChangeSpeed;
            audioSource.volume = Mathf.Clamp01(startVol);
            yield return null;
        }
    }

    private IEnumerator RaiseAudioSource()
    {
        float startVol = 0f;
        while(startVol < 1)
        {
            startVol -= Time.deltaTime * volumeChangeSpeed;
            audioSource.volume = Mathf.Clamp01(startVol);
            yield return null;
        }
    }

    public void ActivateAudioSource()
    {
        StartCoroutine(RaiseAudioSource());
    }

    public void DectivateAudioSource()
    {
        StartCoroutine(LowerAudioSource());
    }

}
