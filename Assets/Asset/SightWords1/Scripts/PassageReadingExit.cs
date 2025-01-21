using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SightWords1
{


    public class PassageReadingExit : MonoBehaviour
    {

        [SerializeField] private ClickableText[] REF_ClickableTexts;


        void OnMouseEnter() => EnableDisableScripts(false);


        void OnMouseExit() => EnableDisableScripts(true);


        private void EnableDisableScripts(bool value)
        {
            foreach (ClickableText script in REF_ClickableTexts)
            {
                script.enabled = value;
            }
        }

    }
}