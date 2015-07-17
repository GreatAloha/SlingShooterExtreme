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
	private SkinnedMeshRenderer CatapultShape;
	private float CatapultLerp;

	//audio
	private AudioSource source;
	public AudioClip ShootAudio;


	void Awake() {
		Transform launchPointTrans = transform.Find("Launchpoint");
		launchPoint = launchPointTrans.gameObject;
		launchPoint.SetActive(false);
		launchPos = launchPointTrans.position;
		Catapult = GameObject.Find("Katapult");
	}

	void OnMouseEnter() {
		//print ("Slingshot:MouseEnter");
		launchPoint.SetActive(true);
		CatapultActive = true;
	}

	void OnMouseExit() {
		//print ("Slingshot:MouseExit");
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
		projectile.GetComponent<MeshRenderer> ().enabled = false;
		projectile.GetComponent<TrailRenderer>().enabled = false;

	}

	void Update() {
		// Check for aiming mode
		if(!aimingMode) return;

		// Get our mouse position and convert to 3D
		Vector3 mousePos2D = Input.mousePosition;
		mousePos2D.z = - Camera.main.transform.position.z;
		mousePos2D = Camera.main.ScreenToWorldPoint(mousePos2D);
		// Calculate the delta between  mouse position and launch position 
		mouseDelta = mousePos2D - launchPos;
		float CatapultRot = -Mathf.Atan2(mouseDelta.x, mouseDelta.y) * Mathf.Rad2Deg;


		// Constrain the delta to the radius of the sphere collider
		float maxMagnitude = this.GetComponent<SphereCollider>().radius;
		mouseDelta = Vector3.ClampMagnitude(mouseDelta, maxMagnitude);

		// Set projectile position to new position and fire it
		projectile.transform.position = launchPos + mouseDelta;
		if(aimingMode){
			float blend = mouseDelta.magnitude - 2f;
			CatapultActive = true;
			launchPoint.SetActive ( true ) ;
			// Get mouse position and convert to 3d
			CatapultShape.SetBlendShapeWeight(0, Mathf.Lerp (CatapultShape.GetBlendShapeWeight(0),blend*35f,0.35f));
			Catapult.transform.rotation = Quaternion.Euler (0,0,CatapultRot);
			// Calculate the delata between launch position and mouse position
			
			
			// Comnstrain the dealta to the radius of the sphere collider
			maxMagnitude = this.GetComponent<SphereCollider>().radius - projectile.GetComponent<SphereCollider>().radius;
			mouseDelta = Vector3.ClampMagnitude(mouseDelta, maxMagnitude);
			
			// Set projectile position to new position and fire it
			projectile.transform.position = launchPos + mouseDelta;
			
			//Linerenderer
			launchAim.UpdateTraj(mouseDelta * velocityMult, launchPos+mouseDelta);
		}
		//blendshape calculation based on mouse position
		else if(CatapultActive){
			Catapult.transform.rotation = Quaternion.Lerp (Catapult.transform.rotation,Quaternion.Euler(0,0,CatapultRot),0.1f);
		}
		else{
			CatapultShape.SetBlendShapeWeight(0, Mathf.Lerp (CatapultShape.GetBlendShapeWeight(0),0f,0.15f));
			Catapult.transform.rotation = Quaternion.Lerp (Catapult.transform.rotation,Quaternion.Euler(0,0,0),0.01f);
		}

		if(Input.GetMouseButtonUp(0)) {
			aimingMode = false;
			projectile.GetComponent<Rigidbody>().isKinematic =false;

			projectile.GetComponent<Rigidbody>().velocity = - mouseDelta * velocityMult;

			FollowCam.S.poi = projectile;
		
		}
	}

	void OnMouseUp () {
		aimingMode=false;
		CatapultActive = false;
		projectile.GetComponent<TrailRenderer>().enabled = true;
		projectile.GetComponent<MeshRenderer> ().enabled = true;
		projectile.GetComponent<Rigidbody>().isKinematic = false; 
		//projectile.GetComponent<Rigidbody>().AddForce(-mouseDelta*1000);
		projectile.GetComponent<Rigidbody>().velocity = mouseDelta * velocityMult;
		FollowCam.S.poi = projectile;
		GameController.ShotFired();
		FollowCam.Shake (.8f);
	}


	public Vector3 getVelocity(){
		return mouseDelta * velocityMult;
	}
}











