using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	public CameraController Camera;

	void Start()
    {

    }

    void Update()
    {
		Camera.OnUpdate();
	}
}
