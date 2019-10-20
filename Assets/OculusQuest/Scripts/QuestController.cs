using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestController : MonoBehaviour
{
	public OVRInput.Controller Controller;

	public Vector3 Velocity {
		get {
			return OVRInput.GetLocalControllerVelocity(Controller);
		}
	}

	public Vector3 AngularVelocity {
		get {
			return OVRInput.GetLocalControllerAngularVelocity(Controller) * -1;
		}
	}

	public void GetGripButton(out bool pressed, out bool released, float pressThreadhold = 0.55f, float releaseThreadhold = 0.35f) {
		float value = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, Controller);
		pressed = false;
		released = false;

		if(value > pressThreadhold) {
			pressed = true;
		}
		else if(value < releaseThreadhold) {
			released = true;
		}
	}
}
