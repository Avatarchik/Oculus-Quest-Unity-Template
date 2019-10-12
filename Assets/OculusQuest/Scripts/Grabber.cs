using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(QuestController))]
[RequireComponent(typeof(Rigidbody))]
public class Grabber : MonoBehaviour
{
	public float GrabBegin = 0.55f;
	public float GrabEnd = 0.35f;

	new private Rigidbody rigidbody;
	private OVRInput.Controller controller;
	private Grabbable touchingGrabbable;
	private Grabbable grabbingGrabbable;

	private void Start() {
		controller = GetComponent<QuestController>().Controller;

		rigidbody = GetComponent<Rigidbody>();
		rigidbody.useGravity = false;
		rigidbody.isKinematic = true;
	}

	private void Update() {
		// grip button status
		float gripAxisValue = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, controller);
		bool gripButtonPressed = gripAxisValue > GrabBegin;
		bool gripButtonReleased = gripAxisValue < GrabEnd;

		if (gripButtonPressed && touchingGrabbable != null && grabbingGrabbable == null) {
			Grab(touchingGrabbable);
		}
		else if(gripButtonReleased && grabbingGrabbable != null) {
			Ungrab();
		}
	}

	private void OnTriggerEnter(Collider other) {
		Grabbable grabbable = other.GetComponent<Grabbable>();
		if (grabbable != null) {
			touchingGrabbable = grabbable;
		}
	}

	private void OnTriggerExit(Collider other) {
		Grabbable grabbable = other.GetComponent<Grabbable>();
		if(grabbable == touchingGrabbable) {
			touchingGrabbable = null;
		}
	}

	private void Grab(Grabbable grabbable) {
		grabbingGrabbable = grabbable;

		// connect the grabbable to the grabber via a fixed joint
		FixedJoint joint = grabbingGrabbable.GetComponent<FixedJoint>();
		if(joint == null) {
			joint = grabbingGrabbable.gameObject.AddComponent<FixedJoint>();
		}
		joint.connectedBody = rigidbody;
	}

	private void Ungrab() {
		// break fixed joint connection
		// must destroy the fixed joint component, otherwise setting grabbable's velocity & angularVelocity won't work
		FixedJoint joint = grabbingGrabbable.GetComponent<FixedJoint>();
		if (joint.connectedBody == this.rigidbody) {
			joint.connectedBody = null;
			Destroy(joint);

			Vector3 velocity = OVRInput.GetLocalControllerVelocity(controller);
			Vector3 angularVelocity = OVRInput.GetLocalControllerAngularVelocity(controller) * -1;
			Rigidbody rigidbody = grabbingGrabbable.GetComponent<Rigidbody>();
			rigidbody.velocity = velocity;
			rigidbody.angularVelocity = angularVelocity;
		}
		grabbingGrabbable = null;
	}
}
