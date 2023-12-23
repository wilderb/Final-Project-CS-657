using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    public GameObject straight;
    public GameObject bend;
    public GameObject bulb;
    public int straightOdds = 4;
    public int bendOdds = 7;
    public int bulbOdds = 8;
    public PipeTracker pipeTracker;
    public float speedMultiplier = 1;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void spawnPipe()
    {
        int roll = Random.Range(1, 9);

        GameObject pipe;

        if (roll <= straightOdds)
        {
            pipe = GameObject.Instantiate(straight, transform.position, Quaternion.identity);
        }
        else if (roll <= bendOdds)
        {
            pipe = GameObject.Instantiate(bend, transform.position, Quaternion.identity);
        }
        else
        {
            pipe = GameObject.Instantiate(bulb, transform.position, Quaternion.identity);
        }

        pipe.GetComponent<FillUp>().incrementTime *= (1 / speedMultiplier);
        pipeTracker.pipes.Add(pipe.GetComponent<FillUp>());
    }
}
