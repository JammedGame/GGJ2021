using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public CameraSettings Settings;
	public Camera Camera;
	public PlayerController Player;

	[Header("Panning")]
	public float MinX;
	public float MaxX;
	public float MinZ;
	public float MaxZ;

	[Header("Current")]
	public float Distance;
	public Vector3 Position;

	void Start()
	{
		Distance = Settings.StartZoom;
	}

	void LateUpdate()
	{
		// update position.
		var offset = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * Settings.KeyboardPanSpeed * Time.deltaTime;
		offset = Quaternion.Euler(0, Settings.Yaw, 0) * offset;
		Position += offset;

		// update zoom
		Distance -= Input.GetAxis("Mouse ScrollWheel") * Settings.ScrollWheelZoomSpeed * Time.deltaTime;

		// validate new value
		ClampValues();

		// update transform.
		var pos = new Vector3(Player.Position.x, 0, Player.Position.z);
		transform.SetPositionAndRotation(pos, Quaternion.Euler(Settings.Pitch, Settings.Yaw, Settings.Roll));
		Camera.transform.localPosition = new Vector3(0, 0, -Distance);
		Camera.fieldOfView = Settings.FieldOfView;
	}

	private void ClampValues()
	{
		Distance = Mathf.Clamp(Distance, Settings.MinZoom, Settings.MaxZoom);
		Position.x = Mathf.Clamp(Position.x, MinX, MaxX);
		Position.z = Mathf.Clamp(Position.z, MinZ, MaxZ);
	}

	public void SetCentralPosition()
	{
		Position = new Vector3(MinX + MaxX, 0, MinZ + MaxZ) / 2f;
	}
}