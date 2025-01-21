using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class T6DotweenManager : MonoBehaviour
{
    public GameObject[] objects; // Array of GameObjects to activate
    public float activationDelay = 0.5f; // Delay between activating each GameObject
    public float activationDuration = 0.5f; // Duration of the activation effect
   

    private void Start()
    {
        // Start the activation sequence
        ActivateObjectsOneByOne();

        // Add click listeners to all objects
        foreach (var obj in objects)
        {
            Button button = obj.GetComponent<Button>();
            // if (button != null)
            // {
            //     button.onClick.AddListener(() => OnObjectClicked(obj));
            // }
        }
    }

    private void ActivateObjectsOneByOne()
    {
        // Iterate through the objects array
        for (int i = 0; i < objects.Length; i++)
        {
            int index = i; // Store the index for use in the lambda
            DOVirtual.DelayedCall(activationDelay * i, () => GamifiedActivate(objects[index]));
        }
    }

    private void GamifiedActivate(GameObject obj)
    {
        // Ensure the object is inactive initially
        obj.SetActive(false);

        // Reset transform properties for effect
        obj.transform.localScale = Vector3.zero; // Start with zero scale
        obj.GetComponent<CanvasGroup>()?.DOFade(0, 0); // Start with zero alpha if it has CanvasGroup

        // Activate the object
        obj.SetActive(true);

        // Play activation effect
        Sequence activationSequence = DOTween.Sequence();

        // Optional movement effect: Move from below
        Vector3 originalPosition = obj.transform.localPosition;
        obj.transform.localPosition += new Vector3(0, -100, 0); // Start slightly below
        activationSequence.Append(obj.transform.DOLocalMove(originalPosition, activationDuration).SetEase(Ease.OutBack));

        // Scale up effect
        activationSequence.Join(obj.transform.DOScale(Vector3.one, activationDuration).SetEase(Ease.OutElastic));

        // Fade-in effect if a CanvasGroup is present
        CanvasGroup canvasGroup = obj.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            activationSequence.Join(canvasGroup.DOFade(1, activationDuration));
        }

        activationSequence.Play();
    }

    // private void OnObjectClicked(GameObject obj)
    // {
    //     // Play click effect
    //     Sequence clickSequence = DOTween.Sequence();

    //     // Scale down slightly and back up
    //     clickSequence.Append(obj.transform.DOScale(Vector3.one * 0.9f, clickEffectDuration / 2).SetEase(Ease.OutQuad));
    //     clickSequence.Append(obj.transform.DOScale(Vector3.one, clickEffectDuration / 2).SetEase(Ease.OutElastic));

    //     // Flash the color if the object has an Image component
    //     Image image = obj.GetComponent<Image>();
    //     if (image != null)
    //     {
    //         Color originalColor = image.color;
    //         clickSequence.Join(image.DOColor(clickFlashColor, clickEffectDuration / 2).SetEase(Ease.InOutQuad));
    //         clickSequence.Append(image.DOColor(originalColor, clickEffectDuration / 2).SetEase(Ease.InOutQuad));
    //     }

    //     // Rotate slightly for added effect
    //     clickSequence.Join(obj.transform.DORotate(new Vector3(0, 0, 15), clickEffectDuration / 2).SetLoops(2, LoopType.Yoyo));

    //     clickSequence.Play();
    // }
}
