using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Barrel : MonoBehaviour
{
	public static readonly HashSet<Barrel> AllBarrels = new HashSet<Barrel>();

	const float minimumHoldingHeight = 10f;

	public Rigidbody rigidBody;
	public float floatingCenter;
	public AnimationCurve forceOverY;
	public AnimationCurve dragOverY;
	public float AngularDragAboveWater = 0f;
	public float AngularDragInWater = 1f;
	public Vector3 DraggingOffset;

	Plane? currentDraggingPlane;
	public Vector3 CenterPosition => transform.position + Vector3.up * floatingCenter;
	public bool IsBeingDragged => currentDraggingPlane != null;
	public bool IsOnShip => rigidBody == null;

	void OnEnable()
	{
		AllBarrels.Add(this);
	}

	void OnDisable()
	{
		AllBarrels.Remove(this);
	}

	public virtual void OnMouseClick(PlayerController ship)
	{
		// if in water, add force up
		if (transform.position.y < 1f)
		{
			var force = new Vector3(Random.Range(-3f, 3f), 15f, Random.Range(-3f, 3f));
			rigidBody.AddForce(force, ForceMode.Impulse);
			rigidBody.angularVelocity = new Vector3(0, Random.Range(1f, 2f), 0f);
		}
		// if not in water and not dragging, start dragging
		else
		{
			StartDragging();
		}
	}

	public void StartDragging()
	{
		if (currentDraggingPlane == null)
		{
			var position = transform.position;
			position.y = minimumHoldingHeight;
			currentDraggingPlane = new Plane(Vector3.up, position);
		}
	}

	public void OnMouseRelease(bool isOverShip, PlayerController ship)
	{
		if (currentDraggingPlane != null && rigidBody)
		{
			currentDraggingPlane = default;
			rigidBody.useGravity = true;

			if (isOverShip)
			{
				OnRelease(ship);
			}
		}
	}

	public virtual void OnRelease(PlayerController ship)
	{
		var pivot = ship.BarrelCount % 2 == 0 ? ship.BarrelPivot1 : ship.BarrelPivot2;
		transform.parent = pivot.transform;
		transform.localPosition = Vector3.up * (ship.BarrelCount / 2) * 1.5f;
		transform.localEulerAngles = default;
		Destroy(rigidBody);
		ship.BarrelCount++;
	}

	void FixedUpdate()
	{
		if (!rigidBody)
			return;

		if (currentDraggingPlane != null)
		{
			// we are dragging the barrel - find position and freeze rigidbody
			var screenRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (currentDraggingPlane.Value.Raycast(screenRay, out float dist))
			{
				transform.position = screenRay.GetPoint(dist) + DraggingOffset;
				rigidBody.useGravity = false;
				rigidBody.velocity = default;
				rigidBody.angularVelocity = default;
			}
		}

		// code that adds force for water interaction.
		var submersion = - floatingCenter - transform.position.y;
		if (submersion > 0f)
		{
			var drag = dragOverY.Evaluate(submersion);
			var force = new Vector3(0, forceOverY.Evaluate(submersion) - Physics.gravity.y, 0f);
            rigidBody.velocity *= drag;
			rigidBody.AddForce(force * Time.fixedDeltaTime, ForceMode.VelocityChange);
			rigidBody.angularDrag = AngularDragInWater;
		}
		else
		{
			// no drag while we are in air
			rigidBody.angularDrag = AngularDragAboveWater;
		}
	}
}
