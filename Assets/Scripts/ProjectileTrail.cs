using UnityEngine;
using System.Collections;

public class ProjectileTrail : MonoBehaviour {
	// Singleton instance
	public static ProjectileTrail S;
	
	// Public inspector fields
	public float minDist = 0.1f;
	
	// Internal fields
	private LineRenderer line;
	private int pointsCount;
	
	private GameObject _poi;
	
	private Vector3 lastPoint;
	
	
	void Awake() {
		S = this;
		
		// Store the linerenderer component in a field
		line = this.GetComponent<LineRenderer>();
		
		// Initalize fields
		pointsCount = 0;
		
		// Disable until needed
		line.enabled = false;
		
		// Set line colors
		Color c1 = Color.yellow;
		Color c2 = Color.red;
		
		line.SetColors(c1,c2);
		
	}
	
	void FixedUpdate() {
		if(poi == null) {
			// If there is no poi yet, try looking at the camera
			if(FollowCam.S.poi != null) {
				if(FollowCam.S.poi.tag == "Projectile") {
					poi = FollowCam.S.poi;
				} else {
					return; // Give up, no poi found
				}
			} else {
				return; // Give up, no poi found
			}
		}
		
		// Now poi definitely has a value and its a projectile
		// So add a point in every FixedUpdate()
		AddPoint();
		if(poi.GetComponent<Rigidbody>().IsSleeping()){
			// The poi is resting, so clear it
			poi = null;
		}
	}
	
	public void AddPoint(){
		Vector3 pt = _poi.transform.position;
		// If the point isnt far enough from the last one, do nothing
		if(pointsCount > 0 && (pt - lastPoint).magnitude < minDist) {
			return;
		}
		
		// If its the first point, enable the line
		if(pointsCount == 0){
			line.enabled = true;
		}
		// For each point, set the new vertex count and set the new position
		pointsCount++;
		line.SetVertexCount(pointsCount);
		line.SetPosition(pointsCount - 1, pt);
		
		
		lastPoint = pt;
	}
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	public void Clear(){
		_poi = null;
		line.enabled = false;
		pointsCount = 0;
		line.SetVertexCount(0);
	}
	
	
	
	// A property: Looks to the outside like a field but calls get/set internally
	
	public GameObject poi {
		get {
			return _poi;
		}
		
		set {
			// Use "value" to access the new value of the property
			// Set the new value of _poi
			_poi = value;
			
			// Check if the poi was set to something (and now to something new)
			// reset the whole thingy
			if(_poi != null) {
				line.enabled = false;
				pointsCount = 0;
				line.SetVertexCount(0);
				
			}
		}
	}
	
	
}
