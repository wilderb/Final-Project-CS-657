using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Signs : MonoBehaviour
{
    public string displayText;
    public TextMeshPro textMesh;
    public Color unselectedColor;
    public Color selectedColor;
    public Color pressedColor;
    public Color disabledColor;
    public bool pressed = false;
    public float resetTime = 1;
    private float timer = 0;
    public GameObject triggeredObject;
    public bool poweredOn;

    // Start is called before the first frame update
    void Start()
    {
        if (poweredOn)
        {
            textMesh.text = displayText;
            textMesh.color = unselectedColor;
        }
        else
        {
            textMesh.text = "No Power";
            textMesh.color = disabledColor;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pressed)
        {
            Timer();
        }
    }

    public void PowerOn()
    {
        textMesh.text = displayText;
        textMesh.color = unselectedColor;
        poweredOn = true;
    }

    public void PressButton()
    {
        if (poweredOn && !pressed)
        {
            pressed = true;
            textMesh.color = pressedColor;
            timer = resetTime;
            if (triggeredObject != null)
            {
                if (triggeredObject.GetComponentInChildren<PipeSpawner>() != null)
                {
                    triggeredObject.GetComponentInChildren<PipeSpawner>().spawnPipe();
                }
                else if (triggeredObject.GetComponentInChildren<Door>() != null)
                {
                    triggeredObject.GetComponentInChildren<Door>().OpenClose();
                }
                else if (triggeredObject.GetComponentInChildren<FillUp>() != null)
                {
                    triggeredObject.GetComponentInChildren<FillUp>().StartFilling();
                }
                else if (triggeredObject.GetComponentInChildren<PipeTracker>() != null)
                {
                    triggeredObject.GetComponentInChildren<PipeTracker>().DestroyAllPipes();
                }
            }
        }
    }

    private void Timer()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer < (resetTime / 2))
            {
                textMesh.color = unselectedColor;
            }
            if (timer <= 0)
            {
                pressed = false;
            }
        }
    }
}
