using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SnapDropZone : Attachable
{
	public List<string> AcceptTags;
	public GameObject Placeholder;
	public Material HighlightMaterial;

	private MeshRenderer placeHolderRenderer;
	private List<Grabbable> touchingGrabbables = new List<Grabbable>();
	private Grabbable snappedGrabbable = null;

	private void Start() {
		// replace placeholder's original material with hightlight material
		placeHolderRenderer = Placeholder.GetComponent<MeshRenderer>();
		placeHolderRenderer.material = HighlightMaterial;
		placeHolderRenderer.enabled = false;
	}

	private void OnTriggerEnter(Collider other) {
		Grabbable grabbable = other.GetComponent<Grabbable>();
		if(CanAccept(grabbable)) {
			touchingGrabbables.Add(grabbable);
			grabbable.OnBeingReleased += Snap;
			Highlight();
		}
	}

	private void OnTriggerExit(Collider other) {
		Grabbable grabbable = other.GetComponent<Grabbable>();
		if(grabbable != null) {
			grabbable.OnBeingReleased -= Snap;
			touchingGrabbables.Remove(grabbable);
		}

		if(touchingGrabbables.Count == 0) {
			Unhighlight();
		}
	}

	public bool CanAccept(Grabbable grabbable) {
		// can only snap 1 object at a time
		if (snappedGrabbable != null) return false;

		// must be a free grabbable
		if (grabbable == null || grabbable.IsBeingAttached || touchingGrabbables.Contains(grabbable)) {
			return false;
		}

		// must has matching tag
		if (AcceptTags.Count > 0 && !AcceptTags.Contains(grabbable.tag)) {
			return false;
		}

		return true;
	}

	public void Highlight() {
		placeHolderRenderer.enabled = true;
	}

	public void Unhighlight() {
		placeHolderRenderer.enabled = false;
	}

	public void Snap(Grabbable grabbable) {
		Unhighlight();

		foreach(Grabbable touchingGrabbable in touchingGrabbables) {
			touchingGrabbable.OnBeingReleased -= Snap;
		}
		touchingGrabbables.Clear();
		snappedGrabbable = grabbable;

		grabbable.AttachTo(this);
	}
}
