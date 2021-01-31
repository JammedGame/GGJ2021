using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public CameraSettings Settings;
	public Camera Camera;
	public PlayerController Player;

	void Start()
	{
		Camera.depthTextureMode = DepthTextureMode.Depth;
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