using UnityEngine;
using System.Collections;

public class ProjectileAim : MonoBehaviour {
	
	public int numSteps=100;
    public Vector3 gravity = new Vector3(0.0f, -10.0f, 0.0f);
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
			
			position += velocity * Time.fixedDeltaTime*3;
			velocity += gravity * Time.fixedDeltaTime*3;
		}
	}
	
	
}
