using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Panels")]
    public GameObject levelPassPanel;
    public GameObject levelFailPanel;

    [Header("UI")]
    public TextMeshProUGUI passCoinsText;
    public TextMeshProUGUI failCoinsText;

    private bool levelEnded = false;

    [Header("Audio")]
    public AudioClip levelFailSound;
    public AudioClip levelPassSound;

    private AudioSource audioSource;

    [Header("Level Root (Disable on End)")]
    public GameObject levelRoot;

    [Header("Delays")]
    public float levelPassDelay = 1.5f; // 🟢 PASS DELAY
    public float levelFailDelay = 1.5f; // 🔴 FAIL DELAY

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        Pauser.UnlockPause();
        Time.timeScale = 1f;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
    }

    // =========================
    // LEVEL PASS
    // =========================
    public void ShowLevelPass()
    {
        if (levelEnded) return;
        levelEnded = true;

        StartCoroutine(LevelPassRoutine());
    }

    IEnumerator LevelPassRoutine()
    {
        yield return new WaitForSeconds(levelPassDelay);

        if (levelPassSound != null)
            audioSource.PlayOneShot(levelPassSound);

        int totalCoins = AddCollectedCoins();

        if (CurrencyManager.instance != null)
        {
            CurrencyManager.instance.AddCurrency(100);
            totalCoins += 100;
        }

        UpdatePassUI(totalCoins);

        if (levelPassPanel != null)
            levelPassPanel.SetActive(true);

        if (levelRoot != null)
            levelRoot.SetActive(false);

        Pauser.LockPause();
        Pauser.instance.Pause();
    }

    // =========================
    // LEVEL FAIL
    // =========================
    public void ShowLevelFail()
    {
        if (levelEnded) return;
        levelEnded = true;

        StartCoroutine(LevelFailRoutine());
    }

    IEnumerator LevelFailRoutine()
    {
        yield return new WaitForSeconds(levelFailDelay);

        if (levelFailSound != null)
            audioSource.PlayOneShot(levelFailSound);

        int totalCoins = AddCollectedCoins();

        UpdateFailUI(totalCoins);

        if (levelFailPanel != null)
            levelFailPanel.SetActive(true);

        if (levelRoot != null)
            levelRoot.SetActive(false);

        Pauser.LockPause();
        Pauser.instance.Pause();
    }

    // =========================
    // COINS
    // =========================
    private int AddCollectedCoins()
    {
        int totalCoins = 0;

        if (CurrencyManager.instance != null && LevelCoinManager.instance != null)
        {
            int collectedCoins = LevelCoinManager.instance.GetCollectedCoins();
            CurrencyManager.instance.AddCurrency(collectedCoins);
            totalCoins = CurrencyManager.instance.GetCurrency();
        }

        return totalCoins;
    }

    // =========================
    // UI
    // =========================
    private void UpdatePassUI(int totalCoins)
    {
        if (passCoinsText)
            passCoinsText.text = totalCoins.ToString();
    }

    private void UpdateFailUI(int totalCoins)
    {
        if (failCoinsText)
            failCoinsText.text = totalCoins.ToString();
    }

    // =========================
    // BUTTONS
    // =========================
    public void Home()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("UI");
    }
}