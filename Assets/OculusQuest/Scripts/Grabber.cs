using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(QuestController))]
[RequireComponent(typeof(Rigidbody))]
public class Grabber : MonoBehaviour
{
	public float GrabBegin = 0.55f;
	public float GrabEnd = 0.35f;

	public Transform SnapTransform;

	private new Rigidbody rigidbody;
	private OVRInput.Controller controller;
	private Grabbable touchingGrabbable;
	private Grabbable grabbingGrabbable;

	private void Start() {
		rigidbody = GetComponent<Rigidbody>();
		rigidbody.useGravity = false;
		rigidbody.isKinematic = true;

		controller = GetComponent<QuestController>().Controller;
	}

	private void Update() {
		// grip button status
		float gripAxisValue = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, controller);
		bool gripButtonPressed = gripAxisValue > GrabBegin;
		bool gripButtonReleased = gripAxisValue < GrabEnd;

		if (gripButtonPressed && touchingGrabbable != null) {
			Grab(touchingGrabbable);
		}
		else if(gripButtonReleased) {
			Ungrab();
		}
	}

	private void OnTriggerEnter(Collider other) {
		Grabbable grabbable = other.GetComponentInParent<Grabbable>();
		if (grabbable != null) {
			touchingGrabbable = grabbable;
		}
	}

	private void OnTriggerExit(Collider other) {
		Grabbable grabbable = other.GetComponentInParent<Grabbable>();
		if(grabbable == touchingGrabbable) {
			touchingGrabbable = null;
		}
	}

	private void Grab(Grabbable grabbable) {
		if (grabbingGrabbable == null && grabbable.GrabBy(this)) {
			grabbingGrabbable = grabbable;
		}
	}

	private void Ungrab() {
		if (grabbingGrabbable != null) {
			grabbingGrabbable.ReleaseFrom(this);
			grabbingGrabbable = null;
		}
	}

	public Vector3 Velocity {
		get {
			return OVRInput.GetLocalControllerVelocity(controller);
		}
	}

	public Vector3 AngularVelocity {
		get {
			return OVRInput.GetLocalControllerAngularVelocity(controller) * -1;
		}
	}
}
