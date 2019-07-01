using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    private Enemy enemy;
    private Player player;
    private Flashlight flashlight;

    public float WaitTime = 2f;
    public float AttackTime = 10f;

    public bool gameIsRunning = true;

    void Start ()
    {
        enemy = FindObjectOfType<Enemy>();
        player = FindObjectOfType<Player>();
        flashlight = FindObjectOfType<Flashlight>();
    }
	
	void Update ()
    {
		if (enemy != null
            && gameIsRunning)
        {
            if (enemy.IsRetreated())
            {
               // Debug.Log("RetreatTime: " + enemy.currentRetreatTime+ " of: " + WaitTime);
                if (enemy.currentRetreatTime > WaitTime)
                {
                    enemy.ChoosePositionAndMove();
                }
            }
            else
            {
               // Debug.Log("AttackTime: " + enemy.currentAttackTime + " of: " + AttackTime);
                if (enemy.currentAttackTime > AttackTime)
                {
                    enemy.Attack();
                    gameIsRunning = false;
                    //flashlight.LampOff();
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
}
