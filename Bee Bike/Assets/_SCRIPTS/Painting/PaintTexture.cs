using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintTexture : MonoBehaviour
{
    public LayerMask layerMask;

    public float pixelUvX;
    public float pixelUvY;

    public Renderer rend;
    public PaintMe paintMe;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        RaycastHit hit;
        if (!Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, layerMask))
        {
            return;
        }


        if (paintMe == null || rend == null){
            rend = hit.transform.GetComponent<Renderer>();
            paintMe = hit.transform.GetComponent<PaintMe>();
        }

        MeshCollider meshCollider = hit.collider as MeshCollider;

        if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null || meshCollider == null)
        {
            return;
        }

        Vector2 pixelUV = hit.textureCoord;
        
        // pixelUV.x *= tex.width;
        // pixelUV.y *= tex.height;

        pixelUvX = pixelUV.x;
        pixelUvY = pixelUV.y;

        paintMe.PaintAt(hit.textureCoord);

    }


}
