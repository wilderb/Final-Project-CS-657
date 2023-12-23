using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchJoystickMove : MonoBehaviour
{
    public float moveSpeed = 2.0f;
    public float rotationSpeed = 45.0f;
    private float horizontalInput;
    private float verticalInput;
    public Rigidbody rb;
    public GameObject cameraRig;
    private Vector3 moveDirection;
    public Transform head;
    public CapsuleCollider capsule;
    public int totalHP = 3;
    public int currentHP;
    public TextMeshPro healthText;
    public GameObject respawnWaitZone;
    private bool canBeHurt = true;
    private float hurtResetTime = 0;
    private float respawnTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = totalHP;
        rb.freezeRotation = true;
        healthText.SetText(currentHP + "/" + totalHP);
    }

    // Update is called once per frame
    void Update()
    {
        // Joystick Rotation
        float rotation = rotationSpeed;
        rotation *= OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).x * Time.deltaTime;
        cameraRig.transform.Rotate(Vector3.up, rotation, Space.World);

        MovementInput();

        capsule.height = head.position.y + 0.05f;
        capsule.center = new Vector3(0, head.position.y / 2, 0);

        
    }


    // Joystick Movement
    private void FixedUpdate()
    {
        Timers();
        MovePlayer();
    }

    private void MovementInput()
    {
        horizontalInput = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).x;
        verticalInput = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).y;
    }

    private void MovePlayer()
    {
        moveDirection = cameraRig.transform.forward * verticalInput + cameraRig.transform.right * horizontalInput;

        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    }


    // Laser Obstacle Collision
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<LaserGrid>() != null)
        {
            if (other.gameObject.GetComponent<LaserGrid>().active)
            {
                Vector3 pushback = capsule.bounds.center - other.ClosestPoint(capsule.bounds.center);
                rb.AddForce(new Vector3(pushback.x, 0, pushback.z) * 2500f, ForceMode.Force);
                if (canBeHurt)
                {
                    TakeDamage();
                }
            }
        }
    }

    private void TakeDamage()
    {
        canBeHurt = false;
        hurtResetTime = 1.5f;
        currentHP--;
        healthText.SetText(currentHP + "/" + totalHP);
        if (currentHP <= 0)
        {
            transform.position = respawnWaitZone.transform.position;
            transform.rotation = Quaternion.Euler(Vector3.zero);
            cameraRig.transform.rotation = Quaternion.Euler(Vector3.zero);
            respawnTime = 5;
        }
    }

    private void Timers()
    {
        if (respawnTime > 0)
        {
            respawnTime -= Time.deltaTime;
            if (respawnTime < 0)
            {
                transform.position = Vector3.zero;
                transform.rotation = Quaternion.Euler(Vector3.zero);
                cameraRig.transform.rotation = Quaternion.Euler(Vector3.zero);
                currentHP = totalHP;
                healthText.SetText(currentHP + "/" + totalHP);
            }
        }
        if (hurtResetTime > 0)
        {
            hurtResetTime -= Time.deltaTime;
            if (hurtResetTime < 0)
            {
                canBeHurt = true;
            }
        }
    }
}