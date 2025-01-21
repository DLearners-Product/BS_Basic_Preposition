using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class T4Manager : MonoBehaviour
{
     public GameObject[] questions; // Array of questions
    private int currentQuestionIndex = 0; // Tracks the current question
    private int correctAnswersSelected = 0; // Tracks correct answers selected for the current question

    public AudioSource source;
    public AudioClip correctAnswer;
    public AudioClip wrongAnswer;
    public GameObject activityCompleted;

    private void Start()
    {
        // Activate the first question
        ActivateQuestion(0);
    }

    private void ActivateQuestion(int questionIndex)
    {
        // Deactivate all questions
        foreach (var question in questions)
        {
            question.SetActive(false);

            // Reset the Canvas Group alpha to 0
            CanvasGroup canvasGroup = question.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0; // Make sure to reset alpha before activating a new question
            }
        }

        // Activate the current question
        GameObject currentQuestion = questions[questionIndex];
        currentQuestion.SetActive(true);

        // Set the Canvas Group alpha to 1
        CanvasGroup currentCanvasGroup = currentQuestion.GetComponent<CanvasGroup>();
        if (currentCanvasGroup != null)
        {
            currentCanvasGroup.alpha = 1; // Ensure the current question is fully visible
            currentCanvasGroup.interactable = true;
            currentCanvasGroup.blocksRaycasts = true;
        }

        // Reset the correct answers counter
        correctAnswersSelected = 0;
    }

    public void OnOptionSelected(Button selectedOption)
    {
        string optionTag = selectedOption.tag; // Get the tag of the selected option

        if (optionTag == "Correct")
        {
            selectedOption.gameObject.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
            source.clip = correctAnswer;
            source.Play();

            // Disable the button if the answer is correct
            selectedOption.interactable = false;
            correctAnswersSelected++;

            // Check if all correct answers are selected
            if (correctAnswersSelected >= GetCorrectAnswerCountForCurrentQuestion())
            {
                // Disable all wrong options
                DisableWrongOptions();

                // Move to the next question
                Invoke("ActivateNextQuestion", 1f); // Optional delay for transition
            }
        }
        else if (optionTag == "Wrong")
        {
            // Optionally handle incorrect answers (e.g., feedback or retry logic)
            source.clip = wrongAnswer;
            source.Play();
        }
    }

    private void DisableWrongOptions()
    {
        // Disable all wrong options for the current question
        Button[] options = questions[currentQuestionIndex].GetComponentsInChildren<Button>();
        foreach (Button option in options)
        {
            if (option.CompareTag("Wrong"))
            {
                option.interactable = false; // Disable wrong options
            }
        }
    }

    private int GetCorrectAnswerCountForCurrentQuestion()
    {
        // Get the correct answer count for the current question by counting buttons with "Correct" tag
        int correctAnswerCount = 0;
        Button[] options = questions[currentQuestionIndex].GetComponentsInChildren<Button>();
        foreach (Button option in options)
        {
            if (option.CompareTag("Correct"))
            {
                correctAnswerCount++;
            }
        }
        return correctAnswerCount;
    }

    private void ActivateNextQuestion()
    {
        // Deactivate the current question
        questions[currentQuestionIndex].SetActive(false);

        // Move to the next question if available
        currentQuestionIndex++;
        if (currentQuestionIndex < questions.Length)
        {
            ActivateQuestion(currentQuestionIndex);
        }
        else
        {
            Debug.Log("All questions completed!");
            activityCompleted.SetActive(true);
            // Optionally, handle end of the quiz logic
        }
    }
}
