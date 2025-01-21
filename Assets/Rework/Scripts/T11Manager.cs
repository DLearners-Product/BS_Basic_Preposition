using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening.Core.Easing;
using System.Collections.Generic;

public class T11Manager : MonoBehaviour
{
 #region user input
    //==================================================================================================

    [Header("USER INPUT---------------------------------------------------------")]
    public float questionSwitchDelay;   //delay for switching a question


    //!end of region - unity reference variables 
    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
    #endregion



    #region unity reference variables
    //==================================================================================================

    [Header("TEXTMESHPRO---------------------------------------------------------")]
    [SerializeField] private Text TXT_Current;
    [SerializeField] private Text TXT_Total;


    [Space(10)]
    [Header("GAME OBJECT---------------------------------------------------------")]
    [SerializeField] private GameObject[] GA_Objects;
    [SerializeField] private GameObject G_TransparentScreen;
    [SerializeField] private GameObject G_ActivityCompleted;

    // [SerializeField] private GameObject G_Camera_Turnoff;

   // [SerializeField] private GameObject G_Transistion;

  //  [SerializeField] private GameObject G_FlashImage;

    [Space(10)]
    [Header("Audio---------------------------------------------------------")]
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip correctAnswer;
    [SerializeField] private AudioClip wrongAnswer;


    [Space(10)]
    [Header("PARTICLES---------------------------------------------------------")]
    // [SerializeField] private ParticleSystem PS_CorrectAnswer_RightSide;
    // [SerializeField] private ParticleSystem PS_CorrectAnswer_LeftSide;



    //!end of region - unity reference variables
    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
    #endregion



    #region local variables
    //==================================================================================================

    private int _currentIndex;
    private string _selectedAnswer;



    //!end of region - local variables
    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
    #endregion



    #region gameplay logic
    //==================================================================================================

    // #region QA

    // private int qIndex;
    // public GameObject questionGO;
    // public GameObject[] optionsGO;
    // public Dictionary<string, Component> additionalFields;
    // Component question;
    // Component[] options;
    // Component[] answers;

    // #endregion


    void Start()
    {
    //     #region DataSetter
    //   //  Main_Blended.OBJ_main_blended.levelno = 3;
    //     QAManager.instance.UpdateActivityQuestion();
    //     qIndex = 0;
    //     GetData(qIndex);
    //     GetAdditionalData();
    //     AssignData();
    //     #endregion
        _currentIndex = -1;
        TXT_Total.text = GA_Objects.Length.ToString();

        // for (int i = 0; i < GA_Objects.Length; i++)
        // {
        //     GA_Objects[i].transform.GetChild(3).transform.localScale = Vector3.zero;
        // }

        ShowQuestion();
    }


    private void ShowQuestion()
    {
        _currentIndex++;
        // G_Transistion.SetActive(false);



        //disabling previous question
        if (_currentIndex > 0)
        {
            GA_Objects[_currentIndex - 1].SetActive(false);
        }

        if (_currentIndex == GA_Objects.Length)
        {
            //showing activity completed 
            Invoke(nameof(ShowActivityCompleted), 1f);
        }
        else
        {
            //enabling current question
            GA_Objects[_currentIndex].SetActive(true);
            UpdateCounter();
        }

    }


    private IEnumerator IENUM_CorrectAnswer(Transform obj)
    {

        //*correct answer
        //TODO: additional functionality for correct answer


        // Text textComponent = obj.GetChild(1).GetComponent<Text>();
        // Color originalColor = textComponent.color;

        // // Change color to green
        // textComponent.color = Color.green;
        // textComponent.SetVerticesDirty(); // Force UI update
        // Debug.Log("Set color to green: " + textComponent.color);
   



        //--------------------------------------------

        //disabling user interation
        G_TransparentScreen.SetActive(true);
        yield return new WaitForSeconds(1f);

     //   textComponent.color = originalColor;



      

        //update correct answer as selected answer
      //  GA_Objects[_currentIndex].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = _selectedAnswer;





        yield return new WaitForSeconds(1);

        
      //  Utilities.Instance.ANIM_Appear(GA_Objects[_currentIndex].transform.GetChild(3));


      



        //wait until questionSwitchDelay and switch to next question
        yield return new WaitForSeconds(questionSwitchDelay);
       
        ShowQuestion();
       
       




    }


    private IEnumerator IENUM_WrongAnswer(Transform obj)
    {
        //!wrong answer
        //TODO: additional functionality for wrong answer



        // obj.GetComponent<Animator>().SetTrigger("BuzzerPress");

        // Text textComponent = obj.GetChild(1).GetComponent<Text>();
        // Color originalColor = textComponent.color;

        // // Change color to red
        // textComponent.color = Color.red;

        //------------------------------------------

        //disabling user interation
        G_TransparentScreen.SetActive(true);

        //animation
        //Utilities.Instance.ANIM_Wrong(obj);

        //waitin
        yield return new WaitForSeconds(1f);

       // textComponent.color = originalColor;


        //enabling user interation
        G_TransparentScreen.SetActive(false);

    }


    private void ShowActivityCompleted()
    {
        G_ActivityCompleted.SetActive(true);
     //   BlendedOperations.instance.NotifyActivityCompleted();
        G_TransparentScreen.SetActive(false);

    }


    private void UpdateCounter()
    {
        //updating text
        TXT_Current.text = (_currentIndex + 1).ToString();

        //animation
        // Utilities.Instance.ANIM_ClickEffect(TXT_Current.transform.parent);

        //enabling user interaction
        G_TransparentScreen.SetActive(false);
    }


    public void THI_CorrectAnswer(Transform obj)
    {
        // ScoreManager.instance.RightAnswer(qIndex, questionID: question.id, answerID: GetOptionID(obj.transform.GetChild(1).name));

        // if (qIndex < GA_Objects.Length - 1)
        //     qIndex++;

        // GetData(qIndex);
        source.clip = correctAnswer;
        source.Play();
       // _selectedAnswer = obj.GetChild(1).GetComponent<Text>().text;
        obj.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
       
        StartCoroutine(IENUM_CorrectAnswer(obj));
        StartCoroutine(IENUM_DummyDelay());
        //  obj.transform.GetChild(3).gameObject.SetActive(true);
    }


    public void THI_WrongAnswer(Transform obj)
    {
      //  ScoreManager.instance.WrongAnswer(qIndex, questionID: question.id, answerID: GetOptionID(obj.transform.GetChild(1).name));
        source.clip = wrongAnswer;
        source.Play();
        StartCoroutine(IENUM_WrongAnswer(obj));

        // obj.GetComponent<Animator>().SetTrigger("BuzzerPress");
    }
    // public IEnumerator IENUM_Flash()
    // {
    //     yield return new WaitForSeconds(0.2f);
    //     G_FlashImage.SetActive(true);
    //     StartCoroutine(IENUM_SmallDelay());
    //     yield return new WaitForSeconds(1.1f);
    //     G_FlashImage.SetActive(false);

    // }

    public IEnumerator IENUM_SmallDelay()
    {
        yield return new WaitForSeconds(0.3f);
        // G_Camera_Turnoff.SetActive(false);



    }
    public IEnumerator IENUM_DummyDelay()
    {
        yield return new WaitForSeconds(2f);




    }

    //!end of region - gameplay logic
    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
    #endregion

    // #region QA

    // int GetOptionID(string selectedOption)
    // {
    //     for (int i = 0; i < options.Length; i++)
    //     {
    //         if (options[i].text == selectedOption)
    //         {
    //             return options[i].id;
    //         }
    //     }
    //     return -1;
    // }

    // bool CheckOptionIsAns(Component option)
    // {
    //     for (int i = 0; i < answers.Length; i++)
    //     {
    //         if (option.text == answers[i].text) { return true; }
    //     }
    //     return false;
    // }

    // void GetData(int questionIndex)
    // {
    //     Debug.Log(">>>>>" + questionIndex);
    //     question = QAManager.instance.GetQuestionAt(0, questionIndex);
    //     //if(question != null){
    //     options = QAManager.instance.GetOption(0, questionIndex);
    //     answers = QAManager.instance.GetAnswer(0, questionIndex);
    //     // }
    // }

    // void GetAdditionalData()
    // {
    //     additionalFields = QAManager.instance.GetAdditionalField(0);
    // }

    // void AssignData()
    // {
    //     // Custom code
    //     for (int i = 0; i < optionsGO.Length; i++)
    //     {
    //         optionsGO[i].GetComponent<Image>().sprite = options[i]._sprite;
    //         optionsGO[i].tag = "Untagged";
    //         Debug.Log(optionsGO[i].name, optionsGO[i]);
    //         if (CheckOptionIsAns(options[i]))
    //         {
    //             optionsGO[i].tag = "answer";
    //         }
    //     }
    //     // answerCount.text = "/"+answers.Length;
    // }

    // #endregion
}
