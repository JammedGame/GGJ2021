using System;
using System.Linq;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public CameraSettings Settings;
	public Camera Camera;
	public PlayerController Player;
	public float ShipRaycastRadius = 3;
	public Vector3 ShipRaycastOffset;

	void Start()
	{
		Camera.depthTextureMode = DepthTextureMode.Depth;
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			OnMouseClick();
		}
		else if (Input.GetMouseButtonUp(0))
		{
			OnMouseRelease();
		}
		else if (Input.GetMouseButton(0))
		{
			Player.Glowing = MouseIsOverShip();
		}
		else
		{
			Player.Glowing = false;
		}
	}

	private void OnMouseRelease()
	{
		var isOverShip = MouseIsOverShip();
		foreach (var barrel in Barrel.AllBarrels.ToArray())
		{
			barrel.OnMouseRelease(isOverShip, Player);
		}
	}

	public bool MouseIsOverShip()
	{
		var ray = Camera.ScreenPointToRay(Input.mousePosition);
		var dist = Vector3.Cross(ray.direction, Player.Position + ShipRaycastOffset - ray.origin).magnitude;
		return dist < ShipRaycastRadius;
	}

	private void OnMouseClick()
	{
		var ray = Camera.ScreenPointToRay(Input.mousePosition);
		var minDistanceSqr = 2f * 2f;
		Barrel closestBarrel = null;

		foreach (var barrel in Barrel.AllBarrels)
		{
			var distSqr = Vector3.Cross(ray.direction, barrel.CenterPosition - ray.origin).sqrMagnitude;
			if (distSqr < minDistanceSqr)
			{
				minDistanceSqr = distSqr;
				closestBarrel = barrel;
			}
		}

		if (closestBarrel != null)
			closestBarrel.OnMouseClick(Player);
	}

	void LateUpdate()
	{
		// update pivot transform.
		transform.SetPositionAndRotation
		(
			new Vector3(Player.Position.x, 0, Player.Position.z),
			Quaternion.Euler(Settings.Pitch, Settings.Yaw, Settings.Roll)
		);

		// update local distance
		Camera.transform.localPosition = new Vector3(0, 0, -Settings.StartZoom);

		// update camera stuff.
		Camera.fieldOfView = Settings.FieldOfView;
	}
}