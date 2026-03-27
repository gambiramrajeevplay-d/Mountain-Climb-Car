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
            // 🚗 Stop control
            car.canControl = false;
            TimeManager.instance.timerRunning = false;

            // 🔇 Stop all car sounds
            AudioSource[] allSounds = car.GetComponentsInChildren<AudioSource>();
            foreach (AudioSource a in allSounds)
            {
                a.Stop();
                a.enabled = false;
            }

            // 🎥 ❗ Disable ALL child cameras of the car
            Camera[] carCameras = car.GetComponentsInChildren<Camera>(true);
            foreach (Camera cam in carCameras)
            {
                cam.enabled = false;
                cam.gameObject.SetActive(false);
            }
        }

        // 🎬 Switch to Cinematic RCC Camera ONLY
        rccCamera = FindObjectOfType<RCC_Camera>();
        if (rccCamera != null)
        {
            rccCamera.useAutoChangeCamera = false;
            rccCamera.ChangeCamera(RCC_Camera.CameraMode.CINEMATIC);

            // 🔥 Force enable RCC camera if disabled
            if (!rccCamera.gameObject.activeSelf)
                rccCamera.gameObject.SetActive(true);
        }

        // ✅ Show Level Complete UI
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ShowLevelPass();
        }
    }
}