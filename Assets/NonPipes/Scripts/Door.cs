using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpen;
    public GameObject doorMesh;
    public Transform upPosition;
    public Transform downPosition;

    private bool opening;
    private bool closing;
    private bool canMove = true;
    
    public float doorSpeed = 1.5f;
    private float startTime;
    private float length;

    // Start is called before the first frame update
    void Start()
    {

        length = Vector3.Distance(downPosition.position, upPosition.position);

        if (isOpen)
        {
            doorMesh.transform.position = upPosition.position;
        }
        else
        {
            doorMesh.transform.position = downPosition.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (opening)
        {
            float distCovered = (Time.time - startTime) * doorSpeed;
            float fractionOfJourney = distCovered / length;
            doorMesh.transform.position = Vector3.Lerp(downPosition.position, upPosition.position, fractionOfJourney);
            if (doorMesh.transform.position == upPosition.position)
            {
                opening = false;
                canMove = true;
            }
        }
        else if (closing)
        {
            float distCovered = (Time.time - startTime) * doorSpeed;
            float fractionOfJourney = distCovered / length;
            doorMesh.transform.position = Vector3.Lerp(upPosition.position, downPosition.position, fractionOfJourney);
            if (doorMesh.transform.position == downPosition.position)
            {
                closing = false;
                canMove = true;
            }
        }
        
    }

    public void OpenClose()
    {
        if (canMove)
        {
            startTime = Time.time;
            if (isOpen)
            {
                closing = true;
                isOpen = false;
            }
            else
            {
                opening = true;
                isOpen = true;
            }
            canMove = false;
        }
    }
}
