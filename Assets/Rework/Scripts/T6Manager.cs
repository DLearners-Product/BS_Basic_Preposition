using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class T6Manager : MonoBehaviour
{
    public GameObject[] Questions;
    public GameObject[] Answers;

    private GameObject selectedQuestion;
    private Color defaultColor = Color.white;

    public Text counter;

    public GameObject frame1;

    public GameObject frame2;

    public ParticleSystem particles;

    public AudioSource source;

    public AudioClip correctAnswer;

    public AudioClip wrongAnswer;

    public GameObject activityCompleted;

    void Start()
    {
        // Initialize the button click listeners
        foreach (var question in Questions)
        {
            question.GetComponent<Button>().onClick.AddListener(() => OnQuestionClicked(question));
        }

        foreach (var answer in Answers)
        {
            answer.GetComponent<Button>().onClick.AddListener(() => OnAnswerClicked(answer));
        }
    }

    void OnQuestionClicked(GameObject question)
    {
        if (selectedQuestion != null)
        {
            // Unselect the previously selected question
            selectedQuestion.GetComponent<Image>().DOColor(defaultColor, 0.3f);
        }

        // Set the new selected question
        selectedQuestion = question;

        // Highlight the new selected question
        question.GetComponent<Image>().DOColor(Color.yellow, 0.3f);
    }

    void OnAnswerClicked(GameObject answer)
    {
        if (selectedQuestion == null) return; // No question selected

        if (selectedQuestion.name == answer.name) // Correct Match
        {
            particles.Play();
            source.clip = correctAnswer;
            source.Play();
            DoCorrectMatchEffect(selectedQuestion, answer);
            int currentCounterValue = int.Parse(counter.text);

            if (currentCounterValue < 7)
            {
                currentCounterValue++;
                counter.text = currentCounterValue.ToString();
                
            }
            if (currentCounterValue == 4)
            {
                StartCoroutine(Delay());
            }

             if (currentCounterValue == 7)
            {
               activityCompleted.SetActive(true);
            }
             
             
            


            // Disable interaction
            selectedQuestion.GetComponent<Button>().interactable = false;
            answer.GetComponent<Button>().interactable = false;

            // Reset selection
            selectedQuestion = null;
        }
        else // Wrong Match
        {
            source.clip = wrongAnswer;
            source.Play();
            DoWrongMatchEffect(selectedQuestion, answer);

            // Reset selection
            selectedQuestion.GetComponent<Image>().DOColor(defaultColor, 0.3f);
            selectedQuestion = null;
        }
    }

    void DoCorrectMatchEffect(GameObject question, GameObject answer)
    {
        // Fancy correct match effect
        question.transform.DOScale(1.2f, 0.3f).OnComplete(() => question.transform.DOScale(1f, 0.2f));
        answer.transform.DOScale(1.2f, 0.3f).OnComplete(() => answer.transform.DOScale(1f, 0.2f));
        question.GetComponent<Image>().DOColor(Color.green, 0.3f);
        answer.GetComponent<Image>().DOColor(Color.green, 0.3f);
    }

    void DoWrongMatchEffect(GameObject question, GameObject answer)
    {
        // Wrong match effect
        question.transform.DOShakePosition(0.5f, 10, 10);
        answer.transform.DOShakePosition(0.5f, 10, 10);
        question.GetComponent<Image>().DOColor(Color.red, 0.3f).OnComplete(() => question.GetComponent<Image>().DOColor(defaultColor, 0.3f));
        answer.GetComponent<Image>().DOColor(Color.red, 0.3f).OnComplete(() => answer.GetComponent<Image>().DOColor(defaultColor, 0.3f));
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1.2f);
        frame1.SetActive(false);
        frame2.SetActive(true);

    }
}
