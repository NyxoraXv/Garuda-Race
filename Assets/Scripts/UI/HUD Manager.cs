using Cars;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

public class HUD : MonoBehaviour
{
    private CarController carController;

    [Header("Digital Number")]
    public TMPro.TextMeshProUGUI Speedometer;
    public TMPro.TextMeshProUGUI Tachometer;
    public TMPro.TextMeshProUGUI Gear;

    [Header("Needle")]
    public RectTransform SpeedometerNeedle;
    public RectTransform rpmNeedle;

    private int MaxSpeedometerSpeed;
    private float shiftUpRPM;
    private int MaxGear;
    private int[] clamp = {90, -90};



    private void Start()
    {
        carController = GameObject.FindAnyObjectByType<CarController>();
        MaxSpeedometerSpeed = Convert.ToInt32(carController.carSetting.LimitForwardSpeed);
        shiftUpRPM = carController.carSetting.shiftUpRPM;
    }

    private float calculateNeedleRotation(int Val, int MaxVal)
    {
        float normalizedSpeedometerValue = (float)Val / MaxVal;
        float Value = clamp[0] - clamp[1];
        return (clamp[0] - normalizedSpeedometerValue * Value);
    }


    private void Update()
    {
        int currentSpeed = carController.getSpeed();
        int currentGear = carController.getGear();
        float currentRPM = carController.motorRPM;

        Speedometer.text = currentSpeed.ToString()+"KM/H";
        Gear.text = "Gear "+currentGear.ToString();
        Tachometer.text = math.floor(currentRPM).ToString();
        
        quaternion speedometerRotation = Quaternion.Euler(0, 0, calculateNeedleRotation(currentSpeed, MaxSpeedometerSpeed));
        SpeedometerNeedle.localRotation = speedometerRotation;

        quaternion rpmRotation = Quaternion.Euler(0, 0, calculateNeedleRotation(Convert.ToInt32(currentRPM), Convert.ToInt32(shiftUpRPM)));
        rpmNeedle.localRotation = rpmRotation;
    }


}
