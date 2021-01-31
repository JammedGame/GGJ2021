using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Barrel : MonoBehaviour
{
	Rigidbody rigidBody => GetComponentInChildren<Rigidbody>();

	public AnimationCurve forceOverY;
	public AnimationCurve dragOverY;

	Plane? holdPlane;

	void OnMouseDown()
	{
		if (transform.position.y < 1f)
		{
			var force = new Vector3(Random.Range(-3f, 3f), 15f, Random.Range(-3f, 3f));
			rigidBody.AddForce(force, ForceMode.Impulse);
		}
		else if (holdPlane == null)
		{
			var position = transform.position;
			position.y = Mathf.Max(10f, position.y);
			holdPlane = new Plane(Vector3.up, position);
		}
	}

	void FixedUpdate()
	{
		if (!Input.GetMouseButton(0))
		{
			holdPlane = default;
		}

		if (holdPlane is Plane planeForDragging)
		{
			var screenRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (planeForDragging.Raycast(screenRay, out float dist))
			{
				transform.position = screenRay.GetPoint(dist);
				rigidBody.useGravity = false;
				rigidBody.velocity = default;
			}
		}

		rigidBody.useGravity = true;
		var pos = transform.position;
		if (pos.y < 0f)
		{
			var drag = dragOverY.Evaluate(-pos.y);
			var force = forceOverY.Evaluate(-pos.y);
            rigidBody.velocity *= drag;
			rigidBody.AddForce(Vector3.up * Time.fixedDeltaTime * force, ForceMode.VelocityChange);
        }
	}
}
