using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public static GameController S;

	//Public Inspector fields
	public GameObject [] castles;
	public Vector3 castlePos;

	public Texture gtLevel;
	public Texture gtShots;

	//Internal Fields

	private int level;
	private int levelMax;
	private int shotsTaken;
	private GameObject castle;
	private string showing = "Slingshot";
	private Gamestate state = Gamestate.idle;

	void Awake(){

		S = this;

		level = 0;
		levelMax = castles.Length;

		StartLevel ();

	}

	void StartLevel(){

	}
}
