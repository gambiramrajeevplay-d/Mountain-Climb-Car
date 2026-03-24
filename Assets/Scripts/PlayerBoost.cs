using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoost : MonoBehaviour
{
    public RCC_CarControllerV3 carController;

    [Header("Boost Settings")]
    public float boostMultiplier = 2f;
    public float boostDuration = 5f;

    [Header("Visuals")]
    public ParticleSystem carBoostEffect1;
    public ParticleSystem carBoostEffect2;
    public ParticleSystem carBoostEffect3;

    [Header("Audio")]
    public AudioClip boostCollectedSound;





    private bool isBoosting = false;
    private float boostTimer = 0f;
    private float currentMotorForce;

    void Start()
    {



        if (carBoostEffect1 != null) carBoostEffect1.Stop();
        if (carBoostEffect2 != null) carBoostEffect2.Stop();
        if (carBoostEffect3 != null) carBoostEffect3.Stop();
    }

    void Update()
    {
        if (isBoosting)
        {
            // Keep applying boost while timer is active
            carController.boostInput = 1f;

            boostTimer -= Time.deltaTime;
            if (boostTimer <= 0f)
            {
                EndBoost();

            }
        }
        else
        {
            // No boost
            carController.boostInput = 0f;
        }



    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Booster"))
        {
            ParticleSystem padEffect = other.GetComponentInChildren<ParticleSystem>();
            if (padEffect != null)
            {
                padEffect.Play();

                AudioSource psAudio = padEffect.GetComponent<AudioSource>();
                if (psAudio != null)
                    psAudio.Play();
            }

            // Play sound at boost position
            if (boostCollectedSound != null)
                AudioSource.PlayClipAtPoint(boostCollectedSound, other.transform.position);

            StartBoost();

            Destroy(other.gameObject);
        }
    }

    void StartBoost()
    {
        isBoosting = true;
        boostTimer = boostDuration;
        carController.useNOS = true;
        carController.boostInput = 1f;
        // currentMotorForce = baseMotorForce * boostMultiplier;

        if (carBoostEffect1 != null) carBoostEffect1.Play();
        if (carBoostEffect2 != null) carBoostEffect2.Play();
        if (carBoostEffect3 != null) carBoostEffect3.Play();





        //  InGameAudioHandler.instance?.PlayBoostSound();
    }

    void EndBoost()
    {

        carController.useNOS = false;
       // carController.NOSSound.Stop();
        isBoosting = false;

        if (carBoostEffect1 != null) carBoostEffect1.Stop();
        if (carBoostEffect2 != null) carBoostEffect2.Stop();
        if (carBoostEffect3 != null) carBoostEffect3.Stop();

        Debug.Log("Car boost ended.");
    }
}
