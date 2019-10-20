using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapDropZone : Attachable
{
	public List<string> AcceptTags;
	public Material HighlightMaterial;

	private MeshRenderer[] renderers;
	private List<Grabbable> touchingGrabbables = new List<Grabbable>();
	private Grabbable snappedGrabbable = null;

	private void Start() {
		SnapTransform = transform;

		foreach(Collider collider in GetComponentsInChildren<Collider>()) {
			collider.isTrigger = true;
		}

		// replace original material with hightlight material
		renderers = GetComponentsInChildren<MeshRenderer>();
		foreach(MeshRenderer renderer in renderers) {
			renderer.material = HighlightMaterial;
			renderer.enabled = false;
		}
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

		// must be a grabbed grabbable
		if (grabbable == null || !grabbable.IsBeingAttachedByGrabber || touchingGrabbables.Contains(grabbable)) {
			return false;
		}

		// must has matching tag
		if (AcceptTags.Count > 0 && !AcceptTags.Contains(grabbable.tag)) {
			return false;
		}

		return true;
	}

	public void Highlight() {
		ToggleRenderers(true);
	}

	public void Unhighlight() {
		ToggleRenderers(false);
	}

	public void Snap(Grabbable grabbable) {
		Unhighlight();

		foreach(Grabbable touchingGrabbable in touchingGrabbables) {
			touchingGrabbable.OnBeingReleased -= Snap;
		}

		grabbable.AttachTo(this);
		grabbable.OnBeingAttached += OnItemBeingDetached;

		touchingGrabbables.Clear();
		snappedGrabbable = grabbable;
	}

	private void ToggleRenderers(bool enabled) {
		foreach (MeshRenderer renderer in renderers) {
			renderer.enabled = enabled;
		}
	}

	private void OnItemBeingDetached(Grabbable grabbable) {
		snappedGrabbable.OnBeingAttached -= OnItemBeingDetached;
		snappedGrabbable = null;
	}
}
