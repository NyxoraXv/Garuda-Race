using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CurrentUIState
{
    MainMenu,
    Garage,
    Shop,
    RaceOption
}

public class MainMenuNavigation : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject garage;
    public GameObject shop;
    public GameObject raceOption;

    private CurrentUIState state;

    private void SwitchUIPage(CurrentUIState newState)
    {
        DisableCurrentUIPage();

        switch (newState)
        {
            case CurrentUIState.MainMenu:
                mainMenu.SetActive(true);
                break;
            case CurrentUIState.Garage:
                garage.SetActive(true);
                break;
            case CurrentUIState.Shop:
                shop.SetActive(true);
                break;
            case CurrentUIState.RaceOption:
                raceOption.SetActive(true);
                break;
        }

        state = newState;
    }

    private void DisableCurrentUIPage()
    {
        switch (state)
        {
            case CurrentUIState.MainMenu:
                mainMenu.SetActive(false);
                break;
            case CurrentUIState.Garage:
                garage.SetActive(false);
                break;
            case CurrentUIState.Shop:
                shop.SetActive(false);
                break;
            case CurrentUIState.RaceOption:
                raceOption.SetActive(false);
                break;
        }
    }

    public void toMainMenu() { SwitchUIPage(CurrentUIState.MainMenu); }
    public void toGarage() { SwitchUIPage(CurrentUIState.Garage); }
    public void toShop() { SwitchUIPage(CurrentUIState.Shop); }
    public void toRaceOption() { SwitchUIPage(CurrentUIState.RaceOption); }
}
