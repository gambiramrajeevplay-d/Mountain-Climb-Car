using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_CutScene : MonoBehaviour
{
    [Header("Cutscene Camera")]
    public Camera cutsceneCamera;
    public List<Transform> points;
    public float speed = 5f;

    private int currentPointIndex = 0;
    private bool isPlaying = false;

    private RCC_CarControllerV3 playerCar;
    private RCC_Camera rccCamera;

    void Start()
    {
        // 🔍 Find player
        playerCar = FindObjectOfType<RCC_CarControllerV3>();

        // 🔍 Find RCC Camera
        rccCamera = FindObjectOfType<RCC_Camera>();

        // 🛑 Disable player control
        if (playerCar != null)
            playerCar.canControl = false;

        // 🎥 Disable gameplay camera
        if (rccCamera != null)
            rccCamera.gameObject.SetActive(false);

        // 🎬 Enable cutscene camera
        if (cutsceneCamera != null)
            cutsceneCamera.gameObject.SetActive(true);

        // ⏱ OPTIONAL: Hide timer during cutscene
        

        // ▶️ Start cutscene
        PlayCutscene();
    }

    public void PlayCutscene()
    {
        if (isPlaying) return;

        isPlaying = true;
        StartCoroutine(MoveCamera());
    }

    IEnumerator MoveCamera()
    {
        while (currentPointIndex < points.Count)
        {
            Transform target = points[currentPointIndex];

            while (Vector3.Distance(cutsceneCamera.transform.position, target.position) > 0.05f)
            {
                // Move
                cutsceneCamera.transform.position = Vector3.MoveTowards(
                    cutsceneCamera.transform.position,
                    target.position,
                    speed * Time.deltaTime
                );

                // Rotate
                cutsceneCamera.transform.rotation = Quaternion.Lerp(
                    cutsceneCamera.transform.rotation,
                    target.rotation,
                    Time.deltaTime * speed
                );

                yield return null;
            }

            currentPointIndex++;
        }

        EndCutscene();
    }

    void EndCutscene()
    {
        Debug.Log("Cutscene Finished");

        // 🎥 Disable cutscene camera
        if (cutsceneCamera != null)
            cutsceneCamera.gameObject.SetActive(false);

        // 🎮 Enable gameplay camera
        if (rccCamera != null)
            rccCamera.gameObject.SetActive(true);

        // 🚗 Enable player control
        if (playerCar != null)
            playerCar.canControl = true;

        // ⏱ SHOW TIMER
        
        // ⏱ START TIMER (IMPORTANT 🔥)
        if (TimeManager.instance != null)
            TimeManager.instance.StartTimer();

        // Reset
        currentPointIndex = 0;
        isPlaying = false;
    }
}