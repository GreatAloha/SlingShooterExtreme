using UnityEngine;
using System.Collections;
using Assets.Scripts;
using System;

public class Catapult : MonoBehaviour
{

    private GameObject launchPoint;
    private bool aimingMode;
    private Vector3 mouseDelta;

    private Vector3 launchPos;

    [HideInInspector]
    public CatapultState catapultstate;

    private bool CatapultActive;
    public SkinnedMeshRenderer CatapultShape;
    private float CatapultLerp;

    public LineRenderer TrajectoryLineRenderer;

    public GameObject ProjectileToThrow;

    public Transform ProjectileWaitPosition;

    public float ThrowSpeed;

    [HideInInspector]
    public float TimeSinceThrown;


    void Start()
    {
        TrajectoryLineRenderer.sortingLayerName = "Foreground";
    
        catapultstate = CatapultState.Idle;
        Transform launchPointTrans = transform.Find("Launchpoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);
        launchPos = launchPointTrans.position;

    }

    void Update()
    {

        switch (catapultstate)
        {
            case CatapultState.Idle:
                InitializeProjectile();
                if (Input.GetMouseButtonDown(0))
                {
                    //get the point on screen user has tapped
                    Vector3 location = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    if (ProjectileToThrow.GetComponent<CircleCollider2D>() == Physics2D.OverlapPoint(location))
                    {
                        catapultstate = CatapultState.UserPulling;
                    }
                }
                break;

            case CatapultState.UserPulling:
                if (Input.GetMouseButton(0))
                {
                    //get where user is tapping
                    Vector3 location = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    location.z = 0;
                    //we will let the user pull the bird up to a maximum distance
                    if (Vector3.Distance(location, launchPos) > 1.5f)
                    {
                        var maxPosition = (location - launchPos).normalized * 1.5f + launchPos;
                        ProjectileToThrow.transform.position = maxPosition;
                    }
                    else
                    {
                        ProjectileToThrow.transform.position = location;
                    }
                    float distance = Vector3.Distance(launchPos, ProjectileToThrow.transform.position);
                    //display projected trajectory based on the distance
                    DisplayTrajectoryLineRenderer2(distance);
                }
                else//user has removed the tap 
                {
                    SetTrajectoryLineRenderesActive(false);
                    TimeSinceThrown = Time.time;
                    float distance = Vector3.Distance(launchPos, ProjectileToThrow.transform.position);
                    if (distance > 1)
                    {
                        catapultstate = CatapultState.ProjectileFlying;
                        ThrowProjectile(distance);
                    }
                    else
                    {
                        ProjectileToThrow.transform.positionTo(distance / 10, //duration
                            ProjectileWaitPosition.transform.position). //final position
                            setOnCompleteHandler((x) =>
                            {
                                x.complete();
                                x.destroy();
                                InitializeProjectile();
                            });

                    }
                }
                break;
            case CatapultState.ProjectileFlying:
                break;
            default:
                break;
        }

        // Get our mouse position and convert to 3D
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        mousePos2D = Camera.main.ScreenToWorldPoint(mousePos2D);
        // Calculate the delta between  mouse position and launch position 
        mouseDelta = mousePos2D - launchPos;


        if (aimingMode)
        {
            float blend = mouseDelta.magnitude;
            CatapultActive = true;
            launchPoint.SetActive(true);
            GetComponent<AudioSource>().Play();
        }
    }

    private void ThrowProjectile(float distance)
    {
        //get velocity
        Vector3 velocity = launchPos - ProjectileToThrow.transform.position;
        ProjectileToThrow.GetComponent<Projectile>().OnThrow();

        ProjectileToThrow.GetComponent<Rigidbody>().velocity = new Vector2(velocity.x, velocity.y) * ThrowSpeed * distance;

        CatapultShape.SetBlendShapeWeight(0, Mathf.Lerp(CatapultShape.GetBlendShapeWeight(0), 100f, 0.6f));


        //notify
        if (ProjectileThrown != null)
            ProjectileThrown(this, EventArgs.Empty);
    }

    public event EventHandler ProjectileThrown;

    private void InitializeProjectile()
    {
        //initialization
        float blend = mouseDelta.magnitude;
        CatapultShape.SetBlendShapeWeight(0, Mathf.Lerp(CatapultShape.GetBlendShapeWeight(0), 0f, 7.5f));
        ProjectileToThrow.transform.position = ProjectileWaitPosition.position;
        catapultstate = CatapultState.Idle;
       
    }

    void SetTrajectoryLineRenderesActive(bool active)
    {
        TrajectoryLineRenderer.enabled = active;
    }


    void DisplayTrajectoryLineRenderer2(float distance)
    {
        SetTrajectoryLineRenderesActive(true);
        Vector3 v2 = launchPos - ProjectileToThrow.transform.position;
        int segmentCount = 15;
        float segmentScale = 2;
        Vector2[] segments = new Vector2[segmentCount];

        // The first line point is wherever the player's cannon, etc is
        segments[0] = ProjectileToThrow.transform.position;

        // The initial velocity
        Vector2 segVelocity = new Vector2(v2.x, v2.y) * ThrowSpeed * distance;

        float angle = Vector2.Angle(segVelocity, new Vector2(1, 0));
        float time = segmentScale / segVelocity.magnitude;
        for (int i = 1; i < segmentCount; i++)
        {
            float time2 = i * Time.fixedDeltaTime * 5;
            segments[i] = segments[0] + segVelocity * time2 + 0.5f * Physics2D.gravity * Mathf.Pow(time2, 2);
        }

        TrajectoryLineRenderer.SetVertexCount(segmentCount);
        for (int i = 0; i < segmentCount; i++)
            TrajectoryLineRenderer.SetPosition(i, segments[i]);
    }
}












