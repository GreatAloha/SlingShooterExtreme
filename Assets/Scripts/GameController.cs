using Assets.Scripts;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class GameController : MonoBehaviour {
	
	public static GameController S;

    public CameraFollow cameraFollow;

    public GameObject[] castles;
    public GameObject[] projectiles;
	public Vector3 castlePos;
    public Vector3 ProjectileSpawn;
	
	public Text gtLevel;
	private int level;
	private int levelMax;
    private string view;

    public Catapult catapult;
    public Vector3 ViewBoth;
    public Vector3 CataPos;
    public Vector3 CastlePos;


    private GameObject SpawnProjectile;
    private GameObject castle;

	private string showing = "Catapult";

    private GameState state = GameState.Idle;

    [HideInInspector]
    public static GameState CurrentGameState = GameState.Start;

    int currentProjectileIndex;
    private List<GameObject> Worms;
    private List<GameObject> Projectiles;
    private List<GameObject> Walls;



    void Awake()
    {
        S = this;

        level = 0;
        levelMax = castles.Length;
        GetComponent<AudioSource>().Play();

        StartLevel();

        CurrentGameState = GameState.Start;
        catapult.enabled = false;
        Walls = new List<GameObject>(GameObject.FindGameObjectsWithTag("Castle"));
        Projectiles = new List<GameObject>(GameObject.FindGameObjectsWithTag("Projectile"));
        Worms = new List<GameObject>(GameObject.FindGameObjectsWithTag("Worm"));
        catapult.ProjectileThrown -= Catapult_ProjectileThrown; catapult.ProjectileThrown += Catapult_ProjectileThrown;
		
	}

    void Update() {

        UpdateGT();

        switch (CurrentGameState)
        {
            case GameState.Start:
                //if(state == GameState.Won)
                //{
                  //  NextLevel();
                //}

                if (Input.GetMouseButtonUp(0))
                {
                    AnimateProjectileToCatapult();
                }

                break;
            case GameState.ProjectileMovingToCatapult:
                break;
            case GameState.Playing:
                if (catapult.catapultstate == CatapultState.ProjectileFlying &&
                    (WallsProjectilesWormsStoppedMoving() || Time.time - catapult.TimeSinceThrown > 6f))
                {
                    catapult.enabled = false;
                    AnimateCameraToStartPosition();
                    CurrentGameState = GameState.ProjectileMovingToCatapult;
                }
                break;
            case GameState.Won:
            case GameState.Lost:
                if (Input.GetMouseButtonUp(0))
                {
                    Application.LoadLevel(Application.loadedLevel);
                }
                break;
            default:
                break;
        }
    }

    private bool AllWormsDestroyed()
    {
        return Worms.All(x => x == null);
    }
    
	
	void UpdateGT() {
		gtLevel.text = "Level:" + (level+1) + " of " + levelMax;
	}

	public void StartLevel() {
		// If a castle exists, get rid of it
		if(castle != null) {
			Destroy (castle);
		}

		castle = Instantiate (castles[level]) as GameObject;
		castle.transform.position = castlePos;

		UpdateGT();
		
		state = GameState.Playing;
		
	}
	

    public void Quit()
    {
        Application.Quit();
    }

private void AnimateCameraToStartPosition()
{
    float duration = Vector2.Distance(Camera.main.transform.position, cameraFollow.StartingPosition) / 10f;
    if (duration == 0.0f) duration = 0.1f;
    //animate the camera to start
    Camera.main.transform.positionTo
        (duration,
        cameraFollow.StartingPosition). //end position
        setOnCompleteHandler((x) =>
        {
            cameraFollow.IsFollowing = false;
            if (AllWormsDestroyed())
            {
                CurrentGameState = GameState.Won;
            }
                //animate the next projectile, if available
                else if (currentProjectileIndex == Projectiles.Count - 1)
            {
                    CurrentGameState = GameState.Lost;
            }
            else
            {
                catapult.catapultstate = CatapultState.Idle;
                    //projectile to throw is the next on the list
                    currentProjectileIndex++;
                AnimateProjectileToCatapult();
            }
        });
}

/// </summary>
void AnimateProjectileToCatapult()
{
    CurrentGameState = GameState.ProjectileMovingToCatapult;
    Projectiles[currentProjectileIndex].transform.positionTo
        (Vector3.Distance(Projectiles[currentProjectileIndex].transform.position / 10,
        catapult.ProjectileWaitPosition.transform.position) / 10, //duration
        catapult.ProjectileWaitPosition.transform.position). //final position
            setOnCompleteHandler((x) =>
            {
                x.complete();
                x.destroy();
                    CurrentGameState = GameState.Playing;
                catapult.enabled = true;
                    catapult.ProjectileToThrow = Projectiles[currentProjectileIndex];
            });
}

private void Catapult_ProjectileThrown(object sender, System.EventArgs e)
{
    cameraFollow.ProjectileToFollow = Projectiles[currentProjectileIndex].transform;
    cameraFollow.IsFollowing = true;
}


bool WallsProjectilesWormsStoppedMoving()
{
    foreach (var item in Walls.Union(Projectiles).Union(Worms))
    {
        if (item != null && item.GetComponent<Rigidbody>().velocity.sqrMagnitude > Constants.MinVelocity)
        {
            return false;
        }
    }

    return true;
}

    public static void AutoResize(int screenWidth, int screenHeight)
    {
        Vector2 resizeRatio = new Vector2((float)Screen.width / screenWidth, (float)Screen.height / screenHeight);
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(resizeRatio.x, resizeRatio.y, 1.0f));
    }

    void OnGUI()
    {
        AutoResize(800, 480);
        switch (CurrentGameState)
        {
            case GameState.Start:
                GUI.Label(new Rect(0, 150, 200, 100), "Tap the screen to start");
                break;
            case GameState.Won:
                GUI.Label(new Rect(0, 150, 200, 100), "You won! Tap the screen to advance");
                break;
            case GameState.Lost:
                GUI.Label(new Rect(0, 150, 200, 100), "You lost! Tap the screen to restart");
                break;
            default:
                break;
        }
    }
}