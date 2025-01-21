using UnityEngine;
using TMPro;
using System.Collections;
using SightWords2;
using UnityEngine.UI;
using TMPro;


public class ClickableText : MonoBehaviour
{
    [SerializeField] private string answer;
    [SerializeField] private Color correctColor = Color.green; // Color for the correct answer
    [SerializeField] private Color incorrectColor = Color.red; // Color for the incorrect answer
    [SerializeField] private float revertDelay = 2f; // Time in seconds to revert back to the original color

    private Camera cam;
    private TextMeshProUGUI text;
    private string originalText;
    private string lastClickedWord;
    private StoryTime REF_StoryTime;

    [SerializeField] private GameObject[] GA_Objects;
    

    [SerializeField] private GameObject G_ActivityCompleted;

    private int _currentIndex;


    public int correctanswercount;

    public TextMeshProUGUI counter;
    
    public GameObject transparenScreen;

    


    void Start()
    {
        cam = Camera.main;
        text = GetComponent<TextMeshProUGUI>();
        originalText = text.text; // Save the original text
        REF_StoryTime = FindObjectOfType<StoryTime>();
        REF_StoryTime.RegisterClickableText(this);
        //  ShowQuestion();
    }

    // private void ShowQuestion()
    // {


    //     if (_currentIndex == GA_Objects.Length)
    //     {
    //         //showing activity completed 
    //         Invoke(nameof(ShowActivityCompleted), 1f);
    //     }


    // }


    void OnDestroy()
    {
        if (REF_StoryTime != null)
        {
            REF_StoryTime.UnregisterClickableText(this);
        }
    }


    public void SetClickable(bool value)
    {
        enabled = value;
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            int wordIndex = TMP_TextUtilities.FindIntersectingWord(text, Input.mousePosition, cam);

            if (wordIndex != -1)
            {
                lastClickedWord = text.textInfo.wordInfo[wordIndex].GetWord();
                Check(wordIndex);
            }
        }
    }


    private void Check(int wordIndex)
    {
        if (lastClickedWord.ToLower().Equals(answer.ToLower()))
        {
            HighlightWord(wordIndex, correctColor);
           
            StopAllCoroutines();
            SetClickable(false);
        }
        else
        {
            REF_StoryTime.THI_WrongAnswer();
            HighlightWord(wordIndex, incorrectColor);
            
            REF_StoryTime.ReportWrongAnswer(lastClickedWord);
            StartCoroutine(RevertColor());
        }
    }




    private void HighlightWord(int wordIndex, Color color)
    {
        TMP_WordInfo wordInfo = text.textInfo.wordInfo[wordIndex];

        string word = wordInfo.GetWord();
        string colorTag = $"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>{word}</color>";

        int startIndex = wordInfo.firstCharacterIndex;
        int length = wordInfo.characterCount;

        string preText = originalText.Substring(0, startIndex);
        string postText = originalText.Substring(startIndex + length);

        text.text = preText + colorTag + postText;
    }

    public void THI_CorrectAnswer(Transform obj)
    {
        // ScoreManager.instance.RightAnswer(qIndex, questionID: question.id, answerID: GetOptionID(obj.transform.GetChild(1).name));

        // if (qIndex < GA_Objects.Length - 1)
        //     qIndex++;

        // GetData(qIndex);
        // Increment correct answer count
        // Increment correct answer count
        correctanswercount++;
        obj.gameObject.GetComponent<AudioSource>().Play();
          REF_StoryTime.ReportCorrectanswer(obj.gameObject.name);
        int currentCounterValue = int.Parse(counter.text);
        if (currentCounterValue < 6)
        {
            currentCounterValue++;
            counter.text = currentCounterValue.ToString();
        }

        // Log the correct answer count
        Debug.Log("Correct Answer Count: " + correctanswercount);

        // Check if the correct answer count is 8
        if (correctanswercount >= 6)
        {
            // Start a coroutine to enable the GameObject after 1 second
          //  
            transparenScreen.SetActive(true);
        }

        // Your existing logic
        _currentIndex++;
        obj.gameObject.GetComponent<Button>().interactable = false;

        Debug.Log("Index Switched");


    }

    public void showActivityCompleted()
    {
        StartCoroutine(EnableActivityCompleted());
    }
    private IEnumerator EnableActivityCompleted()
    {
        // Wait for 1 second
        yield return new WaitForSeconds(1f);

        // Enable the GameObject after the delay
        if (G_ActivityCompleted != null)
        {
            G_ActivityCompleted.SetActive(true);
            this.gameObject.SetActive(false);
            Debug.Log("Activity Completed Screen Enabled!");
          //  BlendedOperations.instance.NotifyActivityCompleted();
        }
        else
        {
            Debug.LogWarning("G_ActivityCompleted is not assigned in the Inspector.");
        }
    }


    private IEnumerator RevertColor()
    {
        yield return new WaitForSeconds(revertDelay);

        text.text = originalText;
    }

    // private void ShowActivityCompleted()
    // {
    //     G_ActivityCompleted.SetActive(true);
    //     // BlendedOperations.instance.NotifyActivityCompleted();


    // }
}
