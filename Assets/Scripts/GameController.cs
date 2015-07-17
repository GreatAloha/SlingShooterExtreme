using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.UI;

enum GameState { 
	idle,
	playing,
	levelEnd
}

public class GameController : MonoBehaviour {
	
	public static GameController S;
	
	
	//public inspector fields
	public GameObject[] castles;
	public Vector3 castlePos;
	
	public Text gtLevel;
	public Text gtShots;
	
	//internal fields
	private int level;
	private int levelMax;
	private int shotsTaken;
	private GameObject castle;
	private string showing = "Slingshot";
	
	private GameState state = GameState.idle;
	
	
	void Awake() {
		S = this;
		
		level = 0 ;
		levelMax = castles.Length;
		
		StartLevel();
		
	}
	
	void Update() {
		UpdateGT();
		
		// Check for level end
		if(state == GameState.playing && Goal.goalMet) {
			//if(FollowCam.S.poi.tag == "Projectile" &&  FollowCam.S.poi.GetComponent<Rigidbody>().IsSleeping()) {
			// Change state to stop checking for level end
			state = GameState.levelEnd;
			// Zoom out
			SwitchView("Both");
			// Start next level in 2 seconds
			Invoke("NextLevel", 2f);
			print ("ttttt");
			//}
		}
	}
	
	void UpdateGT() {
		gtLevel.text = "Level:" + (level+1) + " of " + levelMax;
		gtShots.text = "Shots Taken: " + shotsTaken;
	}
	
	public void SwitchView(string view) {
		S.showing = view;
		switch(S.showing){
		case "Slingshot":
			FollowCam.S.poi = null;
			break;
		case "Castle":
			FollowCam.S.poi = S.castle;
			break;
		case "Both":
			FollowCam.S.poi = GameObject.Find ("ViewBoth");
			break;
		}
	}
	
	
	public void StartLevel() {
		// If a castle exists, get rid of it
		if(castle != null) {
			Destroy (castle);
		}
		
		// Destroy the old projectiles
		GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Projectile");
		foreach(GameObject p in projectiles){
			Destroy(p);
		}
		
		// Instantiate the new castle
		castle = Instantiate (castles[level]) as GameObject;
		castle.transform.position = castlePos;
		shotsTaken = 0;
		
		// Reset the camera
		SwitchView("Both");
		//ProjectileLine.S.Clear();
		
		// Reset the Goal
		Goal.goalMet = false;
		UpdateGT();
		
		state = GameState.playing;
		
	}
	
	void NextLevel() {
		level++;
		if(level == levelMax){
			level = 0;
		}
		StartLevel();
	}
	
	// Static function that allows to increment the score
	public static void ShotFired(){
		S.shotsTaken++;
	}
	
	public void Quit()
	{
		Application.Quit();
	}

}
