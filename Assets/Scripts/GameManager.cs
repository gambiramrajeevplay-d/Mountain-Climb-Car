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

    public GameObject levelObject;

    private CoinCollector coinCollector;

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
        coinCollector = FindObjectOfType<CoinCollector>();
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

       

        // 🔊 Play sound
        if (levelPassSound != null)
            audioSource.PlayOneShot(levelPassSound);

        int collectedCoins = 0;

        if (coinCollector != null)
            collectedCoins = coinCollector.GetLevelCoins();

        int totalReward = collectedCoins + 100;

        // 💰 Add collected + bonus
        CurrecnyManager.instance.AddCurrency(totalReward);

        // 🏆 UNLOCK NEXT LEVEL (BASED ON BUILD INDEX)
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int unlockedLevel = PlayerPrefs.GetInt(StringsData.playerLevel, 1);

        if (currentSceneIndex >= unlockedLevel)
        {
            PlayerPrefs.SetInt(StringsData.playerLevel, currentSceneIndex + 1);
            PlayerPrefs.Save();
        }

        // 🧾 Update UI
        UpdatePassUI(totalReward);

        // 🟢 Show panel
        if (levelPassPanel != null)
            levelPassPanel.SetActive(true);

        levelObject.SetActive(false);


        // ⏸ Lock pause
        Pauser.LockPause();
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

        int collectedCoins = 0;

        if (coinCollector != null)
            collectedCoins = coinCollector.GetLevelCoins();

        // 💰 Add only collected coins to total currency
        CurrecnyManager.instance.AddCurrency(collectedCoins);

        // 🧾 Show ONLY collected coins
        UpdateFailUI(collectedCoins);

        if (levelFailPanel != null)
            levelFailPanel.SetActive(true);

        levelObject.SetActive(false);

        Pauser.LockPause();
     //  Pauser.instance.Pause();
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

        // 🔥 IMPORTANT: Tell MainMenu we came from gameplay
        PlayerPrefs.SetInt("ShowSubscriptionPanel", 1);
        PlayerPrefs.Save();

        SceneManager.LoadScene("UI");
    }
}