using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;

    private Image healthFillImage;

    void Start()
    {
        currentHealth = maxHealth;

        // 🔍 Find UI by tag
        GameObject fillObj = GameObject.FindGameObjectWithTag("Fill_Health");

        if (fillObj != null)
        {
            healthFillImage = fillObj.GetComponent<Image>();
        }
        else
        {
            Debug.LogError("Fill_Health tag not found!");
        }

        UpdateHealthUI();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Fence"))
        {
            TakeDamage(2f);
            Debug.Log("Collided with Fence - Damage Applied");
        }
    }

    // 💥 Take Damage
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // ❤️ Heal (optional)
    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthUI();
    }

    // 🔄 Update UI
    void UpdateHealthUI()
    {
        if (healthFillImage != null)
        {
            healthFillImage.fillAmount = currentHealth / maxHealth;
        }
    }
    IEnumerator DelayFail()
    {
        yield return new WaitForSeconds(1f);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.ShowLevelFail();
        }
    }
    // ☠️ Player death
    void Die()
    {
        Debug.Log("Player Died");

        // 🛑 Stop player movement immediately
        RCC_CarControllerV3 car = GetComponent<RCC_CarControllerV3>();
        if (car != null)
        {
            car.canControl = false;
        }

        // ⏳ Delay fail screen
        StartCoroutine(DelayFail());
    }
}