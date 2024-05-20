using Cars;
using System.Collections;
using UnityEngine;

public class WaitFor3Sec : MonoBehaviour
{
    private CarController controller;
    private float carPower;

    private void Start()  // Note: Use capital 'S' for Start()
    {
        CarController controller = GetComponent<CarController>();
        carPower = controller.carSetting.carPower;
        controller.carSetting.carPower = 0f;
        StartCoroutine(WaitFor3Seconds()); // Start the coroutine
    }

    IEnumerator WaitFor3Seconds()  // Renamed for clarity
    {
        yield return new WaitForSeconds(3f); // Wait for 3 seconds
        CarController controller = GetComponent<CarController>();
        controller.carSetting.carPower = carPower;
    }
}
