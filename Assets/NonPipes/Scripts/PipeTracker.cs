using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeTracker : MonoBehaviour
{
    public List<FillUp> pipes = new List<FillUp>();
    public FillUp startingPipe;
    public FillUp endingPipe;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DestroyAllPipes()
    {
        if (endingPipe.spilling != true)
        {
            FillUp[] arr = pipes.ToArray();
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i].DestroyPipe();
            }
            pipes.Clear();
        }
        startingPipe.ResetPipes();
    }
}
