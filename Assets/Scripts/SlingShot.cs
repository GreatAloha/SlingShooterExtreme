using UnityEngine;
using System.Collections;

public class SlingShot : MonoBehaviour {

	private GameObject launchPoint;

	void Awake(){
		Transform launchPointTrans = transform.Find ("LaunchPoint");
		launchPoint = launchPointTrans.gameObject;
		launchPoint.SetActive (false);
	}

	void OnMouseEnter(){
		//print ("SlingShot:MouseEnter");
		launchPoint.SetActive (true);
	}

	void OnMouseExit(){
		//print ("SlingShot:MouseExit");
		launchPoint.SetActive (false);
	}

}
