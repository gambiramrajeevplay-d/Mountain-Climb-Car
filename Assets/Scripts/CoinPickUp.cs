using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickUp : MonoBehaviour
{
    [SerializeField] private int coinValue = 1;

    // 🔥 Multiple particles
    [SerializeField] private List<ParticleSystem> pickupEffects;

    // 🔊 Audio
    [SerializeField] private AudioClip pickupClip;

    private AudioSource pickupSound;

    private void Awake()
    {
        GameObject audioObj = GameObject.FindGameObjectWithTag("PickUp");

        if (audioObj != null)
        {
            pickupSound = audioObj.GetComponent<AudioSource>();
        }
        else
        {
            Debug.LogWarning("[CoinPickUp] No GameObject found with tag 'PickUp'");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // 🪙 Add coins
        if (LevelCoinManager.instance != null)
            LevelCoinManager.instance.AddCoin(coinValue);

        // 🔊 Play pickup audio clip
        if (pickupSound != null && pickupClip != null)
            pickupSound.PlayOneShot(pickupClip);

        // ✨ Play ALL particles
        foreach (ParticleSystem effect in pickupEffects)
        {
            if (effect != null)
            {
                effect.transform.SetParent(null);
                effect.Play();
                Destroy(effect.gameObject, 2f);
            }
        }

        Destroy(gameObject);
    }
}