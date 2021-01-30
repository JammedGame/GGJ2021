using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	public CameraController Camera;
	public PlayerController Player;

	void Start()
    {

    }

    void Update()
	{
		Player.OnUpdate();
	}
}
