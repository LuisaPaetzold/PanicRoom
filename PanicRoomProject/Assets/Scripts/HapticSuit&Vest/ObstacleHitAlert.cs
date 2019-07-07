using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleHitAlert : MonoBehaviour
{

	[Header("Attach main vibration script")]
	public ActivateVibModules vib;
	public enum VibType {LeftArm=0, RightArm, LeftLeg, RightLeg };
	public VibType vibType = VibType.LeftArm;

	// Tag objects to collide with as Obstacle
	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "Obstacle")
		{
			vib.hitAlert((byte)vibType);
		}
	}
}
