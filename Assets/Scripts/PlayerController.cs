using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[Header("Settings")]
	public float Speed;
	public float TurnSpeed;
	public GameObject RotationPivot;
	public Renderer[] HighlightRenderers;
	public GameObject[] PiratePivots;
	public Transform BarrelPivot1;
	public Transform BarrelPivot2;

	[Header("State")]
	public float Orientation;
	public Vector3 Position;
	public Vector3 Direction => Quaternion.Euler(0, Orientation, 0) * Vector3.forward;

	private bool glowing;
	public bool Glowing
	{
		get => glowing;
		set
		{
			if (glowing != value)
			{
				glowing = value;
				if (glowing)
				{
					var glowBlock = new MaterialPropertyBlock();
					var glowColor = Color.white;
					glowColor.r = glowColor.g = glowColor.b = 0.25f;
					glowBlock.SetColor("_EmissionColor", glowColor);
					foreach (var r in HighlightRenderers)
						r.SetPropertyBlock(glowBlock);
				}
				else
				{
					foreach (var r in HighlightRenderers)
						r.SetPropertyBlock(null);
				}
			}
		}
	}

	public int BarrelCount { get; internal set; }

	public void Start()
	{
		Position = transform.position;
		Orientation = transform.eulerAngles.y;
	}

	public void FixedUpdate()
	{
		Position = transform.position;

		var forward = Input.GetAxis("Vertical");
		var sideways = Input.GetAxis("Horizontal");

		Orientation += sideways * Mathf.Sign(forward) * Mathf.Sqrt(Mathf.Abs(forward)) * Time.deltaTime * TurnSpeed;
		Position += forward * Time.deltaTime * Speed * Direction;

		transform.position = Position;
		RotationPivot.transform.rotation = Quaternion.Euler(0, Orientation, 0);
	}
}
