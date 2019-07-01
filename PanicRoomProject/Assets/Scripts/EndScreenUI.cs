using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenUI : MonoBehaviour
{
    private GameResultSave save;
    private TextMeshProUGUI text;


    void Start ()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        save = FindObjectOfType<GameResultSave>();

        if (save != null && text != null)
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
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(0);
        }
    }
}
