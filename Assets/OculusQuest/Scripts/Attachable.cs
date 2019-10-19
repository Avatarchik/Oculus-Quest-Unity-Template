using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Attachable : MonoBehaviour
{
	public Transform SnapTransform;

	public Transform AttachTransform {
		get {
			return SnapTransform ?? transform;
		}
	}

	public void Attach(Grabbable grabbable) {
		if (SnapTransform != null) {
			grabbable.transform.SetParent(SnapTransform, true);
			grabbable.transform.localPosition = Vector3.zero;
			grabbable.transform.localEulerAngles = Vector3.zero;
		}
		else {
			grabbable.transform.SetParent(this.transform, true);
		}
	}

	public void Detach(Grabbable grabbable) {
		grabbable.transform.SetParent(null, true);
	}
}
