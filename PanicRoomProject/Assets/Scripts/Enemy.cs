﻿using System.Collections;
using System.Collections.Generic;
using Tobii.Research.Unity;
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
    private AudioSource audioSource;
    private VRGazeTrail eyeData;

    public AudioClip JumpScareSound;

    private bool retreated = true;
    private bool jumpScare = false;
    internal bool isLitOn = false;
    public float currentRetreatTime = 0f;
    public float currentAttackTime = 0f;

    public Animator anim;
    public Light enemyLight;

    public AudioClip HurtByLight;
    public AudioClip DisappearSound;

    private bool alreadyPlayedInLocation = false;
    private MovePositions currentPos;

    void Start ()
    {
        player = FindObjectOfType<Player>();
        gameMaster = FindObjectOfType<GameMaster>();
        audioSource = FindObjectOfType<AudioSource>();
        eyeData = FindObjectOfType<VRGazeTrail>();

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

                    if (gameMaster != null
                        && currentAttackTime >= gameMaster.AttackTime / 2
                        && !alreadyPlayedInLocation
                        && currentPos != null)
                    {
                        AudioClip sound = currentPos.GetRandomSound();
                        if (sound != null
                            && audioSource != null)
                        {
                            audioSource.PlayOneShot(sound);
                            alreadyPlayedInLocation = true;
                        }
                    }
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
            alreadyPlayedInLocation = false;
            currentPos = null;
            if (audioSource != null)
            {
                audioSource.PlayOneShot(DisappearSound);
            }
        }
    }

    public void ChoosePositionAndMove()
    {
        if (movePositions != null)
        {
            List<GameObject> possiblePositions = new List<GameObject>();
            foreach (Transform child in movePositions.transform)
            {
                possiblePositions.Add(child.gameObject);
            }

            if (eyeData != null)
            {
                for (int i = possiblePositions.Count - 1; i >= 0; i--)
                {
                    GameObject posCandidate = possiblePositions[i];
                    if (posCandidate.transform == eyeData.GetLatestObjectHit())
                    {
                        possiblePositions.Remove(posCandidate);
                    }
                }
            }
       
            int children = possiblePositions.Count;
            int index = Random.Range(0, children);

            GameObject newPos = possiblePositions[index];

            if (newPos != null)
            {
                transform.position = newPos.transform.position;
                currentPos = newPos.GetComponent<MovePositions>();
                Debug.Log("Moved to: " + newPos.gameObject.name);
                retreated = false;
                currentRetreatTime = 0f;
            }
        }
    }

    public void Attack()
    {
        if (audioSource != null)
        {
            audioSource.PlayOneShot(JumpScareSound);
        }
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
        if (audioSource != null)
        {
            audioSource.PlayOneShot(HurtByLight, 0.3f);
        }
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
