using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacingGameManager : MonoBehaviour
{
    public int totalCheckpoints = 5;
    public Transform[] checkpoints;
    public int currentCheckpointIndex = 0;
    public int lapsCompleted = 0;

    public void PassedCheckpoint(Transform checkpoint)
    {
        if (checkpoint == checkpoints[currentCheckpointIndex])
        {
            currentCheckpointIndex++;

            if (currentCheckpointIndex >= totalCheckpoints)
            {
                currentCheckpointIndex = 0;
                lapsCompleted++;
            }
        }
        else
        {
            Debug.LogWarning("Incorrect checkpoint passed.");
        }
    }
}
