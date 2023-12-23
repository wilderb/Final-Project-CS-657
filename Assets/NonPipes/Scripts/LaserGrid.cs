using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGrid : MonoBehaviour
{
    public MeshRenderer laser;
    public GameObject start;
    public GameObject end;
    public bool active = true;
    public bool blinking = false;
    public float blinkTime = 3f;
    private float currentCountdown = 0f;
    public float initialOffsetTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        laser.enabled = active;
    }

    // Update is called once per frame
    void Update()
    {
        if (blinking)
        {
            if (initialOffsetTime > 0f)
            {
                initialOffsetTime -= Time.deltaTime;
            }
            else
            {
                if (currentCountdown < blinkTime)
                {
                    currentCountdown += Time.deltaTime;
                }
                else
                {
                    active = !active;
                    laser.enabled = active;
                    currentCountdown = 0f;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (active && other.gameObject.GetComponentInChildren<Battery>() != null)
        {
            other.gameObject.GetComponentInChildren<Battery>().Drop(Vector3.zero);
        }
    }
}
