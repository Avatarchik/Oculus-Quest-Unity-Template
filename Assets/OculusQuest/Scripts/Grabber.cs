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

	new private Rigidbody rigidbody;
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

		if (gripButtonPressed && touchingGrabbable != null && grabbingGrabbable == null) {
			Grab(touchingGrabbable);
		}
		else if(gripButtonReleased && grabbingGrabbable != null) {
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
		grabbingGrabbable = grabbable;

		Rigidbody rigidbody = grabbable.GetComponent<Rigidbody>();
		rigidbody.isKinematic = false;
		rigidbody.velocity = Vector3.zero;
		rigidbody.angularVelocity = Vector3.zero;

		if (SnapTransform != null) {
			grabbable.transform.SetParent(SnapTransform, true);
			grabbable.transform.localPosition = Vector3.zero;
			grabbable.transform.localEulerAngles = Vector3.zero;
		}
		else {
			grabbable.transform.SetParent(transform, true);
		}
	}

	private void Ungrab() {
		if(grabbingGrabbable.transform.parent == transform || grabbingGrabbable.transform.parent == SnapTransform) {
			// detach the grabbable from grabber
			grabbingGrabbable.transform.SetParent(null, true);

			// apply controller's velocity to the detached grabbable to support "throw away"
			Vector3 velocity = OVRInput.GetLocalControllerVelocity(controller);
			Vector3 angularVelocity = OVRInput.GetLocalControllerAngularVelocity(controller) * -1;
			Rigidbody rigidbody = grabbingGrabbable.GetComponent<Rigidbody>();
			rigidbody.isKinematic = false;
			rigidbody.velocity = velocity;
			rigidbody.angularVelocity = angularVelocity;
		}
		grabbingGrabbable = null;
	}
}
