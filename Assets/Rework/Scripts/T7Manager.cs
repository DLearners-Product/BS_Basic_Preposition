
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class T7Manager : MonoBehaviour
{
    #region unity reference variables
    //==================================================================================================

    [Header("TEXTMESHPRO---------------------------------------------------------")]
    [SerializeField] private TextMeshProUGUI TXT_Current;
    [SerializeField] private TextMeshProUGUI TXT_Total;


    [Space(10)]
    [Header("GAME OBJECT---------------------------------------------------------")]
    // [SerializeField] private GameObject[] GA_DropObjects;
    [SerializeField] private GameObject[] GA_DragObjects;
    [SerializeField] private GameObject G_TransparentScreen;
    [SerializeField] private GameObject G_ActivityCompleted;
    public static T7Manager instance;
    int q1Index;


    [Space(10)]
    [Header("PARTICLES---------------------------------------------------------")]
    // [SerializeField] public ParticleSystem PS_Drag;
    // [SerializeField] private ParticleSystem PS_CorrectAnswer;



    //!end of region - unity reference variables
    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
    #endregion




    #region local variables
    //==================================================================================================

    private int _currentIndex;

    //!end of region - local variables
    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
    #endregion




    // #region gameplay logic
    // //==================================================================================================

    // // private void Start() => TXT_Total.text = GA_Objects.Length.ToString();
    // #region QA
    // private int qIndex;
    // public GameObject questionGO;
    // public GameObject[] optionsGO;
    // public bool isActivityCompleted = false;
    // public Dictionary<string, Component> additionalFields;
    // Component question;
    // Component[] options;
    // Component[] answers;
    // #endregion


    void Start()
    {
        if (instance == null)
            instance = this;
        Debug.Log("In Start", gameObject);
        //     #region DataSetter
        //    // Main_Blended.OBJ_main_blended.levelno = 8;
        //     QAManager.instance.UpdateActivityQuestion();
        //     qIndex = 0;
        //     GetData(qIndex);
        //     GetAdditionalData();
        //     AssignData();
        //     #endregion
        _currentIndex = 0;
        TXT_Total.text = GA_DragObjects.Length.ToString();
    }


    private IEnumerator IENUM_CorrectAnswer(string answer, Vector3 pos)
    {
        //*correct answer
        //TODO: additional functionality for correct answer

        //--------------------------------------------

        // PlayParticles(pos);
        yield return new WaitForSeconds(4.5f);

        // Ensure the first child of the current question is active before changing
        Transform firstChild = GA_DragObjects[_currentIndex].transform.GetChild(0);
        while (!firstChild.gameObject.activeSelf)
        {
            yield return null; // Wait until the first child is activated
        }

        _currentIndex++;
        if (_currentIndex > 0)
        {
            GA_DragObjects[_currentIndex - 1].SetActive(false);
        }

        if (_currentIndex == GA_DragObjects.Length)
        {
            Invoke(nameof(ShowActivityCompleted), 0f);
        }
        else
        {
            // Enabling current question
            GA_DragObjects[_currentIndex].SetActive(true);
            UpdateCounter();
        }

        yield return null;
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


    public void CorrectAnswer(string answer, Vector3 pos)
    {
        StartCoroutine(IENUM_CorrectAnswer(answer, pos));


    }

    public void ReportCorrectAnswer(string selectedObject)
    {
        // ScoreManager.instance.RightAnswer(q1Index, questionID: question.id, answerID: GetOptionID(selectedObject));
        // q1Index++;
        // if (qIndex < GA_DragObjects.Length - 1)
        // {
        //     qIndex++;
        //     GetData(qIndex);
        // }




    }

    public void ReportWrongAnswer(string selectedObject)
    {
        // ScoreManager.instance.WrongAnswer(q1Index, questionID: question.id, answerID: GetOptionID(selectedObject));
    }


    private void PlayParticles(Vector3 pos)
    {
        // PS_CorrectAnswer.transform.position = pos;
        // PS_CorrectAnswer.Play();
    }


    public void WrongAnswer(string answer)
    {

    }


    public void SetDragParticlesPosition(Transform parent)
    {
        // PS_Drag.transform.SetParent(parent);
        // PS_Drag.GetComponent<RectTransform>().localPosition = Vector3.zero;
    }


    private void ShowActivityCompleted()
    {
        G_ActivityCompleted.SetActive(true);
        //  BlendedOperations.instance.NotifyActivityCompleted();
    }



    // //!end of region - gameplay logic
    // //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
    // #endregion

    // #region QA

    // int GetOptionID(string selectedOption)
    // {
    //     for (int i = 0; i < options.Length; i++)
    //     {
    //         if (options[i].text == selectedOption)
    //         {
    //             Debug.Log(selectedOption);
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
    //     question = QAManager.instance.GetQuestionAt(0, questionIndex);
    //     // if(question != null){
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









