using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinPickUp : MonoBehaviour
{
    [SerializeField] private int coinValue = 1;

    [SerializeField] private List<ParticleSystem> pickupEffects;
    [SerializeField] private AudioClip pickupClip;

    private AudioSource pickupSound;

    private TextMeshProUGUI coinText;

    // ✅ SHARED across all coins
    private static int tempCoinCount = 0;
    private static Coroutine textCoroutine;

    private void Awake()
    {
        GameObject audioObj = GameObject.FindGameObjectWithTag("PickUp");
        if (audioObj != null)
        {
            pickupSound = audioObj.GetComponent<AudioSource>();
        }

        GameObject textObj = GameObject.FindGameObjectWithTag("CoinText");
        if (textObj != null)
        {
            coinText = textObj.GetComponent<TextMeshProUGUI>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (LevelCoinManager.instance != null)
            LevelCoinManager.instance.AddCoin(coinValue);

        if (coinText != null)
        {
            tempCoinCount += coinValue; // ✅ now accumulates globally
            coinText.text = tempCoinCount.ToString();

            if (textCoroutine != null)
                StopCoroutine(textCoroutine);

            textCoroutine = StartCoroutine(ShowCoinText());
        }

        if (pickupSound != null && pickupClip != null)
            pickupSound.PlayOneShot(pickupClip);

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

    IEnumerator ShowCoinText()
    {
        coinText.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);

        coinText.gameObject.SetActive(false);

        tempCoinCount = 0; // reset after delay
    }
}