using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class T7RaycastTaragetOff : MonoBehaviour
{
   public GameObject[] targets;

   void Start()
   {
        foreach(var Targets in targets)
        {
            Targets.gameObject.GetComponent<Image>().raycastTarget = false;
        }
   }
}
