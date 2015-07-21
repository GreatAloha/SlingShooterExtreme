using UnityEngine;
using System.Collections;

public class Slingshot : MonoBehaviour {

	// Inspector variables
	public GameObject prefabProjectile;
	public float velocityMult = 4.0f;
	public ProjectileAim launchAim;

	// Internal state variables
	private GameObject launchPoint;
	private bool aimingMode;
	private float maxMagnitude;
	private Vector3 mouseDelta;

	private GameObject projectile;
	private Vector3 launchPos;

	private GameObject Catapult;
	private bool CatapultActive;
	public SkinnedMeshRenderer CatapultShape;
	private float CatapultLerp;


	void Awake() {
		Transform launchPointTrans = transform.Find("Launchpoint");
		launchPoint = launchPointTrans.gameObject;
		launchPoint.SetActive(false);
		launchPos = launchPointTrans.position;
		Catapult = GameObject.Find("Catapult");
	}

	void OnMouseEnter() {
		launchPoint.SetActive(true);
		CatapultActive = true;
	}

	void OnMouseExit() {
		if(!aimingMode)
			launchPoint.SetActive(false);
		CatapultActive = false;
	}

	void OnMouseDown() {
		// Set the game to aiming mode
		aimingMode = true;
		CatapultActive = true;

		// Instantiate a projectile at launchPoint
		projectile = Instantiate ( prefabProjectile ) as GameObject;
		projectile.transform.position = launchPos;

		// Switch off physics for now
		projectile.GetComponent<Rigidbody>().isKinematic =true;

	}

	void Update() {

		// Get our mouse position and convert to 3D
		Vector3 mousePos2D = Input.mousePosition;
		mousePos2D.z = - Camera.main.transform.position.z;
		mousePos2D = Camera.main.ScreenToWorldPoint (mousePos2D);
		// Calculate the delta between  mouse position and launch position 
		mouseDelta = mousePos2D - launchPos;


		if (aimingMode) {
			float blend = mouseDelta.magnitude;
			CatapultActive = true;
			launchPoint.SetActive (true);
			GetComponent<AudioSource>().Play();
			if (CatapultActive)
				CatapultShape.SetBlendShapeWeight (0, Mathf.Lerp (CatapultShape.GetBlendShapeWeight (0), blend, 3.5f));
			// Calculate the delata between launch position and mouse position
			
			
			// Comnstrain the dealta to the radius of the sphere collider
			maxMagnitude = this.GetComponent<SphereCollider> ().radius - projectile.GetComponent<SphereCollider> ().radius;
			mouseDelta = Vector3.ClampMagnitude (mouseDelta, maxMagnitude);
			
			// Set projectile position to new position and fire it
			projectile.transform.position = launchPos + mouseDelta;
			
			//Linerenderer
			launchAim.UpdateAim (-mouseDelta * velocityMult, launchPos + mouseDelta);



			if (Input.GetMouseButtonUp (0)) {
				aimingMode = false;
				projectile.GetComponent<Rigidbody> ().isKinematic = false;

				projectile.GetComponent<Rigidbody> ().velocity = - mouseDelta * velocityMult;
				CatapultActive = false;
				FollowCam.S.poi = projectile;
		
			}
		} else {
			if (!CatapultActive)
				CatapultShape.SetBlendShapeWeight (0, Mathf.Lerp (CatapultShape.GetBlendShapeWeight (0), 100f, 0.5f));
		}
	}

	public Vector3 getVelocity(){
		return mouseDelta * velocityMult;
	}
}











