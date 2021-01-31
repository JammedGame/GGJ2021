using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Barrel : MonoBehaviour
{
	const float minimumHoldingHeight = 10f;

	public Rigidbody rigidBody;
	public AnimationCurve forceOverY;
	public AnimationCurve dragOverY;

	Plane? currentDraggingPlane;

	void OnMouseDown()
	{
		// if in water, add force up
		if (transform.position.y < 1f)
		{
			var force = new Vector3(Random.Range(-3f, 3f), 15f, Random.Range(-3f, 3f));
			rigidBody.AddForce(force, ForceMode.Impulse);
			rigidBody.angularVelocity = new Vector3(0, Random.Range(1f, 2f), 0f);
		}
		// if not in water and not dragging, start dragging
		else if (currentDraggingPlane == null)
		{
			var position = transform.position;
			position.y = minimumHoldingHeight;
			currentDraggingPlane = new Plane(Vector3.up, position);
		}
	}

	void FixedUpdate()
	{
		if (currentDraggingPlane != null)
		{
			// if mouse is up, stop dragging
			if (!Input.GetMouseButton(0))
			{
				currentDraggingPlane = default;
				rigidBody.useGravity = true;
			}
			else
			{
				// we are dragging the barrel - find position and freeze rigidbody
				var screenRay = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (currentDraggingPlane.Value.Raycast(screenRay, out float dist))
				{
					transform.position = screenRay.GetPoint(dist);
					rigidBody.useGravity = false;
					rigidBody.velocity = default;
					rigidBody.angularVelocity = default;
				}
			}
		}

		// code that adds force for water interaction.
		var pos = transform.position;
		if (pos.y < 0f)
		{
			var drag = dragOverY.Evaluate(-pos.y);
			var force = new Vector3(0, forceOverY.Evaluate(-pos.y) - Physics.gravity.y, 0f);
            rigidBody.velocity *= drag;
			rigidBody.AddForce(force * Time.fixedDeltaTime, ForceMode.VelocityChange);
			rigidBody.angularDrag = 1f;
		}
		else
		{
			// no drag while we are in air
			rigidBody.angularDrag = 0f;
		}
	}
}
