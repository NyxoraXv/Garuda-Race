using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class FinishUIScript : MonoBehaviour
{
    // UI Elements (Assign in the Unity Inspector)
    public GameObject finishUI;
    public TextMeshProUGUI[] leaderboardTexts;

    // Data Source (Assign in the Unity Inspector)
    public LeaderboardSystem leaderboardSystem;

    private void Start()
    {
        // Ensure the UI is hidden on game start
        finishUI.SetActive(false);

        // Potentially initialize the leaderboardSystem here if necessary
    }

    public void ShowFinishUI()
    {
        // Activate the UI after the race ends
        finishUI.SetActive(true);

        // Update and display the leaderboard results
        UpdateLeaderboard();

        // Release the cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Reset any lingering input to avoid issues
        if (Input.mousePresent) // Added as a safety check in case a mouse isn't plugged in.
        {
            Input.ResetInputAxes();
        }
    }

    private void UpdateLeaderboard()
    {
        // Get the leaderboard data (array of car indices sorted by performance)
        int[] sortedIndices = leaderboardSystem.GetSortedIndices();
        int[] lapsCompleted = leaderboardSystem.GetLapsCompleted();

        // Populate the UI leaderboard
        for (int i = 0; i < leaderboardTexts.Length; i++)
        {
            if (i < sortedIndices.Length)
            {
                int carIndex = sortedIndices[i];

                // Determine the car's name (special case for player)
                string carName = (carIndex == 0) ? "Player" : $"Car {carIndex + 1}";

                // Display the car's name and lap count
                leaderboardTexts[i].text = $"{carName}: {lapsCompleted[carIndex]} laps";
            }
            else
            {
                // Clear any unused leaderboard slots
                leaderboardTexts[i].text = "";
            }
        }
    }

    // Button Event Handlers (Assign in the Unity Inspector)
    public void RestartRace()
    {
        // Reload the current scene to start a new race
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // Lock the cursor for gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void GoToHome()
    {
        // Load the main menu scene
        SceneManager.LoadScene("MainMenu"); // Replace "MainMenu" with your actual scene name
    }
}
