using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenUI : MonoBehaviour
{
    private GameResultSave save;
    private TextMeshProUGUI text;
    private AudioSource audioSource;

    public AudioClip WinClip;
    public AudioClip LoseClip;


    void Start ()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        save = FindObjectOfType<GameResultSave>();
        audioSource = GetComponent<AudioSource>();

        if (save != null)
        {
            if (text != null)
            {
                if (save.gameWon)
                {
                    text.text = "You survived!";
                }
                else
                {
                    text.text = "You are dead!";
                }
            }

            if (audioSource != null)
            {
                if (save.gameWon)
                {
                    audioSource.PlayOneShot(WinClip);
                }
                else
                {
                    audioSource.PlayOneShot(LoseClip);
                } 
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(1);
        }
    }
}
