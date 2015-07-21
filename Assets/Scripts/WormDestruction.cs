using UnityEngine;
using System.Collections;

public class WormDestruction : MonoBehaviour {
		
		public float Health = 150f;
		// Use this for initialization
		void Start()
		{

		}
		
		void OnCollisionEnter(Collision col)
		{
			if (col.gameObject.GetComponent<Rigidbody>() == null) return;
			
			//if we are hit by a projectile
			if (col.gameObject.tag == "Projectile")
			{
				GetComponent<AudioSource>().Play();
				Destroy(gameObject);
			}
			else //we're hit by something else
			{
				//calculate the damage via the hit object velocity
				float damage = col.gameObject.GetComponent<Rigidbody>().velocity.magnitude * 10;
				Health -= damage;
				//don't play sound for small damage
				if (damage >= 10)
					GetComponent<AudioSource>().Play();
				if (Health <= 0) Destroy(this.gameObject);
			}
		}
	}
