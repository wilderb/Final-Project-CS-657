using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PsychicGrip: MonoBehaviour
{
    public Transform physicalController;
    public Transform headLocation;
    public Transform leftHandLocation;
    public GeneralGrab grabber;
    private bool active = false;
    public GameObject hpTracker;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = physicalController.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            hpTracker.SetActive(false);

            Vector3 headToController = physicalController.position - headLocation.position;

            float distance = Vector3.Distance(headLocation.position, physicalController.position) * 2f;
            if (distance > 1f)
            {
                transform.position = headLocation.position + (headToController * Mathf.Pow(distance, 11));
            }
            else
                transform.position = headLocation.position + (headToController);
        }
        

        if (Vector3.Distance(headLocation.position, leftHandLocation.position) < 0.25 && OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger) > 0.9f)
        {
            active = true;
        }
        else
        {
            if (active)
            {
                TurnOff();
            }
        }
    }

    public void OnDisable()
    {
        this.transform.position = physicalController.transform.position;
    }

    public void TurnOff()
    {
        if (active)
        {
            if (grabber.heldItem != null)
            {
                grabber.DropHeldItem(0.025f);
            }
            this.transform.position = physicalController.transform.position;
            hpTracker.SetActive(true);
            active = false;
        }
    }
}
