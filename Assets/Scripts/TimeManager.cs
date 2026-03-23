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
    public GameObject levelFailPanel;

    private bool isGameOver = false;

    [Header("Fail Audio")]
    public AudioClip failClip;
    public AudioSource uiAudioSource;

    public GameObject levelGameObj;

    private float bonusTimeCollected = 0f;
    private float flightTime = 0f;

    public static TimeManager instance;

    private bool timerRunning = false; // 🔥 IMPORTANT

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        currentTime = startTime;

        if (uiAudioSource == null)
            uiAudioSource = GetComponent<AudioSource>();

        timerRunning = false; // ❗ Timer paused at start (cutscene phase)

        UpdateUI();
    }

    void Update()
    {
        // ❗ Stop everything if game over OR timer not started
        if (isGameOver || !timerRunning)
            return;

        currentTime -= Time.deltaTime;

        flightTime += Time.deltaTime;

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

    // 🔥 CALL THIS FROM CUTSCENE
    public void StartTimer()
    {
        timerRunning = true;
    }

    public void GameOverFromCrash()
    {
        if (isGameOver) return;

        isGameOver = true;
        StartCoroutine(GameOverDelayRoutine());
    }

    IEnumerator GameOverDelayRoutine()
    {
        if (uiAudioSource != null && failClip != null)
        {
            uiAudioSource.PlayOneShot(failClip);
        }

        yield return new WaitForSeconds(3f);

        if (levelGameObj != null)
            levelGameObj.SetActive(false);

        GameManager.Instance.ShowLevelFail();
    }

    public float GetBonusTimeCollected()
    {
        return bonusTimeCollected;
    }

    public float GetFlightTime()
    {
        return flightTime;
    }

    void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;

        GameManager.Instance.ShowLevelFail();
    }

    public void AddTime(float seconds)
    {
        currentTime += seconds;
    }
}