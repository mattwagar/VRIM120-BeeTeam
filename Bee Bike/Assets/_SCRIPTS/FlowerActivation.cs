using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerActivation : MonoBehaviour
{
    Collider collider;
    public GameObject particleEffect;

    public AudioClip flowerSound;
    AudioSource soundSource;

    private void Start()
    {
        collider = GetComponent<Collider>();

        soundSource.clip = flowerSound;
    }

    /*
     * If the player collides with this object, the flower action will happen.
     */
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collider.enabled = false;
            FlowerAction();
        }
    }


    /*
     * General actions the flower will perform: activate a particle effect and play a sound clip
     */
    private void FlowerAction()
    {
        particleEffect.SetActive(true);

        soundSource.PlayOneShot(flowerSound);

        Debug.Log("Flower activated.");
    }
}
