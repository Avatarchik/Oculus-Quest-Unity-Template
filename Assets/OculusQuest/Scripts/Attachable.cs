using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attachable : MonoBehaviour
{
	public Transform SnapTransform;

	public Transform AttachTransform {
		get {
			return SnapTransform ?? transform;
		}
	}

	public void SetChild(Transform transform) {
		if(SnapTransform != null) {
			transform.SetParent(SnapTransform, true);
			transform.localPosition = Vector3.zero;
			transform.localEulerAngles = Vector3.zero;
		}
		else {
			transform.SetParent(this.transform, true);
		}
	}
}
