using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cameras : MonoBehaviour{
		public Camera Camera1;
		public Camera Camera2;
		public Camera Camera3;
	void Start () {	
	Camera1.enabled = true;
    Camera2.enabled = false;
	Camera3.enabled = false;
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.C)) {
        Camera1.enabled = !Camera1.enabled;
        Camera2.enabled = !Camera2.enabled;
		Camera3.enabled = !Camera3.enabled;
    }
}}
