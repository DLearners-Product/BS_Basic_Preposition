using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayandPause : MonoBehaviour
{
    public GameObject playAudio; // The GameObject with the AudioSource component
    public Sprite pauseSprite;   // The sprite to display when the audio is playing
    public Sprite playSprite;    // The sprite to display when the audio is paused

    private AudioSource audioSource;
    private Image buttonImage;
    private bool isPlaying = false;

    void Start()
    {
        audioSource = playAudio.GetComponent<AudioSource>();
        buttonImage = GetComponent<Image>();

        // Set the initial sprite
        buttonImage.sprite = playSprite;
    }

    public void ToggleAudio()
    {
        if (isPlaying)
        {
            audioSource.Pause();
            buttonImage.sprite = playSprite;
        }
        else
        {
            audioSource.Play();
            buttonImage.sprite = pauseSprite;
            StartCoroutine(CheckIfAudioFinished());
        }

        isPlaying = !isPlaying;
    }

    private IEnumerator CheckIfAudioFinished()
    {
        // Wait until the audio is no longer playing
        while (audioSource.isPlaying)
        {
            yield return null;
        }

        // Reset to play sprite after audio finishes
        buttonImage.sprite = playSprite;
        isPlaying = false;
    }

    public void stopVo()
    {
        audioSource.Stop();
    }
}
