using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject RetreatPosition;
    public GameObject movePositions;
    public float ScareTime = 2f;

	void Start ()
    {
		
	}

	void Update ()
    {
		
	}

    public void ScareAway()
    {
        if (RetreatPosition != null)
        {
            transform.position = RetreatPosition.transform.position;
        }
    }
}
