using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Grabbable : MonoBehaviour
{
	public delegate void GrabbableInteractionDelegate(Grabbable grabbable);
	public GrabbableInteractionDelegate OnBeingGrabbed;
	public GrabbableInteractionDelegate OnBeingSnapped;
	public GrabbableInteractionDelegate OnBeingReleased;

	private new Rigidbody rigidbody;

	private void Start() {
		rigidbody = GetComponent<Rigidbody>();

		// set rigidbody interpolation to solve jitter issue
		// https://forum.unity.com/threads/jittery-motion-on-oculus-quest.683704/#post-4576513
		rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
	}

	private void AttachTo(Transform parent, bool resetLocalTransform = false) {
		rigidbody.isKinematic = true;
		rigidbody.velocity = Vector3.zero;
		rigidbody.angularVelocity = Vector3.zero;

		transform.SetParent(parent, true);
		if(resetLocalTransform) {
			transform.localPosition = Vector3.zero;
			transform.localEulerAngles = Vector3.zero;
		}
	}

	public bool IsFreeForInteraction {
		get {
			return !IsBeingGrabbed && !IsBeingSnapped;
		}
	}
	public bool IsBeingGrabbed { get; private set; } = false;
	public bool IsBeingSnapped { get; private set; } = false;

	public bool IsBeingGrabbedBy(Grabber grabber) {
		return IsBeingGrabbed && (transform.parent == grabber.SnapTransform || transform.parent == grabber.transform);
	}

	public bool IsBeingSnappedTo(SnapDropZone snapDropZone) {
		return IsBeingSnapped && transform.parent == snapDropZone.transform;
	}

	public bool GrabBy(Grabber grabber) {
		if (IsFreeForInteraction) {
			IsBeingGrabbed = true;
			if(grabber.SnapTransform != null) {
				AttachTo(grabber.SnapTransform, true);
			}
			else {
				AttachTo(grabber.transform);
			}
			OnBeingGrabbed?.Invoke(this);
			return true;
		}
		return false;
	}

	public bool SnapTo(SnapDropZone snapDropZone) {
		if (IsFreeForInteraction) {
			IsBeingSnapped = true;
			AttachTo(snapDropZone.transform, true);
			OnBeingSnapped?.Invoke(this);
			return true;
		}
		return false;
	}

	public bool ReleaseFrom(Grabber grabber) {
		if (IsBeingGrabbedBy(grabber)) {
			IsBeingGrabbed = false;
			transform.SetParent(null, true);
			
			rigidbody.isKinematic = false;
			rigidbody.velocity = grabber.Velocity;
			rigidbody.angularVelocity = grabber.AngularVelocity; ;

			OnBeingReleased?.Invoke(this);
			return true;
		}
		return false;
	}
}
