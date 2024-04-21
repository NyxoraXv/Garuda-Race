using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public class PlayerData
    {
        /*public PlayerInformation playerInformation { get; set; } = new PlayerInformation();
        public CurrencyManager currency { get; set; } = new CurrencyManager();
        public Statistic statistic { get; set; } = new Statistic();
        public LevelInformation levelInformation { get; set; } = new LevelInformation();
        public CarSelector selectedCar { get; set; } = new CarSelector();
        public CarList ownedCar { get; set; } = new CarList();*/
    }
    public PlayerData playerData;
    private const string playerDataKey = "PlayerData";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadData(); 
        }
        else
        {
            Destroy(gameObject); 
        }

    }
    public void LoadData()
    {
        if (PlayerPrefs.HasKey(playerDataKey))
        {
            string jsonData = PlayerPrefs.GetString(playerDataKey);
            playerData = JsonUtility.FromJson<PlayerData>(jsonData);
        }
        else
        {
            playerData = new PlayerData();
        }
    }
    public void SaveData()
    {
        string jsonData = JsonUtility.ToJson(playerData);
        PlayerPrefs.SetString(playerDataKey, jsonData);
        PlayerPrefs.Save();
    }
    private void OnApplicationQuit()
    {
        SaveData();
    }
    public void SpendFRG(float frg_cost)
    {
       /* if (frg_cost <= playerData.currency.frg)
        {
            playerData.currency.frg -= frg_cost;
        }*/
    }
    public void SpendLUNC(float lunc_cost)
    {
        /*if (lunc_cost <= playerData.currency.lunc)
        {
            playerData.currency.lunc -= lunc_cost;
        }*/
    }


}
