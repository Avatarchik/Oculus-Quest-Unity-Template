using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(QuestController))]
public class Grabber : Attachable
{
	public bool HideWhenGrab = false;
	
	private MeshRenderer[] meshRenderers;
	private QuestController controller;
	private Grabbable touchingGrabbable;
	private Grabbable grabbingGrabbable;

	private void Start() {
		Rigidbody rigidbody = GetComponent<Rigidbody>();
		rigidbody.useGravity = false;
		rigidbody.isKinematic = true;

		meshRenderers = GetComponentsInChildren<MeshRenderer>();

		controller = GetComponent<QuestController>();
	}

	private void Update() {
		bool gripButtonPressed;
		bool gripButtonReleased;
		controller.GetGripButton(out gripButtonPressed, out gripButtonReleased);

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
		if (grabbingGrabbable == null && grabbable.AttachTo(this)) {
			grabbingGrabbable = grabbable;

			if(HideWhenGrab) {
				ToggleAllRenderer(false);
			}
		}
	}

	private void Ungrab() {
		if (grabbingGrabbable != null) {
			grabbingGrabbable.ReleaseFrom(this, controller.Velocity, controller.AngularVelocity);
			grabbingGrabbable = null;

			if(HideWhenGrab) {
				ToggleAllRenderer(true);
			}
		}
	}

	private void ToggleAllRenderer(bool enabled) {
		foreach(MeshRenderer renderer in meshRenderers) {
			renderer.enabled = enabled;
		}
	}
}
