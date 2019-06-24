using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject RetreatPosition;
    public GameObject movePositions;
    public float ScareTime = 2f;

    private Player player;
    private GameMaster gameMaster;

    private bool retreated = true;
    public float currentRetreatTime = 0f;
    public float currentAttackTime = 0f;

    void Start ()
    {
        player = FindObjectOfType<Player>();
        gameMaster = FindObjectOfType<GameMaster>();

        if (RetreatPosition != null)
        {
            transform.position = RetreatPosition.transform.position;
        }
    }

	void Update ()
    {
        if (gameMaster != null
            && gameMaster.gameIsRunning)
        {
            if (retreated)
            {
                currentRetreatTime += Time.deltaTime;
            }
            else
            {
                currentAttackTime += Time.deltaTime;
            }
        }
		
	}

    private void LateUpdate()
    {
        if (player != null)
        {
            //transform.LookAt(player.transform);
        }
    }

    public void ScareAway()
    {
        if (RetreatPosition != null)
        {
            transform.position = RetreatPosition.transform.position;
            retreated = true;
            currentAttackTime = 0f;
        }
    }

    public void ChoosePositionAndMove()
    {
        if (movePositions != null)
        {
            int children = movePositions.transform.childCount;
            int index = Random.Range(0, children);

            Transform newPos = movePositions.transform.GetChild(index);

            if (newPos != null)
            {
                transform.position = newPos.position;
                Debug.Log("Moved to: " + newPos.gameObject.name);
                retreated = false;
                currentRetreatTime = 0f;
            }
        }
    }

    public void Attack()
    {
        Debug.Log("ATTACK");
        currentAttackTime = 0f;
        ScareAway();
    }

    public bool IsRetreated()
    {
        return retreated;
    }
}
