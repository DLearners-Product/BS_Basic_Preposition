using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class T8AnswerScaleup : MonoBehaviour
{
    public GameObject scaleup;

    public AudioSource source;

    public AudioClip clip;

    public GameObject[] scaleDownObjects;

    void Start()
    {
        // Set initial scale to zero
        scaleup.transform.localScale = Vector3.zero;

        // Apply DOTween scale-up effect when the game starts
        scaleup.transform.DOScale(new Vector3(1f, 1f, 1f), 1.5f)  // Scaling to (1, 1, 1) over 1 second
            .SetEase(Ease.OutElastic);  // Using Ease.OutBounce for a fancy effect

        StartCoroutine(Delay());
    }



    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1.5f);



        foreach (var obj in scaleDownObjects)
        {
            // Punch scale for a quick bounce effect before shrinking
            obj.transform.DOPunchScale(Vector3.one * 0.1f, 0.3f, 5, 1)
                .OnComplete(() =>
                {
                    // After punch, scale down to zero with elastic effect
                    obj.transform.DOScale(Vector3.zero, 1f)
                            .SetEase(Ease.InElastic); // Stretchy shrink
                            StartCoroutine(audioDelay());

                });
        }
    }

    IEnumerator audioDelay()
    {
        yield return new WaitForSeconds(0.5f);
        source.clip = clip;

        source.Play();
    }

}
