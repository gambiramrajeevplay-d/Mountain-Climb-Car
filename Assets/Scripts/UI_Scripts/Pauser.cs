
using Script;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static RCC_UIDashboardDisplay;
public class Pauser : MonoBehaviour
{
    public static Pauser instance;

    [Header("UI")]
    public GameObject PausePannel;
    public GameObject LevelObject;
    public GameObject PauseButton;
    public GameObject levelFailPanel;

    [Header("Sound")]
    public Image sound_;
    public Sprite sound_on, sound_off;

    // 🔒 GLOBAL PAUSE LOCK (Boss fight, cutscenes, etc.)
    public static bool PauseLocked = false;

    private void Awake()
    {
        instance = this;
        AudioManagerPause.Initialize();
    }

    private void OnEnable()
    {
        if (AndroidTV.IsAndroidOrFireTv())
            PauseButton.SetActive(false);
        else
            PauseButton.SetActive(true);
    }

    private void Start()
    {
        if (AndroidTV.IsAndroidOrFireTv())
            PauseButton.SetActive(false);
        else
            PauseButton.SetActive(true);
    }

    void Update()
    {
        // 🔒 HARD BLOCK pause when locked
        if (PauseLocked)
            return;

        // ❌ Already finished → no pause
        if (GameManager.Instance.levelPassPanel.activeSelf ||
    GameManager.Instance.levelFailPanel.activeSelf)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }

    }

    public void Pause()
    {
        if (PauseLocked)
            return;

        LevelObject.SetActive(false);   // 🔴 Disable level
        PausePannel.SetActive(true);

        if (!AndroidTV.IsAndroidOrFireTv())
            PauseButton.SetActive(false);

        Time.timeScale = 0f;
        UpdateSoundSprite();
    }

    public void Resume()
    {
        LevelObject.SetActive(true);    // 🟢 Enable level
        PausePannel.SetActive(false);

        if (!AndroidTV.IsAndroidOrFireTv())
            PauseButton.SetActive(true);

        Time.timeScale = 1f;
    }

    public void MM()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("UI");
    }

    public void Mute()
    {
        AudioManagerPause.IsMuted = !AudioManagerPause.IsMuted;
        UpdateSoundSprite();
    }

    private void UpdateSoundSprite()
    {
        if (sound_ != null)
            sound_.sprite = AudioManagerPause.IsMuted ? sound_off : sound_on;
    }
    // 🔥 CALLED FROM BOSS TRIGGER
    public static void LockPause()
    {
        PauseLocked = true;

        // force unpause
        if (instance != null)
        {
            instance.PausePannel.SetActive(false);
          
        }
    }

    public static void UnlockPause()
    {
        PauseLocked = false;
    }

    public void Sound_on()
    {
        AudioManagerPause.IsMuted = !AudioManagerPause.IsMuted;
        UpdateSoundSprite();
    }
    private void OnApplicationFocus(bool focus)
    {
        // Ignore during countdown
        //if (GameManager.CountdownRunning)
        //    return;

        if (!focus)
        {
            print("App lost focus");

            if (!PauseLocked &&
                !GameManager.Instance.levelPassPanel.activeSelf &&
                !GameManager.Instance.levelFailPanel.activeSelf)
            {
                Pause();
            }
        }
        else
        {
            print("App gained focus");
            AudioListener.volume = 1f;
        }
    }

}
