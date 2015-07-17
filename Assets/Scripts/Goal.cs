using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {

	// Static field accessible from anywhere
	public static bool goalMet;

	void OnTriggerEnter(Collider other) {
		// Check if the object is a projectile
		if(other.tag == "Projectile") {
			// If so, set goalMet to true
			goalMet = true;
			print("success");

			// Also set the goals alpha to a higher opacity
			// (use Renderer component)
			Color c = this.GetComponent<Renderer>().material.color;

			c.a += 0.5f;

			this.GetComponent<Renderer>().material.color = c;


		}
	}



}







