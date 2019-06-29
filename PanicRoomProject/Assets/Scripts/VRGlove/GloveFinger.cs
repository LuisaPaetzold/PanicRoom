using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloveFinger : MonoBehaviour {

	private Glove hand;
	private float degree;
	private int number;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setDegree(float value){
		degree = value;
	}

	public void setHand(Glove glo, int num){
		hand = glo;
		number = num;
	}

	void OnCollisionEnter(Collision collision)
	{
		hand.stopFinger (number);
	}
}
