using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestController : MonoBehaviour
{
	public OVRInput.Controller Controller;


	private Vector3 initScale;


	private void Start() {
		initScale = transform.localScale;
	}


	private void Update() {
		if(OVRInput.Get(OVRInput.Button.One, Controller)) {
			transform.localScale = initScale * 1.5f;
		}
		else {
			transform.localScale = initScale;
		}
	}
}
