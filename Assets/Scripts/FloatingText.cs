using System.Collections;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public float moveUpSpeed = 1f;
    public float fadeDuration = 1f;

    private TextMeshProUGUI text;
    private Color startColor;
    private Vector3 startPosition;

    void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        startPosition = transform.position;
    }

    public void Setup(float amount)
    {
        transform.position = startPosition;

        startColor = Color.white; // ✅ Always white
        text.color = startColor;

        gameObject.SetActive(true);

        text.text = "+" + amount.ToString("0");

        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;

            // Move upward
            transform.position += Vector3.up * moveUpSpeed * Time.deltaTime;

            // Fade
            float alpha = Mathf.Lerp(1f, 0f, time / fadeDuration);
            text.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

            yield return null;
        }

        gameObject.SetActive(false);
    }
}