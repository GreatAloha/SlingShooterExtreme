using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {

	// Static field accessible from anywhere
	public static bool goalMet;

	void OnTriggerEnter(Collider other) {
		// Check if the object is a projectile
		if(other.gameObject.tag == "Projectile") {
			// If so, set goalMet to true
			goalMet = true;

			// Also set the goals alpha to a higher opacity
			// (use Renderer component)
			Color c = this.GetComponent<Renderer>().material.color;

			c.a = 1;

			this.GetComponent<Renderer>().material.color = c;


		}
	}



}







