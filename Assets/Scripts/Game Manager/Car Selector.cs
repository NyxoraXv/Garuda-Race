using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CarSelection : MonoBehaviour
{
    public GameObject[] carPrefabs;
    public Transform carSpawnPoint;
    public TextMeshProUGUI selectedCarText;

    private int selectedCarIndex = 0;

    private void Start()
    {
        UpdateSelectedCarText();
        SpawnSelectedCar();
    }

    private void UpdateSelectedCarText()
    {
        selectedCarText.text = "Selected Car: " + selectedCarIndex;
    }

    private void SpawnSelectedCar()
    {
        if (carPrefabs.Length > 0 && carSpawnPoint != null)
        {
            foreach (Transform child in carSpawnPoint)
            {
                Destroy(child.gameObject);
            }

            GameObject selectedCar = Instantiate(carPrefabs[selectedCarIndex], carSpawnPoint.position, Quaternion.identity);
            selectedCar.transform.parent = carSpawnPoint;

            // Set the rotation of the instantiated car to identity
            selectedCar.transform.rotation = Quaternion.identity;

            // Make the instantiated car 1.5 times bigger
            selectedCar.transform.localScale = Vector3.one * 2f;
        }
    }


    public void SelectNextCar()
    {
        selectedCarIndex = (selectedCarIndex + 1) % carPrefabs.Length;
        UpdateSelectedCarText();
        SpawnSelectedCar();
    }

    public void SelectPreviousCar()
    {
        selectedCarIndex = (selectedCarIndex - 1 + carPrefabs.Length) % carPrefabs.Length;
        UpdateSelectedCarText();
        SpawnSelectedCar();
    }

    public void ConfirmSelection()
    {
        PlayerPrefs.SetInt("SelectedCar", selectedCarIndex);
        PlayerPrefs.Save();

        Debug.Log("Car selection confirmed. Selected Car: " + selectedCarIndex);
        // Add code to transition to the next scene or perform other actions as needed
    }
}
