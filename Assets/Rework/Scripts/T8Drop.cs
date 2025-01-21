
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;

public class T8Drop :  MonoBehaviour, IDropHandler
{
    private T8Manager REF_DragnDrop_V1;
    private Vector3 initialPosition, currentPosition;
    private float elapsedTime, desiredDuration = 0.2f;

    public AudioSource source;
    public AudioClip correctAnswer;

    public AudioClip wrongAnswer;

    // public GameObject text;

    // public GameObject[] OneObj;


    //  public Text counter;

    // private int oneObjIndex = 0; // Track the current index for OneObj array
    // private int twoObjIndex = 0; // Track the current index for TwoObj array


    void Start()
    {
        REF_DragnDrop_V1 = FindObjectOfType<T8Manager>();
        initialPosition = transform.position;
    }


    public void OnDrop(PointerEventData eventData)
    {
        T8Drag drag = eventData.pointerDrag.GetComponent<T8Drag>();
       if (drag.name == gameObject.name)
        {
            drag.isDropped = true;
            StartCoroutine(IENUM_LerpTransform(drag.rectTransform, drag.rectTransform.anchoredPosition, GetComponent<RectTransform>().anchoredPosition));

            REF_DragnDrop_V1.CorrectAnswer(drag.name, transform.position);
            Debug.Log("correctAnswer");
        //    T8Manager.instance.ReportCorrectAnswer(drag.gameObject.transform.GetChild(0).name);
    //        Debug.Log(drag.gameObject.transform.GetChild(0).name);

            Image image = GetComponent<Image>();  // Assuming the object is a UI element with an Image component
            if (image != null)
            {
                Color color = image.color;
                color.a = 0f;  // Set alpha to 0 (fully transparent)
                image.color = color;
            }

            source.clip = correctAnswer;
            source.Play();




        }
        //!wrong answer
        else
        {
            REF_DragnDrop_V1.WrongAnswer(drag.name);
      //      T8Manager.instance.ReportWrongAnswer(drag.gameObject.transform.GetChild(0).name);

            source.clip = wrongAnswer;
            source.Play();


            //  StartCoroutine(WrongAnswerColor());
        }

    }


    IEnumerator IENUM_LerpTransform(RectTransform obj, Vector3 currentPosition, Vector3 targetPosition)
    {
        while (elapsedTime < desiredDuration)
        {
            elapsedTime += Time.deltaTime;
            float percentageComplete = elapsedTime / desiredDuration;

            //  obj.anchoredPosition = Vector3.Lerp(currentPosition, targetPosition, percentageComplete);
            yield return null;
        }

        //setting parent
        // obj.transform.SetParent(transform);
        // this.transform.GetChild(2).GetComponent<ParticleSystem>().Play();
        obj.gameObject.SetActive(false);
        this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        this.gameObject.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(1f);

        // obj.transform.localPosition = Vector2.zero;

        //resetting elapsed time back to zero
        elapsedTime = 0f;
    }
}

