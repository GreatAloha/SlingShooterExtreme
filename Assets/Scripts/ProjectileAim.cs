using UnityEngine;
using System.Collections;

public class ProjectileAim : MonoBehaviour {
	
	public int numSteps=20;
	//public Vector3 gravity = new Vector3(0.0f, -9.82f, 0.0f);
	public Transform initialPosition;
	
	public void UpdateTraj(Vector3 initialVelocity,Vector3 initialPosition)
	{
		LineRenderer lr = GetComponent<LineRenderer>();
		lr.SetVertexCount(numSteps);
		
		Vector3 position = initialPosition;
		Vector3 velocity = initialVelocity;
		for (int i = 0; i < numSteps; ++i)
		{
			lr.SetPosition(i, position);
			
			position += velocity * Time.fixedDeltaTime*3;
			velocity += Physics.gravity * Time.fixedDeltaTime*3;
		}
	}
	
	
}
