using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Grabbable : MonoBehaviour
{
	public delegate void GrabbableInteractionDelegate(Grabbable grabbable);
	public GrabbableInteractionDelegate OnBeingAttached;
	public GrabbableInteractionDelegate OnBeingReleased;

	private new Rigidbody rigidbody;

	private void Start() {
		rigidbody = GetComponent<Rigidbody>();

		// set rigidbody interpolation to solve jitter issue
		// https://forum.unity.com/threads/jittery-motion-on-oculus-quest.683704/#post-4576513
		rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
	}

	public bool IsBeingAttached { get; private set; } = false;

	public bool IsBeingAttachedBy(Attachable attachable) {
		return IsBeingAttached && transform.parent == attachable.AttachTransform;
	}

	public bool IsBeingAttachedByGrabber {
		get {
			return transform.parent != null && transform.parent.GetComponent<Grabber>() != null;
		}
	}
	
	public bool AttachTo(Attachable attachable) {
		if(!IsBeingAttached) {
			IsBeingAttached = true;

			attachable.SetChild(transform);

			rigidbody.isKinematic = true;
			rigidbody.velocity = Vector3.zero;
			rigidbody.angularVelocity = Vector3.zero;
			rigidbody.interpolation = RigidbodyInterpolation.None;

			OnBeingAttached?.Invoke(this);
			return true;
		}
		return false;
	}

	public bool ReleaseFrom(Grabber grabber, Vector3 velocity, Vector3 angularVelocity) {
		if (IsBeingAttachedBy(grabber)) {
			IsBeingAttached = false;

			transform.SetParent(null, true);
			
			rigidbody.isKinematic = false;
			rigidbody.velocity = velocity;
			rigidbody.angularVelocity = angularVelocity;
			rigidbody.interpolation = RigidbodyInterpolation.Interpolate;

			OnBeingReleased?.Invoke(this);
			return true;
		}
		return false;
	}
}
