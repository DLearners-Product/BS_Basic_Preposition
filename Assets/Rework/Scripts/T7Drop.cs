using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;

public class T7Drop : MonoBehaviour, IDropHandler
{
    private T7Manager REF_DragnDrop_V1;
    private Vector3 initialPosition, currentPosition;
    private float elapsedTime, desiredDuration = 0.2f;

    public AudioSource source;
    public AudioClip correctAnswer;

    public AudioClip wrongAnswer;

    private Color initialColor; // To store the initial color of the image
    private Image targetImage; // Reference to the Image component

    // public GameObject text;

    // public GameObject[] OneObj;


    //  public Text counter;

    // private int oneObjIndex = 0; // Track the current index for OneObj array
    // private int twoObjIndex = 0; // Track the current index for TwoObj array


    void Start()
    {
        REF_DragnDrop_V1 = FindObjectOfType<T7Manager>();
        initialPosition = transform.position;
        targetImage = GetComponent<Image>();
        if (targetImage != null)
        {
            initialColor = targetImage.color;
        }
    }


    public void OnDrop(PointerEventData eventData)
    {
        Drag_V1 drag = eventData.pointerDrag.GetComponent<Drag_V1>();

        // Matching using the drag and drop GameObject name
        if (drag.name == gameObject.name) // Correct answer
        {
            drag.isDropped = true;
            StartCoroutine(IENUM_LerpTransform(drag.rectTransform, drag.rectTransform.anchoredPosition, GetComponent<RectTransform>().anchoredPosition));

            REF_DragnDrop_V1.CorrectAnswer(drag.name, transform.position);
            Debug.Log("correctAnswer");

            if (targetImage != null)
            {
                targetImage.color = Color.green; // Change color to green
            }

            source.clip = correctAnswer;
            source.Play();
        }
        else // Wrong answer
        {
            REF_DragnDrop_V1.WrongAnswer(drag.name);
            Debug.Log("wrongAnswer");

            if (targetImage != null)
            {
                StartCoroutine(WrongAnswerColorCoroutine());
            }

            source.clip = wrongAnswer;
            source.Play();
        }

    }
    private IEnumerator WrongAnswerColorCoroutine()
    {
        // Change the image color to red
        if (targetImage != null)
        {
            targetImage.color = Color.red;
        }

        // Wait for 1 second
        yield return new WaitForSeconds(1f);

        // Revert to the initial color
        if (targetImage != null)
        {
            targetImage.color = initialColor;
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
