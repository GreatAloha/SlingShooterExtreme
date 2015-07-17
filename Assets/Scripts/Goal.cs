using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {
	// Static field accessible from anywhere
	// Storing if the goal was met
	public static bool goalMet = false;


	// Trigger got entered
	void OnTriggerEnter(Collider other) {
		// Check if the object entering the trigger is a projectile
		if(other.gameObject.tag == "Projectile") {
			// Set the staic field to true
			goalMet = true;
			// Set the alpha of the color to a higher opacity

			Color c = this.gameObject.GetComponent<Renderer>().material.color;

			c.a = 1;

			this.gameObject.GetComponent<Renderer>().material.color = c;

		}
	
	}


}
