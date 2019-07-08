using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    private Enemy enemy;
    private Player player;
    private Flashlight flashlight;
    private GameResultSave save;
    private ArduinoData aData;

    public float WaitTimeMin = 2f;
    public float WaitTimeMax = 5f;
    public float AttackTimeMin = 10f;
    public float AttackTimeMax = 20f;

    private float AttackTime = 5f;
    private float WaitTime = 20f;

    public float SecondsToWinGame = 180f;

    public bool gameIsRunning = true;

    void Start ()
    {
        enemy = FindObjectOfType<Enemy>();
        player = FindObjectOfType<Player>();
        flashlight = FindObjectOfType<Flashlight>();
        save = FindObjectOfType<GameResultSave>();
        aData = FindObjectOfType<ArduinoData>();

        if (save != null)
        {
            save.gameWon = false;
        }
    }
	
	void Update ()
    {
        if (Time.timeSinceLevelLoad > SecondsToWinGame)
        {
            WinGame();
        }
        CalcNewTimes();

		if (enemy != null
            && gameIsRunning)
        {
            if (enemy.IsRetreated())
            {
                //Debug.Log("RetreatTime: " + enemy.currentRetreatTime+ " of: " + WaitTime);
                if (enemy.currentRetreatTime > WaitTime)
                {
                    enemy.ChoosePositionAndMove();
                }
            }
            else
            {
                //Debug.Log("AttackTime: " + enemy.currentAttackTime + " of: " + AttackTime);
                if (enemy.currentAttackTime > AttackTime)
                {
                    LoseGame();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            enemy.Attack();
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameIsRunning = true;
        }
    }

    public void WinGame()
    {
        if (save != null)
        {
            save.gameWon = true;
        }
        StartCoroutine(LoadLevelWithDelay(0));
    }

    public void LoseGame()
    {
        enemy.Attack();
        gameIsRunning = false;
        StartCoroutine(LoadLevelWithDelay(1));
    }

    public void CalcNewTimes()
    {
        if (aData != null)
        {
            WaitTime = Mathf.Lerp(WaitTimeMin, WaitTimeMax, aData.difficultyMultiplier);
            AttackTime = Mathf.Lerp(AttackTimeMin, AttackTimeMax, aData.difficultyMultiplier);
        }
    }

    private IEnumerator LoadLevelWithDelay(float delay)
    {
        aData.ClosePort();
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(2);
    }
}
