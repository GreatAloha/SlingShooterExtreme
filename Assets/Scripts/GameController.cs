using UnityEngine;
using System.Collections;
using UnityEngine.UI;


enum GameState {
	idle,
	playing,
	levelEnd
}


public class GameController : MonoBehaviour {

	public static GameController S;
	
	// Public inspector fields
	public GameObject[] castles;
	public Vector3 castlePos;

	public Text gtLevel;
	public Text gtShots;
	
	// Internal fields
	private int level;
	private int levelMax;
	private int shotsTaken;
	private GameObject castle;
	private string showing = "Slingshot";
	private GameState state = GameState.idle;


	void Awake() {
		S = this;

		level = 0;
		levelMax = castles.Length;

		StartLevel();
	
	}

	void StartLevel() {
		// if there is a castle, destroy it

		// Destroy all remaining projectile

		// Instantiate a new castle

		// Swith the View to "Both"

		// Clear all Projectile Trails
	
	}

	void Update() {
		// Update our GUI texts

		// Check for level end

	}




	public void SwitchView(string view) {

		// Switch over all possibilies "Castle", "Both", "Slingshot"

			// Set the Followcams POI the the according value
	
	
	}


}
