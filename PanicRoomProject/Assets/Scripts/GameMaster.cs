using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    private Enemy enemy;
    private Player player;

    public float WaitTime = 2f;
    public float AttackTime = 10f;

    public bool gameIsRunning = true;

    void Start ()
    {
        enemy = FindObjectOfType<Enemy>();
        player = FindObjectOfType<Player>();
    }
	
	void Update ()
    {
		if (enemy != null
            && gameIsRunning)
        {
            if (enemy.IsRetreated())
            {
                if (enemy.currentRetreatTime > WaitTime)
                {
                    enemy.ChoosePositionAndMove();
                }
            }
            else
            {
                if (enemy.currentAttackTime > AttackTime)
                {
                    enemy.Attack();
                    gameIsRunning = false;
                }
            }
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameIsRunning = true;
        }
    }
}
