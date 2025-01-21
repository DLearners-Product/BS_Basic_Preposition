using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class T8Drag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    [HideInInspector] public RectTransform rectTransform;
    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public bool isDropped;

    private Image image;
    private Vector3 initialPosition, currentPosition;
    private float elapsedTime, desiredDuration = 0.25f;
    private Canvas canvas;
    private Transform parent;
    private DragnDrop_V1 REF_DragnDrop_V1;


    void Start()
    {
        REF_DragnDrop_V1 = FindObjectOfType<DragnDrop_V1>();
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        canvas = FindObjectOfType<Canvas>();
        initialPosition = transform.position;
        isDropped = false;
        parent = transform.parent;
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        // REF_DragnDrop_V1.SetDragParticlesPosition(transform);
        parentAfterDrag = transform.parent;
        image.raycastTarget = false;

        //setting Drag item's parent to canvas and as last sibling, so this will appear on top of all objects during the drag operation
        //transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        this.gameObject.transform.localScale = new Vector3(1.1f, 1.1f, 0);

    }


    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        //  this.gameObject.transform.localScale = new Vector3(1.1f,1.1f,0);
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDropped)
        {
            //getting the latest position of Drag object
            currentPosition = transform.position;

            //setting the Drag item's parent and position to its initial state
            transform.SetParent(parent);
           //  StartCoroutine(WrongAnswerColor());
            StartCoroutine(IENUM_LerpToInitialPosition());
            this.gameObject.transform.localScale = new Vector3(1f, 1f, 0);
        }

        image.raycastTarget = true;
    }


    IEnumerator IENUM_LerpToInitialPosition()
    {
        while (elapsedTime < desiredDuration)
        {
            elapsedTime += Time.deltaTime;
            float percentageComplete = elapsedTime / desiredDuration;

            transform.position = Vector3.Lerp(currentPosition, initialPosition, percentageComplete);


            yield return null;
          
        }
         ;

        //resetting elapsed time back to zero
        elapsedTime = 0f;

    }
    // IEnumerator WrongAnswerColor()
    // {
    //     // Get the Text component of the first child
    //     Text textComponent = this.gameObject.transform.GetChild(0).GetComponent<Text>();

    //     // Check if the Text component is found
    //     if (textComponent != null)
    //     {
    //         // Store the original color of the text
    //         Color originalColor = textComponent.color;

    //         // Change the text color to red
    //         textComponent.color = Color.red;

    //         // Wait for 1 second
    //         yield return new WaitForSeconds(1f);

    //         // Revert back to the original color
    //         textComponent.color = originalColor;
    //     }
    //     else
    //     {
    //         Debug.LogWarning("Text component not found!");
    //     }

    // }


}