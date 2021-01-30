using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class OnClickLol : MonoBehaviour
{
	Rigidbody rigidBody => GetComponentInChildren<Rigidbody>();

	public AnimationCurve forceOverY;
	public AnimationCurve dragOverY;

	void OnMouseDown()
	{
		rigidBody.AddForce(Vector3.up * 10f, ForceMode.Impulse);
	}

	void FixedUpdate()
	{
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
