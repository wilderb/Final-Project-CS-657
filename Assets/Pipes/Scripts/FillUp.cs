using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class FillUp : MonoBehaviour
{
    public Mesh[] meshes = new Mesh[0];
    public GameObject waterMesh = null;
    public FillUp prev = null;
    public FillUp next = null;
    public float incrementTime = 0.5f;
    public GameObject previousSocket = null;
    public GameObject nextSocket = null;
    private int meshIndex = 0;
    private float timeToNext = 0;
    public bool fillingUp = false;
    public GameObject dummyMid = null;
    public GameObject dummyAhead = null;
    public GameObject spillUp;
    public GameObject spillDown;
    public GameObject spillSideways;
    public Vector3 place;
    public Vector3 nextPlace;
    public Vector3 diff;
    public bool spilling;
    private GameObject spill;
    public PipeEndActivator activatorEnd;
    public enum PipeType { straight, bend, start, end};
    public PipeType type = PipeType.straight;

    // Start is called before the first frame update
    void Start()
    {
        waterMesh.GetComponent<MeshFilter>().mesh = meshes[0];
        
        if (prev != null)
        {
            if (GetComponent<Rigidbody>() != null)
                GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    // Update is called once per frame
    void Update()
    { 
        place = dummyMid.transform.localToWorldMatrix.GetPosition();
        nextPlace = dummyAhead.transform.localToWorldMatrix.GetPosition();
        diff = (place - nextPlace);
        if (fillingUp)
        {
            IncrementWaterMesh();
        }
    }

    private void IncrementWaterMesh()
    {
        timeToNext -= Time.deltaTime;
        if (timeToNext <= 0.0 && meshIndex < meshes.Length)
        {
            waterMesh.GetComponent<MeshFilter>().mesh = meshes[meshIndex];
            meshIndex++;
            timeToNext = incrementTime;
        }
        if (meshIndex == meshes.Length - 1)
        {
            if (next != null)
            {
                next.fillingUp = true;
            }
            else
            {
                if (!spilling)
                {
                    spilling = true;

                    if (type != PipeType.end)
                    {
                        if (diff.y > 0.2)
                        {
                            spill = GameObject.Instantiate<GameObject>(spillDown, nextSocket.transform.position, Quaternion.Euler(0, 0, 0));
                        }
                        else if (diff.y < -0.2)
                        {
                            spill = GameObject.Instantiate<GameObject>(spillUp, nextSocket.transform.position, Quaternion.Euler(180, 0, 0));
                        }
                        else
                        {
                            if (diff.x > 0.2)
                                spill = GameObject.Instantiate<GameObject>(spillSideways, nextSocket.transform.position, Quaternion.Euler(0, 0, -90));
                            else if (diff.x < -.02)
                                spill = GameObject.Instantiate<GameObject>(spillSideways, nextSocket.transform.position, Quaternion.Euler(0, 180, -90));
                            else if (diff.z > 0.2)
                                spill = GameObject.Instantiate<GameObject>(spillSideways, nextSocket.transform.position, Quaternion.Euler(0, 270, -90));
                            else
                                spill = GameObject.Instantiate<GameObject>(spillSideways, nextSocket.transform.position, Quaternion.Euler(0, 90, -90));
                        }
                    }
                    else
                    {
                        activatorEnd.TurnOnSigns();
                    }
                }
            }
        }
    }

    public void Drop(Vector3 velocity)
    {
        bool successful = false;
        if (previousSocket.GetComponent<PipeSocket>().potentialPrev != null)
        {
            prev = previousSocket.GetComponent<PipeSocket>().potentialPrev;

            if ((prev.prev != null || prev.type == PipeType.start)
                && (Vector3.Distance(dummyMid.transform.position, prev.dummyAhead.transform.position) < 0.05f)
                && (prev.spilling == false))
            {
                successful = true;
                prev.next = this;
            }
            else
            {
                detachFromPipe();
            }

            if (successful)
            {
                transform.position = prev.nextSocket.transform.position;
                transform.parent = prev.transform;
                float xAngle, yAngle, zAngle = 0f;

                //float yRot = transform.localRotation.eulerAngles.y;
                //if (yRot >= 45f && yRot < 135f)
                //{
                //    yAngle = 90f;
                //}
                //else if (yRot >= 135f && yRot < 225f)
                //{
                //    yAngle = 180f;
                //}
                //else if (yRot >= 225f && yRot < 315f)
                //{
                //    yAngle = 270f;
                //}
                //else
                //{
                //    yAngle = 0f;
                //}

                //transform.localRotation = Quaternion.Euler(prev.nextSocket.transform.localRotation.eulerAngles.x - 90f, yAngle, prev.nextSocket.transform.localRotation.eulerAngles.z);


                if (transform.rotation.eulerAngles.x >= 45f && transform.rotation.eulerAngles.x < 135f)
                    xAngle = 90f;
                else if (transform.rotation.eulerAngles.x >= 135f && transform.rotation.eulerAngles.x < 225f)
                    xAngle = 180f;
                else if (transform.rotation.eulerAngles.x >= 225f && transform.rotation.eulerAngles.x < 315f)
                    xAngle = 270;
                else
                    xAngle = 0f;


                if (transform.rotation.eulerAngles.y >= 45f && transform.rotation.eulerAngles.y < 135f)
                    yAngle = 90f;
                else if (transform.rotation.eulerAngles.y >= 135f && transform.rotation.eulerAngles.y < 225f)
                    yAngle = 180f;
                else if (transform.rotation.eulerAngles.y >= 225f && transform.rotation.eulerAngles.y < 315f)
                    yAngle = 270;
                else
                    yAngle = 0f;


                if (transform.rotation.eulerAngles.z >= 45f && transform.rotation.eulerAngles.z < 135f)
                    zAngle = 90f;
                else if (transform.rotation.eulerAngles.z >= 135f && transform.rotation.eulerAngles.z < 225f)
                    zAngle = 180f;
                else if (transform.rotation.eulerAngles.z >= 225f && transform.rotation.eulerAngles.z < 315f)
                    zAngle = 270;
                else
                    zAngle = 0f;

                transform.rotation = Quaternion.Euler(xAngle, yAngle, zAngle);
            }
            if (nextSocket.GetComponent<PipeSocket>().potentialEnd != null)
            {
                if (Vector3.Distance(dummyAhead.transform.position, nextSocket.GetComponent<PipeSocket>().potentialEnd.dummyMid.transform.position) < 0.05f)
                {
                    next = nextSocket.GetComponent<PipeSocket>().potentialEnd;
                    next.prev = this;
                }
            }
        }
        else
        {
            detachFromPipe();
        }

        void detachFromPipe()
        {
            successful = false;
            prev = null;
            transform.parent = null;
            this.GetComponent<Rigidbody>().isKinematic = false;
            this.GetComponent<Rigidbody>().velocity = velocity;
        }
    }

    public void PickUp()
    {
        if (prev != null)
            prev.next = null;
        prev = null;
    }

    public void StartFilling()
    {
        if (!fillingUp)
        {
            waterMesh.GetComponent<MeshFilter>().mesh = meshes[0];
            meshIndex = 0;
            fillingUp = true;
            timeToNext = 0;
        }
    }

    public void ResetPipes()
    {
        waterMesh.GetComponent<MeshFilter>().mesh = meshes[0];
        meshIndex = 0;
        fillingUp = false;
        spilling = false;
        //if (next != null)
        //{
        //    next.DestroyPipe();
        //    next = null;
        //}
        if (spill != null)
        {
            Destroy(spill);
            spill = null;
        }
    }

    public void DestroyPipe()
    {
        if (next != null)
        {
            next.DestroyPipe();
            next = null;
        }
        if (spill != null)
        {
            Destroy(spill);
            spill = null;
        }
        Destroy(this.gameObject);
    }
}
