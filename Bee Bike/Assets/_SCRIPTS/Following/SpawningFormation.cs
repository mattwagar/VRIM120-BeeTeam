using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningFormation : MonoBehaviour
{
    #region Variables

    private static Transform[] spawningPositions;
    private static int formationCounter = -1; // Being initialized at -1 since we increment it before its first use for finding an array element

    #endregion

    #region Unity Methods

    /*
     * Initializes the array holding the spawningPositions by creating an array with a number of elements directly 
     * equal to the number of children this gameObject has. This ensures the array is the proper size to hold 
     * each child gameObject individually.
     */

    private void Start()
    {
        spawningPositions = new Transform[transform.childCount];

        //Debug.Log("Number of Children: " + transform.childCount);

        for (int i = 0; i < transform.childCount; i++)
        {
            spawningPositions[i] = transform.GetChild(i).gameObject.transform;
        }

        GPU_Instancing.setInstantiationArray(spawningPositions);
    }

    #endregion

}
