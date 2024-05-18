using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarGameManager : MonoBehaviour
{
    public int lapsCompleted = 0;

    public void CompleteLap()
    {
        lapsCompleted++;
    }
}
