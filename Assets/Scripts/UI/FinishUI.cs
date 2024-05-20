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
        int[] sortedIndices = leaderboardSystem.GetSortedIndices();

        for (int i = 0; i < leaderboardTexts.Length; i++)
        {
            if (i < sortedIndices.Length)
            {
                int carIndex = sortedIndices[i];
                string carName = (carIndex == 0) ? "Player" : $"Car {carIndex + 1}";

                // Format without laps, showing score
                leaderboardTexts[i].text = $"{(i + 1)}. {carName}"; // F2 for 2 decimal places
            }
            else
            {
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
