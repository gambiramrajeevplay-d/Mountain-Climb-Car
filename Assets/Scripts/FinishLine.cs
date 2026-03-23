using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class FinishLine : MonoBehaviour
{
    [Header("UI")]
    public GameObject finishPanel;

    public GameObject levelRoot;

    [Header("Audio")]
    public AudioClip finishClip; // 🎵 assign in Inspector
    private AudioSource pickupSource;

    private RCC_Camera rccCamera;

    private void Awake()
    {
        // 🔍 Find AudioSource with tag "PickUp"
        GameObject audioObj = GameObject.FindGameObjectWithTag("PickUp");

        if (audioObj != null)
        {
            pickupSource = audioObj.GetComponent<AudioSource>();
        }
        else
        {
            Debug.LogWarning("[FinishLine] No GameObject found with tag 'PickUp'");
        }
    }

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

            // Stop all sounds
            AudioSource[] allSounds = car.GetComponentsInChildren<AudioSource>();
            foreach (AudioSource a in allSounds)
            {
                a.Stop();
                a.enabled = false;
            }
        }

        // Play sound
        if (pickupSource != null && finishClip != null)
        {
            pickupSource.PlayOneShot(finishClip);
        }

        // Start sequence
        StartCoroutine(FinishSequence(0.5f));
    }

    private void Update()
    {
        //if (rccCamera != null && finishPanel != null && finishPanel.activeSelf)
        //{
        //    if (rccCamera.cameraMode != RCC_Camera.CameraMode.CINEMATIC)
        //    {
        //        rccCamera.ChangeCamera(RCC_Camera.CameraMode.CINEMATIC);
        //    }
        //}
    }

    IEnumerator FinishSequence(float delay)
    {
        yield return new WaitForSeconds(delay);

        rccCamera = FindObjectOfType<RCC_Camera>();

        if (rccCamera != null)
        {
            rccCamera.useAutoChangeCamera = false;
            rccCamera.ChangeCamera(RCC_Camera.CameraMode.CINEMATIC);
        }

        yield return new WaitForSeconds(1f); // let camera settle

        if (finishPanel != null)
            finishPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void ReloadScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
        //SceneManager.GetActiveScene().buildIndex
    }
}