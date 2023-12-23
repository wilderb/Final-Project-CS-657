using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PipeSocket : MonoBehaviour
{
    public FillUp pipe = null;
    public enum pipeSocketType { forward, backward };
    public pipeSocketType socket = pipeSocketType.forward;
    public FillUp potentialPrev;
    public FillUp potentialEnd;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<PipeSocket>() != null)
        {
            if (socket == pipeSocketType.backward && other.GetComponent<PipeSocket>().socket == pipeSocketType.forward)
            {
                potentialPrev = other.GetComponent<PipeSocket>().pipe;
            }
            if (socket == pipeSocketType.forward && other.GetComponent<PipeSocket>().pipe.type == FillUp.PipeType.end)
            {
                potentialEnd = other.GetComponent<PipeSocket>().pipe;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<FillUp>() == potentialPrev)
        {
            potentialPrev = null;
        }
        if (other.gameObject.GetComponent<FillUp>() == potentialEnd)
        {
            potentialEnd = null;
        }
    }
}
