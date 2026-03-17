using UnityEngine;

public class FailTrigger : MonoBehaviour
{
    [Header("UI")]
    public GameObject failPanel;

    public GameObject levelRoot;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") && other.GetComponent<RCC_CarControllerV3>() == null)
            return;

        // 🚗 Get car safely
        RCC_CarControllerV3 car = other.GetComponent<RCC_CarControllerV3>();
        if (car == null)
            car = other.GetComponentInParent<RCC_CarControllerV3>();

        if (car != null)
        {
            // ❌ Disable control
            car.canControl = false;

            // 🔇 Stop all RCC sounds
            AudioSource[] allSounds = car.GetComponentsInChildren<AudioSource>();
            foreach (AudioSource a in allSounds)
            {
                a.Stop();
                a.enabled = false;
            }
        }

        // 📱 Show fail panel
        if (failPanel != null)
            failPanel.SetActive(true);

        // ❌ Disable level
        if (levelRoot != null)
            levelRoot.SetActive(false);
    }
}