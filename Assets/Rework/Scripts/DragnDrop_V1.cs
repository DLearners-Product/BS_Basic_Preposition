using System.Collections;
using UnityEngine;
using TMPro;


public class DragnDrop_V1 : MonoBehaviour
{

    #region unity reference variables
    //==================================================================================================

    [Header("TEXTMESHPRO---------------------------------------------------------")]
    // [SerializeField] private TextMeshProUGUI TXT_Current;
    // [SerializeField] private TextMeshProUGUI TXT_Total;


    [Space(10)]
    [Header("GAME OBJECT---------------------------------------------------------")]
   // [SerializeField] private GameObject[] GA_DropObjects;
    [SerializeField] private GameObject[] GA_DragObjects;
   // [SerializeField] private GameObject G_TransparentScreen;
   // [SerializeField] private GameObject G_ActivityCompleted;


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




    #region gameplay logic
    //==================================================================================================

    // private void Start() => TXT_Total.text = GA_Objects.Length.ToString();


    void Start()
    {
        _currentIndex = 0;
      //  TXT_Total.text = GA_DragObjects.Length.ToString();
    }


    private IEnumerator IENUM_CorrectAnswer(string answer, Vector3 pos)
    {
        //*correct answer
        //TODO: additional functionality for correct answer





        //--------------------------------------------

        PlayParticles(pos);
        _currentIndex++;

        if (_currentIndex == GA_DragObjects.Length)
        {
            Invoke(nameof(ShowActivityCompleted), 2f);
        }

        yield return null;
    }


    public void CorrectAnswer(string answer, Vector3 pos)
    {
        StartCoroutine(IENUM_CorrectAnswer(answer, pos));
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
      //  G_ActivityCompleted.SetActive(true);
    }



    //!end of region - gameplay logic
    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
    #endregion


}