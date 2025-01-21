using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class T4BeeAnimation : MonoBehaviour
{

    [Header("End Position")]
    [SerializeField] private float xPos; // X position for the end point
    [SerializeField] private float yPos; // Y position for the end point

    [Header("Movement Settings")]
    [SerializeField] private float moveDuration = 2f; // Duration of the movement
    [SerializeField] private float randomnessStrength = 10f; // Intensity of randomness during movement
    [SerializeField] private int randomSteps = 10; // Number of steps for random positions

    [Header("Canvas Group Settings")]
    [SerializeField] private CanvasGroup movingObjectCanvasGroup; // Canvas group of the moving object
    [SerializeField] private CanvasGroup fadeInCanvasGroup; // Canvas group of the object to fade in after movement
    [SerializeField] private float fadeDuration = 1f; // Duration of the fade transitions

    private RectTransform rectTransform;

    public GameObject[] options;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        if (rectTransform == null)
        {
            Debug.LogError("This script requires a RectTransform component.");
            return;
        }

        // Ensure canvas groups are set up correctly
        if (movingObjectCanvasGroup != null)
            movingObjectCanvasGroup.alpha = 1; // Keep the moving object visible initially

        if (fadeInCanvasGroup != null)
            fadeInCanvasGroup.alpha = 0; // Start with fade-in object invisible

        // Start moving the object
        MoveToPosition();
    }

    private void MoveToPosition()
    {
        // Define the start and end positions
        Vector2 startPosition = rectTransform.anchoredPosition;
        Vector2 endPosition = new Vector2(xPos, yPos);

        // Create a sequence for smooth movement
        Sequence movementSequence = DOTween.Sequence();

        // Add intermediate random positions to the sequence
        for (int i = 1; i <= randomSteps; i++)
        {
            // Calculate a step position between the start and end positions
            Vector2 stepPosition = Vector2.Lerp(startPosition, endPosition, (float)i / randomSteps);

            // Apply slight randomness to the step position
            stepPosition += new Vector2(
                Random.Range(-randomnessStrength, randomnessStrength),
                Random.Range(-randomnessStrength, randomnessStrength)
            );

            // Add the step to the sequence
            movementSequence.Append(rectTransform.DOAnchorPos(stepPosition, moveDuration / randomSteps).SetEase(Ease.InOutSine));
        }

        // Add the final movement to the exact end position
        movementSequence.Append(rectTransform.DOAnchorPos(endPosition, moveDuration / randomSteps).SetEase(Ease.InOutSine));

        // Callback on completion
        movementSequence.OnComplete(() =>
        {
            Debug.Log("UI element has reached its destination with smooth randomness!");
            FadeInCanvasGroup(); // Fade in the canvas after movement completion
            FadeOutCanvasGroup(); // Fade out the moving object canvas group after completion
        });
    }

    private void FadeInCanvasGroup()
    {
        if (fadeInCanvasGroup != null)
        {
            fadeInCanvasGroup.DOFade(1, fadeDuration).OnComplete(() =>
            {
                Debug.Log("Canvas group faded in after movement.");
            });
        }
    }

    private void FadeOutCanvasGroup()
{
    if (movingObjectCanvasGroup != null)
    {
        movingObjectCanvasGroup.DOFade(0, fadeDuration).OnComplete(() =>
        {
            Debug.Log("Moving object's canvas group faded out.");
            ActivateObjectsOneByOne(); // Activate the objects after fade-out
        });
    }
}

private void ActivateObjectsOneByOne()
{
     if (options == null || options.Length == 0)
    {
        Debug.LogWarning("No options to activate.");
        return;
    }

    Sequence activationSequence = DOTween.Sequence();

    foreach (GameObject option in options)
    {
        // Ensure the object starts inactive and scaled down
        option.SetActive(false);
        RectTransform rectTransform = option.GetComponent<RectTransform>();

        if (rectTransform != null)
        {
            rectTransform.localScale = Vector3.zero; // Start with scale 0
        }

        // Add a fancy activation effect
        activationSequence.AppendCallback(() =>
        {
            option.SetActive(true); // Activate the GameObject
            Debug.Log($"{option.name} activated!");

            if (rectTransform != null)
            {
                // Add a scaling effect with a bounce
                rectTransform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce);

                // Optional: Add a fade-in effect (if it has a CanvasGroup)
                CanvasGroup canvasGroup = option.GetComponent<CanvasGroup>();
                if (canvasGroup != null)
                {
                    canvasGroup.alpha = 0; // Start with alpha 0
                    canvasGroup.DOFade(1, 0.5f);
                }
            }
        });

        // Add a delay between activations
        activationSequence.AppendInterval(0.5f);
    }

    activationSequence.OnComplete(() =>
    {
        Debug.Log("All options have been activated with fancy effects!");
    });
}
}