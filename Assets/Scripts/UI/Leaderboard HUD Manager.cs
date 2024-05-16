using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class LeaderboardSystem : MonoBehaviour
{
    public Transform playerCar;
    public string enemyCarTag; // Tag for enemy cars
    public TextMeshProUGUI[] leaderboardTexts; // Array of TextMeshProUGUI elements for the leaderboard
    public Image[] leaderboardImages; // Array of Image elements for the leaderboard backgrounds

    private GameObject[] enemyCars; // Array to store enemy car GameObjects
    private float[] distances; // Array to store distances between player and enemies
    private int[] sortedIndices; // Indices of leaderboardTexts sorted by distance

    void Start()
    {
        // Find all enemy cars in the scene based on tag
        UpdateEnemyCars();
        distances = new float[enemyCars.Length];
        sortedIndices = new int[enemyCars.Length];
    }

    void Update()
    {
        UpdateDistances();
        SortLeaderboard();
        UpdateLeaderboardUI();
    }

    void UpdateEnemyCars()
    {
        enemyCars = GameObject.FindGameObjectsWithTag(enemyCarTag);
    }

    void UpdateDistances()
    {
        for (int i = 0; i < enemyCars.Length; i++)
        {
            distances[i] = Vector3.Distance(playerCar.position, enemyCars[i].transform.position);
        }
    }

    void SortLeaderboard()
    {
        for (int i = 0; i < enemyCars.Length; i++)
        {
            sortedIndices[i] = i;
        }

        System.Array.Sort(distances, sortedIndices);
    }

    void UpdateLeaderboardUI()
    {
        int leaderboardCount = Mathf.Min(leaderboardTexts.Length, enemyCars.Length);

        for (int i = 0; i < leaderboardCount; i++)
        {
            int index = sortedIndices[i];
            TextMeshProUGUI text = leaderboardTexts[i];
            Image parentImage = leaderboardImages[i];

            // Update the text content
            text.text = "Enemy " + (i + 1) + ": " + distances[index].ToString("F2") + "m";

            // Smoothly fade the parent image using DoTween
            parentImage.DOFade(1f, 0.5f).SetEase(Ease.OutQuad); // Fade in
            parentImage.DOFade(0f, 0.5f).SetDelay(3f).SetEase(Ease.InQuad); // Fade out after 3 seconds
        }

        // Clear remaining leaderboard slots
        for (int i = leaderboardCount; i < leaderboardTexts.Length; i++)
        {
            leaderboardTexts[i].text = "";
        }
    }

}
