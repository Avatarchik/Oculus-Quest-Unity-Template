using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Grabbable : MonoBehaviour
{
	private void Start() {
		// set rigidbody interpolation to solve jitter issue
		// https://forum.unity.com/threads/jittery-motion-on-oculus-quest.683704/#post-4576513
		Rigidbody rigidbody = GetComponent<Rigidbody>();
		rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
	}

	public void Ungrab(Vector3 velocity, Vector3 angularVelocity) {
		Rigidbody rigidbody = GetComponent<Rigidbody>();
		rigidbody.velocity = velocity;
		rigidbody.angularVelocity = angularVelocity;
	}
}
