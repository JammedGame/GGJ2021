using System;
using UnityEngine;

public class Pirate : Barrel
{
	public int Index;
	public float MinDistanceForCollecting = 10f;

	public override void OnRelease(PlayerController ship)
	{
		var pivot = ship.PiratePivots[Index];
		transform.parent = pivot.transform;
		transform.localPosition = default;
		transform.localEulerAngles = default;
		Destroy(rigidBody);
	}

	public override void OnMouseClick(PlayerController ship)
	{
		if (Vector3.Distance(ship.transform.position, transform.position) < MinDistanceForCollecting)
		{
			StartDragging();
		}
	}
}