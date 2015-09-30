using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow S;
    private float camZ;
    public GameObject POI;

    // Use this for initialization
    void Start()
    {
        S = this;
        StartingPosition = transform.position;
        camZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsFollowing)
        {
            if (ProjectileToFollow != null) 
            {
                var projectilePosition = ProjectileToFollow.transform.position;
                float x = Mathf.Clamp(projectilePosition.x, minCameraX, maxCameraX);
                transform.position = new Vector3(x, StartingPosition.y, StartingPosition.z);
            }
            else
                IsFollowing = false;
        }
    }

    public Vector3 StartingPosition;


    private const float minCameraX = 0;
    private const float maxCameraX = 36;
    [HideInInspector]
    public bool IsFollowing;
    [HideInInspector]
    public Transform ProjectileToFollow;
}

