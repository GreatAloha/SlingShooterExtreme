using UnityEngine;
using System.Collections;

public class Worm : MonoBehaviour {
		
		public float Health = 90f;
		// Use this for initialization
		
		void OnCollisionEnter(Collision col)
		{
			if (col.gameObject.GetComponent<Rigidbody>() == null) return;
			
			//Hit by a projectile
			if (col.gameObject.tag == "Projectile")
			{
				GetComponent<AudioSource>().Play();
				Destroy(gameObject);
			}
			else //Hit
			{
				//calculate the damage via the hit object velocity
				float damage = col.gameObject.GetComponent<Rigidbody>().velocity.magnitude * 10;
				Health -= damage;

				if (damage >= 10)
					GetComponent<AudioSource>().Play();

				if (Health <= 0) Destroy(this.gameObject);
			}
		}
	}
