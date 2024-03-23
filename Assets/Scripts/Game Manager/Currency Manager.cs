using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyManager : MonoBehaviour
{
    private int Gold { get; set; }
    private int Ruby { get; set; }

    private const string GoldKey = "Gold";
    private const string RubyKey = "Ruby";

    private void Start()
    {
        Load();
    }

    public void AddGold(int goldToAdd)
    {
        Gold += goldToAdd;
        Save();
    }

    public bool SpendGold(int goldToSpend)
    {
        if (Gold >= goldToSpend)
        {
            Gold -= goldToSpend;
            Save();
            return true;
        }
        else
        {
            Debug.LogWarning("Not enough gold to spend.");
            return false;
        }
    }

    public int AddRuby(int rubyToAdd)
    {
        Ruby += rubyToAdd;
        Save();
        return Ruby;
    }

    public bool SpendRuby(int rubyToSpend)
    {
        if (Ruby >= rubyToSpend)
        {
            Ruby -= rubyToSpend;
            Save();
            return true;
        }
        else
        {
            Debug.LogWarning("Not enough rubies to spend.");
            return false;
        }
    }

    public int GetGold()
    {
        return Gold;
    }

    public int GetRuby()
    {
        return Ruby;
    }

    private void Save()
    {
        PlayerPrefs.SetInt(GoldKey, Gold);
        PlayerPrefs.SetInt(RubyKey, Ruby);
        PlayerPrefs.Save();
    }

    private void Load()
    {
        Gold = PlayerPrefs.GetInt(GoldKey, 0);
        Ruby = PlayerPrefs.GetInt(RubyKey, 0);
    }
}

