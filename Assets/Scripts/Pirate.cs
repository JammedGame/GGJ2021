using System;
using System.Collections;
using UnityEngine;

public class Pirate : Barrel
{
	public int Index;
	public float MinDistanceForCollecting = 10f;
	public Animator Animator;
	public bool Waving;

	public override void OnRelease(PlayerController ship)
	{
		var pivot = ship.PiratePivots[Index];
		transform.parent = pivot.transform;
		transform.localPosition = default;
		transform.localEulerAngles = default;
		SetIdle();
		Destroy(rigidBody);
	}

	private void SetIdle() => Animator.SetInteger("State", 0);
	private void SetStruggle() => Animator.SetInteger("State", 1);
	private void SetWaving() => Animator.SetInteger("State", 2);
	private void SetSwiming() => Animator.SetInteger("State", 3);

	void Update()
	{
		if (IsBeingDragged)
			SetStruggle();
		else if (IsOnShip)
			SetIdle();
		else if (Waving)
			SetWaving();
		else if (transform.position.y < 0.2f)
			SetSwiming();
		else
			SetIdle();
	}

	public override void OnMouseClick(PlayerController ship)
	{
		if (Vector3.Distance(ship.transform.position, transform.position) < MinDistanceForCollecting)
		{
			StartDragging();
		}
		else
		{
			Waving = true;
			StartCoroutine(LookAtMe(ship));
		}
	}

	private IEnumerator LookAtMe(PlayerController ship)
	{
		while (!IsOnShip)
		{
			var lookVector = ship.Position - transform.position;
			lookVector.y = 0f;
			var targetRot = Quaternion.LookRotation(lookVector);
			transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, 180f * Time.deltaTime);

			yield return null;
		}
	}
}