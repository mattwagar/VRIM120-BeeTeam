using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    #region Variables

    private Rigidbody rb;
    private Collider collider;
    private GameObject leader;

    private bool isFollowing = false;

    public float followSpeed = 4f;
    public float stoppingDistance = 2.0f;

    #endregion



    #region Unity Methods

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }

    /*
     * If the player collides with this object, it will begin following them in some manner, determined 
     * by the FixedUpdate method.
     */
    void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            isFollowing = true;
            collider.enabled = false;
            leader = Leader.GetFormationPosition();
        }
    }

    void FixedUpdate()
    {
        if (isFollowing)
        {
            FollowPlayerPositionLerp();
        }
    }

    /* ==================================================================================================
     * The following "Follow" methods are various options to be used in the FixedUpdate of this object to 
     * determine how it will follow a leader object. Once it is decided which movement type is best, all 
     * others may be removed.
     */


    /* 
     * This object will turn towards its leader and move towards the leader's current location, as long 
     * as it is not closer than the stoppingDistance
     */
    void FollowPlayer()
    {
        if (Vector3.Distance(transform.position, leader.transform.position) < stoppingDistance)
        {
            rb.velocity = Vector3.zero;
        }
        else
        {
            transform.LookAt(leader.transform);
            rb.velocity = transform.forward * followSpeed;
        }
    }


    /* 
     * This object's position will be assigned that of its leader
     */
    void FollowPlayerPosition()
    {
        transform.position = leader.transform.position;
    }


    /*
     * Similar to FollowPlayerPostion, but uses a lerp function to smooth the movement between the  
     * different positions.
     */
    void FollowPlayerPositionLerp()
    {
        transform.position = Vector3.Lerp(transform.position, leader.transform.position, Time.deltaTime * followSpeed);
    }

    #endregion

}