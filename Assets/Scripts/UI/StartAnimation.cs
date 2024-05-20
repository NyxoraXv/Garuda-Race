using DG.Tweening;
using TMPro;
using UnityEngine;
using System.Collections;

public class CountdownAnimation : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    public AudioSource beepSound, raceStartMusic;

    void Start()
    {
        StartCoroutine(PlayCountdown());
    }

    IEnumerator PlayCountdown()
    {
        // 3...
        countdownText.text = "3";
        countdownText.transform.DOScale(Vector3.one * 2f, 0.5f).SetEase(Ease.OutBack);
        beepSound.Play();

        yield return new WaitForSeconds(1f);

        // 2...
        countdownText.text = "2";
        countdownText.transform.DOScale(Vector3.one, 0.5f); // Reset scale before next animation
        beepSound.Play();

        yield return new WaitForSeconds(1f);

        // 1...
        countdownText.text = "1";
        countdownText.transform.DOScale(Vector3.one * 2f, 0.5f).SetEase(Ease.OutBack);
        beepSound.Play();

        yield return new WaitForSeconds(1f);

        // GO!
        countdownText.text = "GO!";
        countdownText.transform.DOScale(Vector3.one * 3f, 0.2f).OnComplete(() => countdownText.gameObject.SetActive(false));
        raceStartMusic.Play();

        // Flashing Effect for "GO!"
        DOTween.To(() => countdownText.faceColor, x => countdownText.faceColor = x, Color.white, 0.2f)
            .SetLoops(-1, LoopType.Yoyo);
    }
}
