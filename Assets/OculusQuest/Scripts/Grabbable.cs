using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Grabbable : MonoBehaviour
{
	public void Ungrab(Vector3 velocity, Vector3 angularVelocity) {
		Rigidbody rigidbody = GetComponent<Rigidbody>();
		rigidbody.velocity = velocity;
		rigidbody.angularVelocity = angularVelocity;
	}
}
