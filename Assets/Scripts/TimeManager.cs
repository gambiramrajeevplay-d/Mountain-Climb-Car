using UnityEngine;
using TMPro;
using System.Collections;

public class TimeManager : MonoBehaviour
{
    [Header("Time Settings")]
    public float startTime = 60f;
    private float currentTime;

    [Header("UI")]
    public TextMeshProUGUI timeText;

    private bool isGameOver = false;
    public bool timerRunning = false;

    public static TimeManager instance;

    void Awake()
    {
        instance = this;

        // 🔍 Auto find if not assigned
        if (timeText == null)
        {
            GameObject timeObj = GameObject.FindGameObjectWithTag("Time");
            if (timeObj != null)
            {
                timeText = timeObj.GetComponent<TextMeshProUGUI>();
            }
            else
            {
                Debug.LogError("Time Text with tag 'Time' NOT FOUND!");
            }
        }
    }

    void Start()
    {
        currentTime = startTime;
        timerRunning = false;

        UpdateUI(); // show initial time
    }

    void Update()
    {
        if (isGameOver || !timerRunning)
            return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            GameOver();
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        if (timeText != null)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60f);
            int seconds = Mathf.FloorToInt(currentTime % 60f);
            int milliseconds = Mathf.FloorToInt((currentTime * 100f) % 100f);

            timeText.text = $"{minutes:00}:{seconds:00}:{milliseconds:00}";
        }
    }

    // 🔥 Call this after cutscene
    public void StartTimer()
    {
        timerRunning = true;
    }

    // 💥 Crash fail
    public void GameOverFromCrash()
    {
        if (isGameOver) return;

        isGameOver = true;
        StartCoroutine(GameOverDelayRoutine());
    }

    IEnumerator GameOverDelayRoutine()
    {
        yield return new WaitForSeconds(3f);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.ShowLevelFail();
        }
    }

    void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.ShowLevelFail();
        }
    }

    public void AddTime(float seconds)
    {
        currentTime += seconds;
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }
}