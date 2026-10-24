using System;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float turnSpeed = 25f;

    public float accelerateSpeed;
    public float maxSpeed;
    private float currSpeed = 0f;

    public float reverseSpeed;
    public float drag;
    public float rayLength;
    private Vector3 velocity;
    private Vector3 previousPosition;
    private bool collision;
    private Vector3 movement;
    private float horizontal;
    private float rotationAmount;

    public Rigidbody rb;

    [Header("FX")]
    public ParticleSystem exhaustParticles;

    [Header("UI")]
    public TextMeshProUGUI text;

    [Header("Camera")]
    public CinemachineCamera cinemachineCamera;
    public float minZoom;
    public float maxZoom;
    public float zoomSmoothness = 3f;

    [Header("Components")] 
    public AudioSource source;

    void Start()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();

        previousPosition = rb.position;
        
        rb.centerOfMass = new Vector3(0f, -0.5f, 0f);
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        float speedPercent = maxSpeed > 0 ? currSpeed / maxSpeed : 0f;
        float targetZoom = Mathf.Lerp(minZoom, maxZoom, speedPercent);

        cinemachineCamera.Lens.FieldOfView = Mathf.Lerp(
            cinemachineCamera.Lens.FieldOfView,
            targetZoom,
            zoomSmoothness * Time.deltaTime
        );

        if (velocity != Vector3.zero)
        {
            
        }

        Ray ray = new Ray(rb.position, rb.transform.forward);
        collision = Physics.Raycast(ray, rayLength + currSpeed * Time.deltaTime); 
        
        rotationAmount = horizontal * turnSpeed * Time.fixedDeltaTime;

        float verticalFactor = Mathf.Abs(transform.forward.z);

        float cameraCompensation = Mathf.Lerp(
            1f,
            1f / Mathf.Cos(25f * Mathf.Deg2Rad),
            verticalFactor
        );

        if (!collision)
        {
            
        }
        
        if (vertical > 0)
        {
            if (currSpeed < 0)
                currSpeed += reverseSpeed * Time.deltaTime;
            else
                currSpeed += accelerateSpeed * Time.deltaTime;
        }
        else if (vertical < 0)
        {
            if (currSpeed > 0)
                currSpeed -= reverseSpeed * Time.deltaTime;
            else
                currSpeed -= accelerateSpeed * Time.deltaTime;
        }
        else
        {
            currSpeed = Mathf.MoveTowards(currSpeed, 0f, drag * Time.deltaTime);
        }

        currSpeed = Mathf.Clamp(currSpeed, -maxSpeed, maxSpeed);

        movement = transform.forward * (currSpeed * cameraCompensation);

        var emission = exhaustParticles.emission;
        emission.rateOverTime = currSpeed * 3f;

        int speedForText = Mathf.RoundToInt(currSpeed);
        text.text = speedForText.ToString();

        source.volume = currSpeed / 50;
    }

    private void FixedUpdate()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, rotationAmount, 0f));
        
        rb.linearVelocity = new Vector3(
            movement.x,
            rb.linearVelocity.y,
            movement.z
        );

        previousPosition = rb.position;
        
        print("working");
    }
}