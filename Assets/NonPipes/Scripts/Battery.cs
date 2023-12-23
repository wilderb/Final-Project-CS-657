using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour
{
    public GameObject dummy;
    public MeshRenderer mesh1;
    public MeshRenderer mesh2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Drop(Vector3 velocity)
    {
        this.gameObject.SetActive(false);
    }
}
