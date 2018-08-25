﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour {

	[SerializeField]
	private Transform target; // игрок
	
	// Update is called once per frame
	void Update () {
		if (target.position.y > transform.position.y) {
			transform.position = new Vector3(transform.position.x, target.position.y, transform.position.z);
		}
	}
}
