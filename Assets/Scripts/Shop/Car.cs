using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Car : MonoBehaviour
{
    public Sprite imageCar;
    public string nameCar;
    [Range(0, 100)] public float acceleration;
    [Range(0, 100)]public float speed;
    [Range (0, 100)] public float grip;
    [Range(0, 100)] public float engine;
    public Sprite rarity;
    public int price;
    public bool isPurchased;
}
