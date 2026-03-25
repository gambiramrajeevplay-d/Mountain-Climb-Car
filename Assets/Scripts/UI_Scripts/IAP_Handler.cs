using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IAP_Handler : MonoBehaviour
{

    public static IAP_Handler instance;

    public bool hasSubscription;
    public GameObject purchaseFailedUI;

    private Button defaultButtonToSelect;

    private CurrecnyManager currencyManager;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("IAP Handler Already Exsists!");
        }
        if (PlayerPrefs.GetInt(StringsData.hasSubscription) == 1)
        {
            hasSubscription = true;
        }
        else
        {
            hasSubscription = false;
        }


    }

    private void Start()
    {
        currencyManager = CurrecnyManager.instance;
    }

    public void Buy_coinpacks(int val)
    {
        currencyManager?.AddCurrency(val);

        if (MainMenu.instance != null)
        {
            MainMenu.instance.UpdateCurrencyText();
        }
    }
    public void failedTest(Button _buttonToSelect)
    {
        if (_buttonToSelect != null)
        {
            defaultButtonToSelect = _buttonToSelect;
        }
        purchaseFailedUI.SetActive(true);
    }
    public void subscribedAny()
    {
        //full game access
        //unlock all vehicles
        //
        for (int i = 1; i < 9; i++)
        {
            if (PlayerPrefs.GetInt("car" + i, 0) == 0)
            {
                PlayerPrefs.SetInt("car" + i, 1);
                //unlocked++;
            }
        }
        hasSubscription = true;
        //  PlayerPrefs.SetInt(StringsData.hasSubscription, 1);
        UnlockFullGame();




    }

    public void UnlockFullGame()
    {

        //UnlockAllCharacters();

        UnlockAllLevels();
    }

    public void UnlockAllCharacters()
    {
        for (int i = 1; i < 9; i++)
        {
            if (PlayerPrefs.GetInt("car" + i, 0) == 0)
            {
                PlayerPrefs.SetInt("car" + i, 1);
                //unlocked++;
            }
        }
    }

    public void Buy4Characters()
    {
        if (currencyManager != null)
        {
            for (int i = 4; i <= 7; i++)
            {
                currencyManager.UnlockCharacter(i);
            }
        }
    }

    public void BuyCharacter(int _characterIndex)
    {
        currencyManager.UnlockCharacter(_characterIndex);
    }

    public void UnlockFourCarsDummy()
    {
        //  int unlocked = 0;
        for (int i = 1; i < 5; i++)
        {
            if (PlayerPrefs.GetInt("car" + i, 0) == 0)
            {
                PlayerPrefs.SetInt("car" + i, 1);
                //unlocked++;
            }
        }
        PlayerPrefs.Save();
    }
    public void UnlockAllLevels()
    {
        PlayerPrefs.SetInt("FullGame", 1);// modes unlock
        PlayerPrefs.SetInt(StringsData.unlockedAllLevels, 1);// enemy unlock


    }

    public void RemoveAds()
    {


    }
}


