using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour {


	public static FollowCam S; // Singleton Follow Cam Instance
	public GameObject poi;

	private float camZ;
	public float easing = 0.05f;
	public float zoom = 1;

	public Vector2 minXY;

	//camera shake

	private static float shake;
	public float shakeAmount = 2.5f;
	public float decreaseFactor = 1.5f;

	void Awake() {
		S = this;
		camZ = transform.position.z;
	}

	void FixedUpdate() {

			
		if (shake > 0) {S.transform.localPosition = new Vector3 (transform.localPosition.x + Random.insideUnitCircle.x * shakeAmount,S.transform.localPosition.y + Random.insideUnitCircle.y * shakeAmount, transform.position.z) ;
			shake -= Time.deltaTime * decreaseFactor;
				
			} else {
				shake = 0.0f;
			}

		Vector3 destination;

		// Check if the poi exists
		if(poi == null) {
			// set the destination to the slingshot (zero  vector)
			destination = Vector3.zero;
		} else {
			// else (there is a poi)

			destination = poi.transform.position;

			// is the poi a projectile ?
			if(poi.tag == "Projectile") {

				// check if it is resting (sleeping)
				if(poi.GetComponent<Rigidbody>().IsSleeping()  ){
			
					// set the poi to default (null)
					poi = null;
					return;
				}

			}
		}

		destination.x = Mathf.Max (minXY.x, destination.x);
		destination.y = Mathf.Max (minXY.y, destination.y);

		destination = Vector3.Lerp (transform.position, destination, easing);

		destination.z = camZ;

		transform.position = destination;

		this.GetComponent<Camera>().orthographicSize = 10 + destination.y;
	
	}

	public static void Shake(float init){
		shake = .35f;
	}



}






