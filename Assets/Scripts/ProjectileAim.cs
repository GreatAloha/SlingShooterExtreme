using UnityEngine;
using System.Collections;

public class ProjectileAim : MonoBehaviour {
	
	public int numSteps;
	public Transform initialPosition;
	
	public void UpdateAim(Vector2 initialVelocity,Vector2 initialPosition)
	{
		LineRenderer lr = GetComponent<LineRenderer>();
		lr.SetVertexCount(numSteps);

		
		Vector3 position = initialPosition;
		Vector3 velocity = initialVelocity;
		for (int i = 0; i < numSteps; ++i)
		{
			lr.SetPosition(i, position);
			
			position += velocity * Time.fixedDeltaTime*2;
			velocity += Physics.gravity * Time.fixedDeltaTime*2;
		}
	}
	
	
}
