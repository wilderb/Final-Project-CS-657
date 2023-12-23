using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spill : MonoBehaviour
{
    public Mesh[] meshes = new Mesh[0];
    public GameObject waterMesh = null;
    public float incrementTime = 0.5f;
    private int meshIndex = 0;
    private float timeToNext = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        IncrementWaterMesh();
    }
    private void IncrementWaterMesh()
    {
        timeToNext -= Time.deltaTime;
        if (timeToNext <= 0.0 && meshIndex < meshes.Length)
        {
            waterMesh.GetComponent<MeshFilter>().mesh = meshes[meshIndex];
            meshIndex++;
            timeToNext = incrementTime;
        }
    }
}
