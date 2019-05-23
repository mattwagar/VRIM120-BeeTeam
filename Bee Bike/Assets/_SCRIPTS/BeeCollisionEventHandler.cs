using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeCollisionEventHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) 
    {
        if(other.GetComponent<BeeAudioManager>() != null)
        {
            other.GetComponent<BeeAudioManager>().ActivateAudioSource();
        }
    }
}