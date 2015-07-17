using UnityEngine;
using System.Collections;

public class CloudCrafter : MonoBehaviour {

	// Fields in Inspector pane

	public GameObject[] cloudPrefabs;

	public int numClouds = 40;

	public Vector3 cloudPosMin;
	public Vector3 cloudPosMax;

	public float cloudSpeedMult = 0.5f;

	public float cloudScaleMin = 1.0f;
	public float cloudScaleMax = 5.0f;


	// Internal fields
	public GameObject[] cloudInstances;

	void Awake() {
		// Create an array to hold our cloud instances
		cloudInstances = new GameObject[numClouds];

		// Find the clouds anchor object
		GameObject anchor = GameObject.Find("Clouds");

		// Iterate through array and create each cloud
		GameObject cloud;
		for(int i = 0; i<numClouds; i++) {

			// Pick a random cloud prefab between 0 and cloudPrefabs.Length-1,
			int prefabNum = Random.Range(0, cloudPrefabs.Length-1);


			// Instantiate a cloud and position it accordingly

			cloud = Instantiate(cloudPrefabs[prefabNum]);

			Vector3 cPos = Vector3.zero;
			cPos.x = Random.Range(cloudPosMin.x, cloudPosMax.x);
			cPos.y = Random.Range(cloudPosMin.y, cloudPosMax.y);

			float scaleU = Random.value;
			float scaleVal = Mathf.Lerp(cloudScaleMin,cloudScaleMax,scaleU);

			cPos.y = Mathf.Lerp(cloudPosMin.y, cPos.y, scaleU);

			cPos.z = 100 - 90*scaleU;



			// Scale the cloud

			cloud.transform.position = cPos;
			cloud.transform.localScale = Vector3.one * scaleVal;

			// Make the cloud a child of the cloud anchor
			cloud.transform.parent = anchor.transform;


			// Add the cloud to our clud instances
			cloudInstances[i] = cloud;

		}
	
	}


	void Update() {
		// Iterate over all cloud objects in the background

		foreach(GameObject cloud in cloudInstances) {
			// Get Polsition and scale
			float scaleVal = cloud.transform.localScale.x;
			Vector3 cPos = cloud.transform.position;

			// Move larger clouds faster

			cPos.x -= Time.deltaTime * cloudSpeedMult * scaleVal;

			if(cPos.x < cloudPosMin.x){
				cPos.x = cloudPosMax.x;
			}

			cloud.transform.position = cPos;

		
		}
	
	
	}




}







