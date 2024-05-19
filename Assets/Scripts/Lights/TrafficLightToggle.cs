using UnityEngine;
using System.Collections;

public class TrafficLightTrack : MonoBehaviour
{
    public MeshRenderer[] trackSections;  // Assign in the Inspector
    public Material redMaterial;
    public Material greenMaterial;

    private int currentIndex = 0;

    void Start()
    {
        // Initial setup (all red)
        foreach (MeshRenderer renderer in trackSections)
        {
            renderer.material = redMaterial;
        }

        StartCoroutine(RaceStartSequence());
    }

    IEnumerator RaceStartSequence()
    {
        // "1-2-3 Go!" countdown
        for (int count = 0; count < 3; count++)
        {
            trackSections[currentIndex].material = greenMaterial;
            yield return new WaitForSeconds(1f);
            trackSections[currentIndex].material = redMaterial;
            currentIndex = (currentIndex + 1) % trackSections.Length; // Move to the next section (wrap around)
        }

        // Start the race (all green)
        foreach (MeshRenderer renderer in trackSections)
        {
            renderer.material = greenMaterial;
        }

        // You can now start your race logic here!
    }
}
