using DG.Tweening;
using UnityEngine;

public class FinishUIAnimation : MonoBehaviour
{
    [System.Serializable]
    public struct UIAnimationData
    {
        public RectTransform element;
        public Vector3 startPosition;
        public Vector3 endPosition;
        public Vector3 startRotation;
        public Vector3 endRotation;
    }

    [Header("Animation Properties")]
    public float animationDuration = 1.5f;
    public Ease animationEase = Ease.OutBack;

    [Header("UI Animation Data")]
    public UIAnimationData[] uiAnimations;

    void Start()
    {
        // Reset to start positions and rotations before animating
        foreach (var animationData in uiAnimations)
        {
            animationData.element.anchoredPosition = animationData.startPosition;
            animationData.element.localRotation = Quaternion.Euler(animationData.startRotation);
        }

        // Start the animation sequence
        AnimateUI();
    }

    void AnimateUI()
    {
        Sequence sequence = DOTween.Sequence();

        foreach (var animationData in uiAnimations)
        {
            sequence.Join(animationData.element.DOAnchorPos(animationData.endPosition, animationDuration).SetEase(animationEase));
            sequence.Join(animationData.element.DOLocalRotate(animationData.endRotation, animationDuration).SetEase(animationEase));
        }
    }
}
