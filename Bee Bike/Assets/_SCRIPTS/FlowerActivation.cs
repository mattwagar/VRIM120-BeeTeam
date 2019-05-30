using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerActivation : MonoBehaviour
{
    Collider collider;

    private void Start()
    {
        collider = GetComponent<Collider>();
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

    private void FlowerAction()
    {
        Debug.Log("Flower activated.");
    }
}
