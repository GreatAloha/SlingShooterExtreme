using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class Projectile : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        GetComponent<TrailRenderer>().enabled = false;
        GetComponent<TrailRenderer>().sortingLayerName = "Foreground";
        GetComponent<Rigidbody>().isKinematic = true;

        GetComponent<SphereCollider>().radius = Constants.ProjectileColliderRadiusBig;
        State = ProjectileState.BeforeThrown;
    }



    void FixedUpdate()
    {
        if (State == ProjectileState.Thrown &&
            GetComponent<Rigidbody>().velocity.sqrMagnitude <= Constants.MinVelocity) ;
       // {
         //   //destroy the bird after 2 seconds
        //    StartCoroutine(DestroyAfter(2));
       // }
    }

    public void OnThrow()
    {
        //show the trail renderer
        GetComponent<TrailRenderer>().enabled = true;
        //allow for gravity forces
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<SphereCollider>().radius = Constants.ProjectileColliderRadiusNormal;
        State = ProjectileState.Thrown;
    }

    IEnumerator DestroyAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        //Destroy(gameObject);
    }

    public ProjectileState State
    {
        get;
        private set;
    }
}

