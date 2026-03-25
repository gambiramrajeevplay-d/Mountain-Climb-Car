using UnityEngine;

public class FinishLine : MonoBehaviour
{
    private RCC_Camera rccCamera;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") && other.GetComponent<RCC_CarControllerV3>() == null)
            return;

        RCC_CarControllerV3 car = other.GetComponent<RCC_CarControllerV3>();
        if (car == null)
            car = other.GetComponentInParent<RCC_CarControllerV3>();

        if (car != null)
        {
            car.canControl = false;

            // 🔇 Stop all car sounds
            AudioSource[] allSounds = car.GetComponentsInChildren<AudioSource>();
            foreach (AudioSource a in allSounds)
            {
                a.Stop();
                a.enabled = false;
            }
        }

        // 🎬 Optional cinematic camera
        rccCamera = FindObjectOfType<RCC_Camera>();
        if (rccCamera != null)
        {
            rccCamera.useAutoChangeCamera = false;
            rccCamera.ChangeCamera(RCC_Camera.CameraMode.CINEMATIC);
        }

        // ✅ CALL GAME MANAGER
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ShowLevelPass();
        }
    }
}