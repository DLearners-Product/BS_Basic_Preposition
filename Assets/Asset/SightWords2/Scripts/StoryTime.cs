using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.EventSystems;


namespace SightWords2
{
    public class StoryTime : MonoBehaviour
    {
        #region unity reference variables

        [Header("AUDIO---------------------------------------------------------")]
        [SerializeField] private AudioClip AC_Passage;
        [SerializeField] private AudioClip[] ACA_Words;

        [Space(10)]
        [Header("ANIMATOR---------------------------------------------------------")]
        [SerializeField] private Animator ANIM_Info;

        [Space(10)]
        [Header("SPRITE---------------------------------------------------------")]
        [SerializeField] private Sprite SPR_Play;
        [SerializeField] private Sprite SPR_Pause;

        [Space(10)]
        [Header("IMAGE---------------------------------------------------------")]
        [SerializeField] private Image IMG_PlayPause;
        [SerializeField] private Image IMG_InfoBG;

        [Space(10)]
        [Header("GAME OBJECT---------------------------------------------------------")]
        [SerializeField] private GameObject G_Listening;
        [SerializeField] private GameObject G_Clicking;
        [SerializeField] private GameObject G_ActivityCompleted;

        [Space(10)]
        [Header("SCROLL RECT---------------------------------------------------------")]
        [SerializeField] private ScrollRect SRECT_Handle;

        [Space(10)]
        [Header("PARTICLE SYSTEM---------------------------------------------------------")]
        [SerializeField] private ParticleSystem Particles;

        [SerializeField] private ClickableText[] REF_ClickableTexts;


        [SerializeField] public List<string> answerss;

        private float scrollPosition = 1f; // Start at the top

        //!experimental

        public Transform parent;
        public TextMeshProUGUI wordPrefab;

        private int wordCount;
        private float padding = 10f;

        //!---------------

        private int I_CurrentIndex,
                    _ansCount;
        private bool B_IsPlaying;
        private bool B_IsFirstTimePlaying = true;
        private Coroutine currentAudioCoroutine;
        private bool isAudioPlaying = false;
        private List<ClickableText> clickableTexts = new List<ClickableText>();
        private bool isPlaying;

        private GameObject questionObject;
        public AudioSource source;

        public AudioClip ohoh;

        // #region QA

        // private int qIndex;
        // public GameObject questionGO;
        // public GameObject[] optionsGO;
        // public Dictionary<string, Component> additionalFields;
        // Component[] questions;
        // Component[] options;
        // Component[] answers;

        // #endregion



        //!end of region - unity reference variables
        //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
        #endregion

        void Start()
        {
            // #region DataSetter
            // //   Main_Blended.OBJ_main_blended.levelno = 11;
            // QAManager.instance.UpdateActivityQuestion();
            // qIndex = 0;
            // GetData(qIndex);
            // GetAdditionalData();
            // AssignData();
            // #endregion

            I_CurrentIndex = 0;
            _ansCount = 0;
        }


        public void PlayWordVO(int index)
        {
         //   AudioManager.Instance.PlayVoice(ACA_Words[index]);
            _ansCount++;

            if (_ansCount == 13)
            {
                Invoke(nameof(BUT_Exit), 1.5f);
            }

        }


        public void PlayParticles(Transform t)
        {
            Particles.transform.position = t.position;
            Particles.Play();

            TextMeshProUGUI clickedText = t.GetComponent<TextMeshProUGUI>();
            clickedText.GetComponent<TextMeshProUGUI>().text = "<color=#3C415A>" + clickedText.text + "</color>";
        }


        public void BUT_PlayPause()
        {
            if (B_IsFirstTimePlaying)
            {
                B_IsFirstTimePlaying = false;
                B_IsPlaying = true;
                IMG_PlayPause.sprite = SPR_Pause;
                // currentAudioCoroutine = StartCoroutine(PlayAudioCoroutine());
                // StartCoroutine(IENUM_PlayAudioCoroutine1());
              //  AudioManager.Instance.PlayVoice(AC_Passage);
            }
            else
            {
                if (B_IsPlaying)
                {
                    B_IsPlaying = false;
                    if (isAudioPlaying)
                    {
                     //   AudioManager.Instance.PauseVoice();
                    }
                    IMG_PlayPause.sprite = SPR_Play;
                }
                else
                {
                    B_IsPlaying = true;
                    if (isAudioPlaying)
                    {
                      //  AudioManager.Instance.ResumeVoice();
                    }
                    IMG_PlayPause.sprite = SPR_Pause;
                }
            }
        }

        public void StopAudio()
        {
            if (currentAudioCoroutine != null)
            {
                StopCoroutine(currentAudioCoroutine);
                currentAudioCoroutine = null;
            }
         //   AudioManager.Instance.StopVoice();
            B_IsPlaying = false;
            isAudioPlaying = false;
            IMG_PlayPause.sprite = SPR_Play;
        }

        public void BUT_Reset()
        {
          //  AudioManager.Instance.StopVoice();
            PlayVO();
        }


        public void BUT_PlayPauseSentence()
        {
            // if (AudioManager.Instance.AS_Voice.isPlaying)
            // {
            //     PauseVO();
            // }
            // else
            // {
            //     if (AudioManager.Instance.AS_Voice.time == 0 || AudioManager.Instance.AS_Voice.time == AudioManager.Instance.AS_Voice.clip.length)
            //     {
            //         PlayVO();
            //     }
            //     else
            //     {
            //         ResumeVO();
            //     }
            // }

        }


        // Method to play the audio
        public void PlayVO()
        {
            // AudioManager.Instance.SetVO(AC_Passage);
            // AudioManager.Instance.PlayVO();
            isPlaying = true;
            IMG_PlayPause.sprite = SPR_Pause;
            // Debug.Log("play");
        }


        // Method to pause the audio
        public void PauseVO()
        {

         //   AudioManager.Instance.AS_Voice.Pause();
            isPlaying = false;
            IMG_PlayPause.sprite = SPR_Play;
            // Debug.Log("pause");
        }

        // Method to resume the audio
        public void ResumeVO()
        {

          //  AudioManager.Instance.AS_Voice.UnPause();
            isPlaying = true;
            IMG_PlayPause.sprite = SPR_Pause;
            // Debug.Log("resume");
        }

        // Method to stop the audio
        public void StopVO()
        {

          //  AudioManager.Instance.AS_Voice.Stop();
            isPlaying = false;
            IMG_PlayPause.sprite = SPR_Play;
            // Debug.Log("stop");
        }

        public void ReportCorrectanswer(string obj)
        {
            questionObject = EventSystem.current.currentSelectedGameObject;
            Debug.Log(questionObject.name);
            // ScoreManager.instance.RightAnswer(qIndex, questionID: GetQuestionID(questionObject.name), answer: obj);
            // qIndex++;


            // // Proceed to the next question
            // if (qIndex < ACA_Words.Length)
            // {

            //     GetData(qIndex);
            // }
        }

        public void ReportWrongAnswer(string lastClickedWord)
        {
           // questionObject = EventSystem.current.currentSelectedGameObject;
          //  ScoreManager.instance.WrongAnswer(qIndex, questionID: questions[qIndex].id, answer: lastClickedWord);
        }








        public void ANIM_ButtonClick(Transform obj)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(obj.DOScale(new Vector3(1.2f, 1.2f, 1f), 0.25f));
            sequence.Append(obj.DOScale(new Vector3(0, 0, 0), 0.5f).SetDelay(0.25f));
            sequence.Play();
        }


        public void THI_CorrectAnswer(GameObject txt)
        {
            TextMeshProUGUI clickedText = txt.GetComponent<TextMeshProUGUI>();
            clickedText.GetComponent<TextMeshProUGUI>().text = "<color=#4473cb>" + clickedText.text + "</color>";
            Particles.transform.position = txt.transform.position;
            Particles.Play();
         //   AudioManager.Instance.PlayVoice(ACA_Words[int.Parse(txt.name)]);
        }

        public void THI_WrongAnswer()
        {
            // AudioManager.Instance.PlayWrong();
            source.clip = ohoh;
            source.Play();
        }

        public void PlayParticles(Vector3 pos)
        {
            pos = new Vector3(pos.x, pos.y, 0);
            Particles.transform.position = pos;
            Particles.Play();
        }

        public void BUT_Next()
        {
          //  AudioManager.Instance.PlayBubblyButtonClick();

            G_Listening.transform.DOMoveX(-17.8f, 1f);
            G_Clicking.transform.DOMoveX(0, 1f);
          //  StopVO();
            StopAllCoroutines();
        }

        public void BUT_Back()
        {
          //  AudioManager.Instance.PlayBubblyButtonClick();

            G_Listening.transform.DOMoveX(0, 1f);
            G_Clicking.transform.DOMoveX(17.8f, 1f);
        }

        public void BUT_Exit()
        {
           // AudioManager.Instance.PlayBubblyButtonClick();

            foreach (ClickableText script in REF_ClickableTexts)
            {
                script.enabled = false;
            }

            // AudioManager.OnAudioSettingsOpen -= DisableAllClickableTexts;
            // AudioManager.OnAudioSettingsClose -= EnableAllClickableTexts;

           // G_ActivityCompleted.SetActive(true);

        }

        public void BUT_InfoShow()
        {
          //  AudioManager.Instance.PlayBubblyButtonClick();

            ANIM_Info.SetTrigger("show");
            IMG_InfoBG.raycastTarget = true;
            DisableAllClickableTexts();  // Disable ClickableText here
        }

        public void BUT_InfoHide()
        {
          //  AudioManager.Instance.PlayBubblyButtonClick();

            ANIM_Info.SetTrigger("hide");
            IMG_InfoBG.raycastTarget = false;
            EnableAllClickableTexts();  // Enable ClickableText here
        }

        void OnEnable()
        {
            // AudioManager.OnAudioSettingsOpen += DisableAllClickableTexts;
            // AudioManager.OnAudioSettingsClose += EnableAllClickableTexts;
        }

        // void OnDisable()
        // {
        //     AudioManager.Instance.StopMusic();
        //     AudioManager.Instance.StopVoice();
        //     AudioManager.Instance.StopSFX();
        // }

        public void RegisterClickableText(ClickableText clickableText)
        {
            if (!clickableTexts.Contains(clickableText))
            {
                clickableTexts.Add(clickableText);
            }
        }

        public void UnregisterClickableText(ClickableText clickableText)
        {
            if (clickableTexts.Contains(clickableText))
            {
                clickableTexts.Remove(clickableText);
            }
        }

        private void DisableAllClickableTexts()
        {
            foreach (var clickableText in clickableTexts)
            {
                clickableText.SetClickable(false);
            }
        }

        private void EnableAllClickableTexts()
        {
            foreach (var clickableText in clickableTexts)
            {
                clickableText.SetClickable(true);
            }
        }
    //     #region QA

    //     int GetOptionID(string selectedOption)
    //     {
    //         for (int i = 0; i < options.Length; i++)
    //         {
    //             if (options[i].text == selectedOption)
    //             {
    //                 return options[i].id;
    //             }
    //         }
    //         return -1;
    //     }

    //     int GetQuestionID(string selectedQuestion)
    //     {
    //         for (int i = 0; i < questions.Length; i++)
    //         {
    //             if (questions[i].text == selectedQuestion)
    //             {
    //                 return questions[i].id;
    //             }
    //         }
    //         return -1;
    //     }


    //     bool CheckOptionIsAns(Component option)
    //     {
    //         for (int i = 0; i < answers.Length; i++)
    //         {
    //             if (option.text == answers[i].text) { return true; }
    //         }
    //         return false;
    //     }

    //     void GetData(int questionIndex)
    //     {
    //         Debug.Log(">>>>>" + questionIndex);
    //         questions = QAManager.instance.GetAllQuestions(0);
    //         //if(question != null){
    //         options = QAManager.instance.GetOption(0, questionIndex);
    //         answers = QAManager.instance.GetAnswer(0, questionIndex);
    //         // }
    //     }

    //     void GetAdditionalData()
    //     {
    //         additionalFields = QAManager.instance.GetAdditionalField(0);
    //     }

    //     void AssignData()
    //     {
    //         // Custom code
    //         for (int i = 0; i < optionsGO.Length; i++)
    //         {
    //             optionsGO[i].GetComponent<Image>().sprite = options[i]._sprite;
    //             optionsGO[i].tag = "Untagged";
    //             Debug.Log(optionsGO[i].name, optionsGO[i]);
    //             if (CheckOptionIsAns(options[i]))
    //             {
    //                 optionsGO[i].tag = "answer";
    //             }
    //         }
    //         // answerCount.text = "/"+answers.Length;
    //     }

    //     #endregion
    // }

}
}

