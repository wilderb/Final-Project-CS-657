using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    public float rotationSpeed = 45f;
    public bool spinning;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (spinning)
        {
            float rotation = rotationSpeed * Time.deltaTime;
            transform.Rotate(Vector3.up, rotation, Space.Self);
        }
    }
}
