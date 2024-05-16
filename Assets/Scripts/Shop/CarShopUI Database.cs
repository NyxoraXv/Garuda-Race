using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Car Shop Database", menuName = "Shopping/Cars Shop Database")]
public class CarShopUIDatabase : ScriptableObject
{
    public Car[] cars;

    public int CarsCount{
        get { return cars.Length; }
    }
    public Car GetCars(int index)
    {
        return cars[index];
    }
    public void PurchaseCars(int index)
    {
        cars[index].isPurchased = true;
    }
}
