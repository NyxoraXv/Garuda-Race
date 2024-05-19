using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishManager : MonoBehaviour
{
    public int totalLap;
    public FinishUIScript finishUI;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided with car"+other.tag);
        if (other.CompareTag("Player") || other.CompareTag("enemy"))
        {
            CarGameManager carLapManager = other.GetComponent<CarGameManager>();
            if (carLapManager != null)
            {
                if (carLapManager.lapsCompleted<=totalLap)
                {
                    carLapManager.CompleteLap();
                    Debug.Log(carLapManager.lapsCompleted);
                }
                else if(carLapManager.lapsCompleted>=totalLap)
                {
                    FinishGame(other.gameObject);
                }
            }
            else
            {
                Debug.Log("Car Manager is Null");
            }
        }
    }

    private void FinishGame(GameObject gameObject)
    {
        finishUI.ShowFinishUI();
    }
}
