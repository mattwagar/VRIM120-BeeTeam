using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    #region Variables

    private bool gameEnded = false;

    public float gameDuration = 3.0f;
    private float gameTime;

    //public GameObject fadeSphere;
    //public Material fadeMaterial;
    public Renderer rend;
    public float fadeTime = 2.0f;

    #endregion

    #region Unity Methods

    private void Start()
    {
        gameTime = 0f;

        //fadeMaterial = fadeSphere.gameObject.GetComponent<Material>();
    }

    private void Update()
    {
        gameTime += Time.deltaTime;

        TrackTime();
    }


    /*
     * Keeps track of current in game play time and ends game when the overall set duration is met
     */
    private void TrackTime()
    {
        if(gameTime >= gameDuration && gameEnded == false)
        {
            EndGame();
        }

        //gameTime += Time.deltaTime;

    }


    /*
     * Performs the actions indicating the game has ended
     */
    private void EndGame()
    {
        gameEnded = true;

        StartCoroutine(FadeOutTest());

        // Actions to perform to end game
        Debug.Log("End Game");
    }


    /*
     * Change alpha on black sphere to fade out for VR scene
     */
    private IEnumerator FadeOutTest()
    {
        Debug.Log("FadeOutTest Started");

        for(float f = 0f; f <= 1; f += 0.05f)
        {
            Color fadeColor = rend.material.color;
            fadeColor.a = f;
            rend.material.color = fadeColor;
            yield return new WaitForSeconds(fadeTime/20f);
        }
    }

    #endregion
}
