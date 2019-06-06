using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjData
{
    public Vector3 pos;
    public Vector3 scale;
    public Quaternion rot;

    public Matrix4x4 matrix
    {
        get
        {
            return Matrix4x4.TRS(pos, rot, scale);
        }
    }

    public ObjData(Vector3 pos, Vector3 scale, Quaternion rot)
    {
        this.pos = pos;
        this.scale = scale;
        this.rot = rot;
    }
}



public class GPU_Instancing : MonoBehaviour
{
    //public int instances;
    //public Vector3 maxPos;

    [Header("Mesh Rendering Factors")]
    public Mesh _objMesh;
    public static Mesh objMesh;
    public Material objMaterial;
    public Vector3 instantiationScale = new Vector3(1.0f, 1.0f, 1.0f);

    private List<List<ObjData>> batches = new List<List<ObjData>>();
    private static bool isRunning = true;
    private static Transform[] spawningLocations;
    //public Transform[] spawningLocations;
    //public Transform instantiateLocation;
    //public int flowersToSpawnAtOnce = 3;

    [Header("Flower Spawn and Movement Variables")]
    public float timeBetweenInstantiations = 1.0f;
    public float testMoveSpeed = 0.01f;
    public float initialHeight = 0.0f;
    public float finalHeight = 20.0f;

    // Noise Range Variables
    [Header("Noise for Spawn Locations")]
    public float xNoise = 1.0f;
    public float yNoise = 1.0f;
    public float zNoise = 1.0f;



    private void Start()
    {
        setObjectMesh(_objMesh);

        //int batchIndexNum = 0;
        List<ObjData> currBatch = new List<ObjData>();

        batches.Add(currBatch);

        //for (int i = 0; i < instances; i++)
        //{
        //    AddObj(currBatch, i);
        //    batchIndexNum++;
        //    if (batchIndexNum >= 1000)
        //    {
        //        batches.Add(currBatch);
        //        currBatch = BuildNewBatch();
        //        batchIndexNum = 0;
        //    }
        //}

        //if (batchIndexNum != 0)
        //{
        //    batches.Add(currBatch);
        //    batchIndexNum = 0;
        //}


    }

    private void Update()
    {
        RenderBatches();
        MoveObjects(batches[0]);
        //CreateNewObject(batches[0]);
        if (isRunning == false)
        {
            StartCoroutine(TimedObjectCreation(batches[0]));
        }
    }

    private void RenderBatches()
    {
        foreach (var batch in batches)
        {
            Graphics.DrawMeshInstanced(objMesh, 0, objMaterial, batch.Select(a => a.matrix).ToList());
            /* a => a.matrix:
             * void FuncName (a){
             *      a.matrix;
             * }
             * 
             * batch.Select(a => a.matrix).ToList()
             * satisfies the parameter type:
             * List<Matrix4x4> matrices
             * 
             * batches is a list of lists of ObjData objects
             * so batch is a single list of ObjData objects
             * batch.Select is "selecting" the matrix property of the ObjData a
             * this is then added to a list
             */
        }
    }

    //private void AddObj(List<ObjData> currBatch, int i)
    //{
    //    Vector3 position = new Vector3(Random.Range(-maxPos.x, maxPos.x), Random.Range(-maxPos.y, maxPos.y), Random.Range(-maxPos.z, maxPos.z));
    //    currBatch.Add(new ObjData(position, new Vector3(instantiationScale.x, instantiationScale.y, instantiationScale.z), Quaternion.identity));
    //}

    private List<ObjData> BuildNewBatch()
    {
        return new List<ObjData>();
    }


    /*
     * Takes an entire list of ObjData objects and moves them in the y direction according to the testMoveSpeed
     */
    private void MoveObjects(List<ObjData> batchToMove)
    {
        foreach (var thingy in batchToMove)
        {
            if (thingy.pos.y < finalHeight)
            {
                thingy.pos.y += testMoveSpeed;
            }
        }
    }


    /* 
     * Used to add another object to a list of ObjData at a specified location
     */
    //private void CreateNewObject(List<ObjData> currBatch)
    //{
    //    Vector3 position = instantiateLocation.transform.position;
    //    position = AddNoise(position);
    //    currBatch.Add(new ObjData(position, new Vector3(instantiationScale.x, instantiationScale.y, instantiationScale.z), Quaternion.identity));
    //}


    /* 
     * Used to add multiple objects to a list of ObjData at a specified location at a time
     */
    //private void CreateMultipleNewObjects(List<ObjData> currBatch)
    //{
    //    for (int i = 0; i < flowersToSpawnAtOnce; i++)
    //    {
    //        Vector3 position = instantiateLocation.transform.position;
    //        position = AddNoise(position);
    //        currBatch.Add(new ObjData(position, new Vector3(instantiationScale.x, instantiationScale.y, instantiationScale.z), Quaternion.identity));
    //    }
    //}


    /*
     * Allows for setting of the spawningLocations array
     */
    public static void setInstantiationArray(Transform[] spawningFormation)
    {
        spawningLocations = spawningFormation;

        isRunning = false;
    }

    /*
     * Sets the objMesh to a new mesh so future renders will be a new mesh
     */
    public static void setObjectMesh(Mesh meshToSet)
    {
        objMesh = meshToSet;

        Debug.Log("objMesh set to: " + objMesh.name);
    }

    /* 
     * Used to add multiple objects to a list of ObjData at a specified location at a time using an 
     * array of locations to generally position the objects
     */
    private void CreateMultipleNewObjectsFormation(List<ObjData> currBatch)
    {
        for (int i = 0; i < spawningLocations.Length; i++)
        {
            Vector3 position = spawningLocations[i].transform.position;
            position = AddNoise(position);

            position.y = initialHeight;
            currBatch.Add(new ObjData(position, new Vector3(instantiationScale.x, instantiationScale.y, instantiationScale.z), Quaternion.identity));
        }
    }


    /* 
     * Allows for timed use of the CreateNewObject method
     */
    private IEnumerator TimedObjectCreation(List<ObjData> currBatch)
    {
        isRunning = true;

        yield return new WaitForSeconds(timeBetweenInstantiations);
        //CreateNewObject(currBatch);
        //CreateMultipleNewObjects(currBatch);
        CreateMultipleNewObjectsFormation(currBatch);

        isRunning = false;
    }

    private Vector3 AddNoise(Vector3 vect)
    {
        vect += new Vector3(Random.Range(-xNoise, xNoise),
            Random.Range(-yNoise, yNoise),
            Random.Range(-zNoise, zNoise));

        return vect;
    }
}
