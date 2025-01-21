
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class T11DotweenManager : MonoBehaviour
{
     public GameObject[] objects;
    [SerializeField] private AudioClip scaleSound;
    public GameObject questionImage;
    public ParticleSystem correctAnswerParticles;

    private AudioSource audioSource;
    private CanvasGroup questionCanvasGroup;

    void Start()
    {
        if (scaleSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = scaleSound;
        }

        questionImage.transform.localScale = Vector3.zero;
        questionCanvasGroup = questionImage.GetComponent<CanvasGroup>();
        questionCanvasGroup.alpha = 0;

        StartCoroutine(ScaleObjectsSequentially());
    }

    private IEnumerator ScaleObjectsSequentially()
    {
        foreach (var obj in objects)
        {
            obj.transform.DOScale(new Vector3(1.2f, 1.2f, 1f), 0.3f)
                .SetEase(Ease.OutQuad)
                .OnStart(() => AddFancyEffects(obj))
                .OnComplete(() => ReturnToOriginalScale(obj));

            yield return new WaitForSeconds(0.2f);
        }

        ShowQuestionImage();
    }

    private void AddFancyEffects(GameObject obj)
    {
        if (audioSource != null) audioSource.Play();

        obj.transform.DORotate(new Vector3(0, 0, 10f), 0.15f, RotateMode.LocalAxisAdd)
            .SetLoops(2, LoopType.Yoyo);
    }

    private void ReturnToOriginalScale(GameObject obj)
    {
        obj.transform.DOScale(Vector3.one, 0.15f)
            .OnComplete(() => MakeInteractable(obj));
    }

    private void MakeInteractable(GameObject obj)
    {
        var button = obj.GetComponent<Button>();
        if (button != null)
        {
            button.interactable = true;
            button.onClick.AddListener(() => CheckAnswer(obj));
        }
    }

    private void ShowQuestionImage()
    {
        questionImage.SetActive(true);
        questionImage.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        questionCanvasGroup.DOFade(1, 0.5f).SetEase(Ease.InOutQuad);
    }

    private void CheckAnswer(GameObject obj)
    {
        if (obj.CompareTag("correctanswer"))
        {
            CorrectAnswerEffect(obj);
        }
        // else if (obj.CompareTag("Ques"))
        // {
        //     // Apply only minimal wrong answer effects (no camera shake or color change)
        //     WrongAnswerEffect(obj, applyColor: false, shakeCamera: false);
        // }
        else
        {
            WrongAnswerEffect(obj, applyColor: true, shakeCamera: true);
        }
    }

    private void WrongAnswerEffect(GameObject obj, bool applyColor, bool shakeCamera)
    {
        // Apply red flicker only if applyColor is true
        if (applyColor)
        {
            obj.GetComponent<Image>().DOColor(Color.red, 0.05f).SetLoops(4, LoopType.Yoyo);
        }

        // Shake the object's position
        obj.transform.DOShakePosition(0.4f, strength: new Vector3(20f, 10f, 0), vibrato: 20);

        // Only shake the camera if shakeCamera is true
        if (shakeCamera)
        {
            Camera.main.transform.DOShakePosition(0.4f, strength: new Vector3(5f, 5f, 0), vibrato: 20);
        }
    }

    private void CorrectAnswerEffect(GameObject obj)
    {
        // Correct answer - pulse and particle celebration
        obj.GetComponent<Image>().DOColor(Color.green, 0.2f).SetEase(Ease.InOutFlash);
        obj.transform.DOScale(new Vector3(1.3f, 1.3f, 1f), 0.3f).SetEase(Ease.OutElastic)
            .SetLoops(2, LoopType.Yoyo);
        obj.transform.DOLocalMoveY(obj.transform.localPosition.y + 20f, 0.5f).SetLoops(2, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);

        // Trigger particle effect
        if (correctAnswerParticles != null)
        {
            correctAnswerParticles.transform.position = obj.transform.position;
            correctAnswerParticles.Play();
        }
    }
}
