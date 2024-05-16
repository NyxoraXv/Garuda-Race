using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class CarsItemUI : MonoBehaviour
{
    [SerializeField] TMP_Text carsNameText;
    [SerializeField] Image carsRarityImage;
    [SerializeField] Image carsTumbnail;
    [SerializeField] TMP_Text carsPriceText;
    [SerializeField] Button carsPurchaseButton;

    public void SetCarsImage (Sprite tumbnail)
    {
        carsTumbnail.sprite = tumbnail;
    }
    public void SetCarsname( string name)
    {
        carsNameText.text = name;
    }
    public void SetCarsRarity(Sprite rarity)
    {
        carsRarityImage.sprite = rarity;
    }
    public void SetCarsPrice(int price)
    {
       carsPriceText.text = price.ToString();
    }
    public void SetCarsAsPurchased()
    {
        carsPurchaseButton.gameObject.SetActive(false);
    }

}
