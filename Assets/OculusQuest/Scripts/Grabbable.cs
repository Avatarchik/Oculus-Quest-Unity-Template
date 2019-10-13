using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Grabbable : MonoBehaviour
{
	private new Rigidbody rigidbody;

	private void Start() {
		rigidbody = GetComponent<Rigidbody>();

		// set rigidbody interpolation to solve jitter issue
		// https://forum.unity.com/threads/jittery-motion-on-oculus-quest.683704/#post-4576513
		rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
	}

	public bool IsGrabbingBy(Grabber grabber) {
		return transform.parent == grabber.SnapTransform || transform.parent == grabber.transform;
	}

	public void GrabBy(Grabber grabber) {
		rigidbody.velocity = Vector3.zero;
		rigidbody.angularVelocity = Vector3.zero;

		if (grabber.SnapTransform != null) {
			transform.SetParent(grabber.SnapTransform, true);
			transform.localPosition = Vector3.zero;
			transform.localEulerAngles = Vector3.zero;
		}
		else {
			transform.SetParent(grabber.transform, true);
		}
	}

	public bool ReleaseFrom(Grabber grabber) {
		if (IsGrabbingBy(grabber)) {
			transform.SetParent(null, true);
			
			rigidbody.isKinematic = false;
			rigidbody.velocity = grabber.Velocity;
			rigidbody.angularVelocity = grabber.AngularVelocity; ;

			return true;
		}
		return false;
	}
}
