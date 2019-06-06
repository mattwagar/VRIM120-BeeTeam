using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PesticideActivation : MonoBehaviour
{
    Collider collider;
    public GameObject objectToActivate;


    private void Start()
    {
        collider = GetComponent<Collider>();

    }

    /*
     * If the player collides with this object, the attached object will activate
     */
    void OnTriggerEnter(Collider collision)
    {
        Debug.Log(collision.transform.name);
        if (collision.gameObject.name == "Bike")
        {
            Transform beeController = collision.transform.Find("BeeController");
            Animator BeeAnimator = beeController.GetComponent<Animator>();
            BeeAnimator.SetTrigger("DamageTrigger");
            // objectToActivate.SetActive(true);
            Debug.Log("Player entered pesticide cloud.");
        }
    }


    /*
     * If the player leaves the object, the attached object will deactivate
     */
    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // objectToActivate.SetActive(false);
            Debug.Log("Player exited pesticide cloud.");
        }
    }

}
