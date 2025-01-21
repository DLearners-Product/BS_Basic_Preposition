using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class T8DoTweenManager : MonoBehaviour
{
    public GameObject[] objects;
    private float scaleDuration = 0.2f;
    private float activationInterval = 0.3f;

    public GameObject[] scaleobjects; // The two objects
    public float scaleUpSize = 2.3f; // Scale size to increase to
    public float scaleDownSize = 2.1f; // Scale size to return to
    public float duration = 2f; // Total duration

    public GameObject clickEffect;


    private void Start()
    {
        // Set initial scale of all objects to zero
        foreach (GameObject obj in objects)
        {
            obj.transform.localScale = Vector3.zero;
        }

        StartCoroutine(ActivateObjects());
    }

    private IEnumerator ActivateObjects()
    {
        // Scale up the first object with OutBounce easing, no punch scale
        if (objects.Length > 0)
        {
            objects[0].transform.localScale = Vector3.zero; // Ensure it starts at scale zero
            objects[0].GetComponent<CanvasGroup>().alpha = 0; // Make sure it starts invisible

            // Combine scaling, rotation, and fade-in for the first object
            objects[0].transform.DOScale(Vector3.one, scaleDuration)
                .SetEase(Ease.OutBounce); // OutBounce for the first object scaling
            objects[0].transform.DORotate(new Vector3(0, 0, 360), 1f, RotateMode.FastBeyond360); // Spin while scaling
            objects[0].GetComponent<CanvasGroup>().DOFade(1f, 0.5f); // Fade in

            yield return new WaitForSeconds(activationInterval);
        }

        // Activate the remaining objects sequentially with punch scaling, rotation, and fade-in
        for (int i = 1; i < objects.Length; i++)
        {
            GameObject obj = objects[i];

            obj.transform.localScale = Vector3.zero; // Ensure it starts at scale zero
            obj.GetComponent<CanvasGroup>().alpha = 0; // Make sure it starts invisible

            // Combine scaling, rotation, and punch scale
            obj.transform.DOScale(Vector3.one, scaleDuration)
                .OnComplete(() => obj.transform.DOPunchScale(Vector3.one * 0.2f, 0.5f, 5, 0.5f)); // Punch scale after scaling
            obj.transform.DORotate(new Vector3(0, 0, 360), 1f, RotateMode.FastBeyond360); // Spin while scaling
            obj.GetComponent<CanvasGroup>().DOFade(1f, 0.5f); // Fade in

            // Wait before activating the next object
            yield return new WaitForSeconds(activationInterval);
        }

        // After all objects are activated, call StartScaleEffect()
        StartScaleEffect();
    }

    void StartScaleEffect()
    {
        foreach (GameObject obj in scaleobjects)
        {
            // Start at the current scale
            obj.transform.localScale = new Vector3(scaleDownSize, scaleDownSize, scaleDownSize);

            // Create a sequence for scaling
            Sequence scaleSequence = DOTween.Sequence();

            // Scale up with Ease.InOutBack
            scaleSequence.Append(obj.transform.DOScale(new Vector3(scaleUpSize, scaleUpSize, scaleUpSize), duration / 3f)
                .SetEase(Ease.InOutBack)); // Slight overshoot at both start and end

            // Scale back down with Ease.InBounce
            scaleSequence.Append(obj.transform.DOScale(new Vector3(scaleDownSize, scaleDownSize, scaleDownSize), duration / 4)
                .SetEase(Ease.InCubic)); // Bouncy effect as it scales down

            // Play the sequence
            scaleSequence.Play();
        }
    }

     public void CorrectAnswerEffect()
    {
        // Correct answer - pulse and particle celebration
      //  clickEffect.GetComponent<Image>().DOColor(Color.green, 0.2f).SetEase(Ease.InOutFlash);
    clickEffect.transform.DOScale(new Vector3(0.8f, 0.8f, 1f), 0.3f).SetEase(Ease.OutElastic)
            .SetLoops(2, LoopType.Yoyo);
        clickEffect.transform.DOLocalMoveY(clickEffect.transform.localPosition.y + 20f, 0.5f).SetLoops(2, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);

        
    }
}


