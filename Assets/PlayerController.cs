using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[Header("Settings")]
	public float Speed;
	public float TurnSpeed;
	public GameObject RotationPivot;

	[Header("State")]
	public float Orientation;
	public Vector3 Position;
	public Vector3 Direction => Quaternion.Euler(0, Orientation, 0) * Vector3.forward;

	public void Start()
	{
		Position = transform.position;
		Orientation = transform.eulerAngles.y;
	}

	public void FixedUpdate()
	{
		Position = transform.position;

		var forward = Mathf.Abs(Input.GetAxis("Vertical"));
		var sideways = Input.GetAxis("Horizontal");

		Orientation += sideways * Mathf.Sqrt(forward) * Time.deltaTime * TurnSpeed;
		Position += forward * Time.deltaTime * Speed * Direction;

		transform.position = Position;
		RotationPivot.transform.rotation = Quaternion.Euler(0, Orientation, 0);
	}
}
