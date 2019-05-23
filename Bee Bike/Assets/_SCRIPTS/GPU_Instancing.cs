﻿using System.Collections;
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
    public int instances;
    public Vector3 maxPos;
    public Mesh objMesh;
    public Material objMaterial;

    public Vector3 instantiationScale = new Vector3(1.0f, 1.0f, 1.0f);

    private List<List<ObjData>> batches = new List<List<ObjData>>();

    private void Start()
    {
        int batchIndexNum = 0;
        List<ObjData> currBatch = new List<ObjData>();

        for (int i = 0; i < instances; i++)
        {
            AddObj(currBatch, i);
            batchIndexNum++;
            if(batchIndexNum >= 1000)
            {
                batches.Add(currBatch);
                currBatch = BuildNewBatch();
                batchIndexNum = 0;
            }
        }

        if(batchIndexNum != 0)
        {
            batches.Add(currBatch);
            batchIndexNum = 0;
        }
    }

    private void Update()
    {
        RenderBatches();
    }

    private void RenderBatches()
    {
        foreach(var batch in batches)
        {
            Graphics.DrawMeshInstanced(objMesh, 0, objMaterial, batch.Select((a) => a.matrix).ToList());
        }
    }

    private void AddObj(List<ObjData> currBatch, int i)
    {
        Vector3 position = new Vector3(Random.Range(-maxPos.x, maxPos.x), Random.Range(-maxPos.y, maxPos.y), Random.Range(-maxPos.z, maxPos.z));
        currBatch.Add(new ObjData(position, new Vector3(instantiationScale.x, instantiationScale.y, instantiationScale.z), Quaternion.identity));
    }

    private List<ObjData> BuildNewBatch()
    {
        return new List<ObjData>();
    }


}
