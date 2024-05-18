using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishManager : MonoBehaviour
{
    public int totalLap;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided with car"+other.tag);
        if (other.CompareTag("Player"))
        {
            CarGameManager carLapManager = other.GetComponent<CarGameManager>();
            if (carLapManager != null)
            {
                if (carLapManager.lapsCompleted<=totalLap)
                {
                    carLapManager.CompleteLap();
                    Debug.Log(carLapManager.lapsCompleted);
                }
                else if(carLapManager.lapsCompleted==totalLap)
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
        print(gameObject.name);
    }
}
