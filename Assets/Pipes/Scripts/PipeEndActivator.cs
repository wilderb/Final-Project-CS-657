using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeEndActivator : MonoBehaviour
{
    public Signs[] signs = new Signs[0];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TurnOnSigns()
    {
        foreach (Signs s in signs)
        {
            s.PowerOn();
        }
    }
}
