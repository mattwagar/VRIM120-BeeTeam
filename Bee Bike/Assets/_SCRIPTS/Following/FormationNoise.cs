using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationNoise : MonoBehaviour
{
    #region Variables

    public float randomRangeMin = 0.0f;
    public float randomRangeMax = 1.0f;

    public enum Motion
    {
        AllRandom,
        XRandom,
        SwayRandom
    }
    public Motion motion;

    public float sway = 0.0f;
    public float swaySpeed = 1.0f;

    private Vector3 initialLocation;
    private bool turn = true;
    #endregion


    #region Unity Methods

    private void Start()
    {
        //initialLocation = transform.position;
        initialLocation = transform.localPosition;
    }

    private void FixedUpdate()
    {
        switch (motion)
        {
            case Motion.AllRandom:
                NoiseMovement();
                break;
            case Motion.XRandom:
                NoiseMovementxFocused();
                break;
            case Motion.SwayRandom:
                NoiseMovementGuided();
                break;
            default:
                break;
        }
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
        //float xGuide = sway * Mathf.Cos(Time.deltaTime * swaySpeed);

        //Vector3 randomPosition = initialLocation +
        //    new Vector3(Random.Range(randomRangeMin, randomRangeMax) + xGuide,
        //    Random.Range(randomRangeMin, randomRangeMax),
        //    Random.Range(randomRangeMin, randomRangeMax));

        // Reverse swaying direction of object once it hits a maximum
        if ((transform.localPosition.x > initialLocation.x + sway) || (transform.localPosition.x < initialLocation.x - sway))
        {
            turn = !turn;
        }

        float xGuide = SwayMotion();

        Vector3 randomPosition = new Vector3(xGuide + transform.localPosition.x,
            Random.Range(randomRangeMin, randomRangeMax) + initialLocation.y,
            Random.Range(randomRangeMin, randomRangeMax) + initialLocation.z);

        transform.localPosition = randomPosition;
    }

    private float SwayMotion()
    {
        float swayLocation;

        if (turn)
        {
            swayLocation = Time.deltaTime * swaySpeed;
        }
        else
        {
            swayLocation = -Time.deltaTime * swaySpeed;
        }

        return swayLocation;
    }

    #endregion
}
