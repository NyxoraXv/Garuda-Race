using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class LeaderboardSystem : MonoBehaviour
{
    public CarGameManager playerCarGameManager;
    public string enemyCarTag;
    public TextMeshProUGUI[] leaderboardTexts;
    public Image[] leaderboardImages;

    private CarGameManager[] carGameManagers;
    private int[] lapsCompleted;
    private int[] sortedIndices;

    void Start()
    {
        UpdateCarGameManagers();
        lapsCompleted = new int[carGameManagers.Length];
        sortedIndices = new int[carGameManagers.Length];
    }

    void Update()
    {
        UpdateLapsCompleted();
        SortLeaderboard();
        UpdateLeaderboardUI();
    }

    void UpdateCarGameManagers()
    {
        GameObject[] enemyCars = GameObject.FindGameObjectsWithTag(enemyCarTag);
        carGameManagers = new CarGameManager[enemyCars.Length + 1];
        carGameManagers[0] = playerCarGameManager;

        for (int i = 0; i < enemyCars.Length; i++)
        {
            carGameManagers[i + 1] = enemyCars[i].GetComponent<CarGameManager>();
        }
    }

    void UpdateLapsCompleted()
    {
        for (int i = 0; i < carGameManagers.Length; i++)
        {
            if (carGameManagers[i] != null)
            {
                lapsCompleted[i] = carGameManagers[i].lapsCompleted;
            }
            else
            {
                Debug.LogWarning("CarGameManager at index " + i + " is null.");
            }
        }
    }

    // Corrected sorting logic
    void SortLeaderboard()
    {
        for (int i = 0; i < carGameManagers.Length; i++)
        {
            sortedIndices[i] = i;
        }
        System.Array.Sort(sortedIndices, (x, y) => lapsCompleted[y].CompareTo(lapsCompleted[x]));
    }

    void UpdateLeaderboardUI()
    {
        int leaderboardCount = Mathf.Min(leaderboardTexts.Length, carGameManagers.Length);

        for (int i = 0; i < leaderboardCount; i++)
        {
            int index = sortedIndices[i];
            TextMeshProUGUI text = leaderboardTexts[i];
            Image parentImage = leaderboardImages[i];

            string carName = (index == 0) ? "Player" : "Car " + (i + 1);
            text.text = carName + ": " + lapsCompleted[index] + " laps";

            parentImage.DOFade(1f, 0.5f).SetEase(Ease.OutQuad).OnComplete(() =>
                parentImage.DOFade(0f, 0.5f).SetDelay(3f).SetEase(Ease.InQuad));
        }

        for (int i = leaderboardCount; i < leaderboardTexts.Length; i++)
        {
            leaderboardTexts[i].text = "";
        }
    }

    public int[] GetSortedIndices()
    {
        return sortedIndices;
    }

    // Method to get the laps completed
    public int[] GetLapsCompleted()
    {
        return lapsCompleted;
    }

    //New method to get the winner index
    public int GetWinnerIndex()
    {
        if (sortedIndices.Length > 0)
        {
            return sortedIndices[0]; // The first index is the winner
        }
        else
        {
            Debug.LogWarning("No participants in the leaderboard.");
            return -1; // Or some other indicator of no winner
        }
    }
}
