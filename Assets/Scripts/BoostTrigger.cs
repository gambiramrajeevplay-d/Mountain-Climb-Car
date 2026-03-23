using UnityEngine;

public class BoostTrigger : MonoBehaviour
{
    private RCC_CarControllerV3 carController;

    [Header("Boost Settings")]
    public float boostDuration = 5f;

    [Header("Visuals")]
    public ParticleSystem carBoostEffect1;
    public ParticleSystem carBoostEffect2;
    public ParticleSystem carBoostEffect3;

    [Header("Audio")]
    public AudioClip boostCollectedSound;

    private bool isBoosting = false;
    private float boostTimer = 0f;

    private float originalMaxSpeed;
    private float originalEngineTorque;

    [Header("Jump Boost")]
    public float jumpForce = 8000f;
    private Rigidbody rb;
    void Start()
    {
        carController = GetComponent<RCC_CarControllerV3>();
        rb = GetComponent<Rigidbody>();
        if (carBoostEffect1) carBoostEffect1.Stop();
        if (carBoostEffect2) carBoostEffect2.Stop();
        if (carBoostEffect3) carBoostEffect3.Stop();
    }

    void Update()
    {
        if (isBoosting)
        {
            carController.useNOS = true;
            carController.boostInput = 1f;
            carController.gasInput = 1f;

            boostTimer -= Time.deltaTime;

            if (boostTimer <= 0f)
                EndBoost();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // ✅ Now we check BOOST PAD
        if (other.CompareTag("Boost"))
        {
            Debug.Log("BOOST PICKED");

            // Play pad effect
            ParticleSystem padEffect = other.GetComponentInChildren<ParticleSystem>();
            if (padEffect) padEffect.Play();

            // Sound
            if (boostCollectedSound)
                AudioSource.PlayClipAtPoint(boostCollectedSound, transform.position);

            StartBoost();

            Destroy(other.gameObject); // destroy pad
        }
    }

    void StartBoost()
    {
        isBoosting = true;
        boostTimer = boostDuration;

        // Save original values
        originalMaxSpeed = carController.maxspeed;
        originalEngineTorque = carController.engineTorque;

        // 🚀 BOOST POWER
        carController.maxspeed *= 1.5f;
        carController.engineTorque *= 2f;

        carController.useNOS = true;

        // 🔥 JUMP FORCE (THIS MAKES IT FLY)
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        if (carBoostEffect1) carBoostEffect1.Play();
        if (carBoostEffect2) carBoostEffect2.Play();
        if (carBoostEffect3) carBoostEffect3.Play();
    }
    void EndBoost()
    {
        carController.boostInput = 0f;

        // 🔄 Reset values
        carController.maxspeed = originalMaxSpeed;
        carController.engineTorque = originalEngineTorque;

        isBoosting = false;

        if (carBoostEffect1) carBoostEffect1.Stop();
        if (carBoostEffect2) carBoostEffect2.Stop();
        if (carBoostEffect3) carBoostEffect3.Stop();

        Debug.Log("BOOST ENDED");
    }
}