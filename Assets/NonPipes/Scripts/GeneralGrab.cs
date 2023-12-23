using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralGrab : MonoBehaviour
{
    public enum Hand { right, left };
    public Hand hand = Hand.right;
    public GameObject heldItem = null;
    private Vector3 lastPosition = Vector3.zero;
    private Vector3 secondLastPosition = Vector3.zero;
    private float vibrationTime = 0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (heldItem != null)
        {
            // this checks if the object has been picked up with the other hand, so it stops moving it on this hand and wont cause it to drop if this hand releases the trigger
            if (heldItem.transform.parent != this.transform)
                heldItem = null;

            if ((OVRInput.Get(OVRInput.RawAxis1D.RHandTrigger) < 0.1f) && hand == Hand.right ||
                (OVRInput.Get(OVRInput.RawAxis1D.LHandTrigger) < 0.1f) && hand == Hand.left)
            {
                DropHeldItem(1);
            }
            else
            {
                // stores the last two positions of the object, to use when calculating velocity on release
                secondLastPosition = lastPosition;
                lastPosition = heldItem.transform.position;
            }
        }

        // vibrations
        if (vibrationTime > 0.0)
        {
            if (hand == Hand.right)
                OVRInput.SetControllerVibration(1f, vibrationTime * 3, OVRInput.Controller.RTouch);
            else
                OVRInput.SetControllerVibration(1f, vibrationTime * 3, OVRInput.Controller.LTouch);
            vibrationTime -= Time.deltaTime;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (heldItem == null)
        {

            if (other.tag.Equals("grabbable") && ((OVRInput.Get(OVRInput.RawAxis1D.RHandTrigger) > 0.9 && hand == Hand.right) ||
                (OVRInput.Get(OVRInput.RawAxis1D.LHandTrigger) > 0.9 && hand == Hand.left)))
            {
                bool accepted = true;
                heldItem = other.gameObject;
                FillUp filler = heldItem.GetComponent<FillUp>();
                if (heldItem.GetComponent<FillUp>() != null)
                {

                    if (filler.next == null && filler.fillingUp == false)
                    {
                        heldItem.GetComponent<FillUp>().PickUp();
                    }
                    else
                    {
                        accepted = false;
                    }
                }
                if (accepted)
                {
                    Vibrate(1);
                    heldItem.transform.parent = this.transform;
                    heldItem.GetComponent<Rigidbody>().isKinematic = true;
                    lastPosition = heldItem.transform.position;
                    secondLastPosition = lastPosition;
                }
            }
        }
        else if (heldItem.tag != "grabbable")
        {
            DropHeldItem(1);
        }
    }

    public void DropHeldItem(float velocityMultiplier)
    {
        if (heldItem.GetComponent<FillUp>() != null)
        {
            heldItem.GetComponent<FillUp>().Drop((lastPosition - secondLastPosition) / Time.deltaTime * velocityMultiplier);
        }
        else
        {
            heldItem.transform.parent = null;
            heldItem.GetComponent<Rigidbody>().isKinematic = false;
            heldItem.GetComponent<Rigidbody>().velocity = (lastPosition - secondLastPosition) / Time.deltaTime * velocityMultiplier;
        }
        heldItem = null;
    }

    public void Vibrate(float timeLength)
    {
        vibrationTime = timeLength;
    }
}
