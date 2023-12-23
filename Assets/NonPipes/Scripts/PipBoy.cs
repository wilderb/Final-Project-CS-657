using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PipBoy : MonoBehaviour
{
    public TextMeshPro countdownText;
    public int minutesRemaining = 30;
    public int secondsToNextMinute = 00;
    private float secondCounter = 1f;

    public LineRenderer lineRenderer;
    public Transform laserSightTransform;
    public Material unselectedMat;
    public Material selectedMat;

    private bool isPointing;
    public GameObject selectedObject;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        countdownText.text = minutesRemaining + ":" + secondsToNextMinute.ToString("D2");
        Countdown();

        isPointing = (!OVRInput.Get(OVRInput.NearTouch.PrimaryIndexTrigger) && OVRInput.Get(OVRInput.RawAxis1D.LHandTrigger) > 0.6);

        if (isPointing)
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, laserSightTransform.position);

            RaycastHit hit;

            if (Physics.Raycast(laserSightTransform.position, transform.forward, out hit))
            {
                if (hit.collider.gameObject.GetComponentInParent<Signs>() != null)
                {
                    Signs sign = hit.collider.gameObject.GetComponentInParent<Signs>();
                    if (sign.poweredOn && sign.pressed == false)
                    {
                        sign.textMesh.color = sign.selectedColor;
                    }
                    lineRenderer.SetPosition(1, hit.point);
                    lineRenderer.material = selectedMat;
                    selectedObject = hit.collider.gameObject;
                    if (OVRInput.GetDown(OVRInput.RawButton.Y) && sign.poweredOn)
                    {
                        sign.PressButton();
                    }
                }
                else
                {
                    if (selectedObject != null)
                    {
                        if (selectedObject.GetComponentInParent<Signs>() != null)
                        {
                            Signs sign = selectedObject.GetComponentInParent<Signs>();
                            if (sign.poweredOn && sign.pressed == false)
                            {
                                sign.textMesh.color = sign.unselectedColor;
                            }
                        }
                    }
                    selectedObject = null;
                    lineRenderer.material = unselectedMat;
                    lineRenderer.SetPosition(1, transform.forward * 100);
                }
            }
            else
            {
                if (selectedObject != null)
                {
                    if (selectedObject.GetComponentInParent<Signs>() != null)
                    {
                        Signs sign = selectedObject.GetComponentInParent<Signs>();
                        if (sign.poweredOn && sign.pressed == false)
                        {
                            sign.textMesh.color = sign.unselectedColor;
                        }
                    }
                    selectedObject = null;
                    lineRenderer.material = unselectedMat;
                }
                lineRenderer.SetPosition(1, transform.forward * 100);
            }
        }
        else
        {
            if (selectedObject != null)
                {
                    if (selectedObject.GetComponentInParent<Signs>() != null)
                    {
                        Signs sign = selectedObject.GetComponentInParent<Signs>();
                        if (sign.pressed == false)
                        {
                            sign.textMesh.color = sign.unselectedColor;
                        }
                    }
                    selectedObject = null;
                    lineRenderer.material = unselectedMat;
                }
            lineRenderer.enabled = false;
        }
    }

    public void Countdown()
    {
        if (secondsToNextMinute == 0)
        {
            minutesRemaining--;
            secondsToNextMinute = 60;
        }
        secondCounter -= Time.deltaTime;
        if (secondCounter <= 0)
        {
            secondsToNextMinute--;
            secondCounter = 1;
        }
    }
}
