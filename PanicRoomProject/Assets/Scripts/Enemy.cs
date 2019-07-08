using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject RetreatPosition;
    public GameObject ScarePosition;
    public GameObject movePositions;
    public float ScareTime = 2f;
    public ActivateVibModules vib;

    private Player player;
    private GameMaster gameMaster;

    private bool retreated = true;
    private bool jumpScare = false;
    internal bool isLitOn = false;
    public float currentRetreatTime = 0f;
    public float currentAttackTime = 0f;

    public Animator anim;
    public Light enemyLight;

    void Start ()
    {
        player = FindObjectOfType<Player>();
        gameMaster = FindObjectOfType<GameMaster>();

        if (RetreatPosition != null)
        {
            transform.position = RetreatPosition.transform.position;
        }

        if (enemyLight != null)
        {
            enemyLight.intensity = 0;
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
                if (!isLitOn)
                {
                    currentAttackTime += Time.deltaTime;
                }
            }
        }
	}

    private void LateUpdate()
    {
        if (player != null)
        {
            transform.LookAt(player.transform);
        }
        if (jumpScare)
        {
            transform.position = ScarePosition.transform.position;
        }
    }

    public void ScareAway()
    {
        if (RetreatPosition != null)
        {
            transform.position = RetreatPosition.transform.position;
            retreated = true;
            currentAttackTime = 0f;
            vib.shiftVibrationsLegs(150);
            if (anim != null)
            {
                anim.SetTrigger("StopShake");
            }
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
        StartCoroutine(JumpScare());
        vib.shortFuzzLoopAll(200, 10, 4, 150);
        //ScareAway();
    }

    private IEnumerator JumpScare()
    {
        if (ScarePosition != null)
        {
            jumpScare = true;
            if (enemyLight != null)
            {
                enemyLight.intensity = 5;
            }
            if (anim != null)
            {
                anim.SetTrigger("Attack");
            }
            yield return new WaitForSeconds(1);
            if (enemyLight != null)
            {
                enemyLight.intensity = 0;
            }
            jumpScare = false;
            ScareAway();
        }
    }

    public bool IsRetreated()
    {
        return retreated;
    }

    public void StartShaking()
    {
        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        while (isLitOn)
        {
            transform.position += new Vector3(0.1f, 0, 0.1f);
            yield return new WaitForEndOfFrame();
            transform.position -= new Vector3(0.1f, 0, 0.1f);
            yield return new WaitForEndOfFrame();
            transform.position += new Vector3(-0.1f, 0, -0.1f);
            yield return new WaitForEndOfFrame();
            transform.position -= new Vector3(-0.1f, 0, -0.1f);
            yield return new WaitForEndOfFrame();

        }
    }
}
