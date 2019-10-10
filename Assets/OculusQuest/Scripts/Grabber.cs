using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(QuestController))]
[RequireComponent(typeof(FixedJoint))]
public class Grabber : MonoBehaviour
{
	public float GrabBegin = 0.55f;
	public float GrabEnd = 0.35f;

	private OVRInput.Controller controller;
	private FixedJoint joint;
	private Grabbable touchingGrabbable;
	private Grabbable grabbingGrabbable;

	private void Start() {
		controller = GetComponent<QuestController>().Controller;
		joint = GetComponent<FixedJoint>();
	}

	private void Update() {
		float gripAxisValue = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, controller);

		if (gripAxisValue > GrabBegin && touchingGrabbable != null && grabbingGrabbable == null) {
			Grab(touchingGrabbable);
		}
		else if(gripAxisValue < GrabEnd && grabbingGrabbable != null) {
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
		joint.connectedBody = grabbable.GetComponent<Rigidbody>();
	}

	private void Ungrab() {
		joint.connectedBody = null;

		Vector3 velocity = OVRInput.GetLocalControllerVelocity(controller);
		Vector3 angularVelocity = OVRInput.GetLocalControllerAngularVelocity(controller) * -1;
		grabbingGrabbable.Ungrab(velocity, angularVelocity);
		grabbingGrabbable = null;
	}
}
