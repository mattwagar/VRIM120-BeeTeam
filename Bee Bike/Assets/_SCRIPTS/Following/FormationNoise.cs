using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationNoise : MonoBehaviour
{
    #region Variables

    [Header("Buzziness Noise")]
    public float randomRangeMin = 0.0f;
    public float randomRangeMax = 1.0f;

    //public enum Motion
    //{
    //    AllRandom,
    //    XRandom,
    //    SwayRandom
    //}
    //public Motion motion;

    [Header("Sway Parameters")]
    public float swayX = 0.0f;
    public float swaySpeedX = 1.0f;
    public float swayY = 0.0f;
    public float swaySpeedY = 1.0f;

    private Vector3 initialLocation;
    private bool turnX = true;
    private bool turnY = true;
    #endregion


    #region Unity Methods

    private void Start()
    {
        initialLocation = transform.localPosition;

        RandomizeStartingPosition();
    }

    private void FixedUpdate()
    {
        //switch (motion)
        //{
        //    case Motion.AllRandom:
        //        NoiseMovement();
        //        break;
        //    case Motion.XRandom:
        //        NoiseMovementxFocused();
        //        break;
        //    case Motion.SwayRandom:
        //        NoiseMovementGuided();
        //        break;
        //    default:
        //        break;
        //}
        NoiseMovementGuided();
    }

    /*
     * Moves this object's position randomly within a fixed range
     */
    private void NoiseMovement()
    {
        Vector3 randomPosition = initialLocation +
            new Vector3(Random.Range(randomRangeMin, randomRangeMax),
            Random.Range(randomRangeMin, randomRangeMax),
            Random.Range(randomRangeMin, randomRangeMax));

        //transform.position = Vector3.Lerp(transform.position, leader.transform.position, Time.deltaTime * followSpeed);
        transform.localPosition = randomPosition;
    }


    /*
     * Moves this object's x position randomly within a fixed range
     */
    private void NoiseMovementxFocused()
    {
        Vector3 randomPosition = initialLocation +
            new Vector3(Random.Range(randomRangeMin, randomRangeMax),
            0f,
            0f);

        transform.localPosition = randomPosition;
    }


    /*
     * Moves this object's position randomly, with additional swinging back and forth in x
     */
    private void NoiseMovementGuided()
    {
        // Reverse swaying direction of object once it hits a maximum
        if ((transform.localPosition.x > initialLocation.x + swayX) || (transform.localPosition.x < initialLocation.x - swayX))
        {
            turnX = !turnX;
        }

        // Reverse swaying direction of object once it hits a maximum
        if ((transform.localPosition.y > initialLocation.y + swayY) || (transform.localPosition.y < initialLocation.y - swayY))
        {
            turnY = !turnY;
        }

        float xGuide = SwayMotionX();
        float yGuide = SwayMotionY();

        Vector3 randomPosition = new Vector3(xGuide + transform.localPosition.x,
            yGuide + transform.localPosition.y,
            Random.Range(randomRangeMin, randomRangeMax) + initialLocation.z);

        transform.localPosition = randomPosition;
    }

    private float SwayMotionX()
    {
        float swayLocation;

        if (turnX)
        {
            swayLocation = Time.deltaTime * swaySpeedX;
        }
        else
        {
            swayLocation = -Time.deltaTime * swaySpeedX;
        }

        return swayLocation;
    }


    private float SwayMotionY()
    {
        float swayLocation;

        if (turnY)
        {
            swayLocation = Time.deltaTime * swaySpeedY;
        }
        else
        {
            swayLocation = -Time.deltaTime * swaySpeedY;
        }

        return swayLocation;
    }


    /*
     * Initialize each position in a bit of a randomized position within the sway constraints
     * so they do not all move together as uniformly
     */
    private void RandomizeStartingPosition()
    {
        float maxRangeX = transform.localPosition.x + swayX;
        float maxRangeY = transform.localPosition.y + swayY;

        transform.localPosition = new Vector3(Random.Range(transform.localPosition.x, maxRangeX),
            Random.Range(transform.localPosition.y, maxRangeY),
            transform.localPosition.z);

        Debug.Log(gameObject.name + " had its initial location randomized to: " + transform.localPosition);
    }

    #endregion
}
