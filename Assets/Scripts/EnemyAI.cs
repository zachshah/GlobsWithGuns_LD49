using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public bool follower;
    public float speed;
    public float health;
    public float maxDistToKill;
    public GameObject objToFollow;
    public Transform markerMax;
    public Transform markerMin;
    Vector3 startingPosition;
    Vector3 direction;
    bool isDying;
    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
         direction = Vector3.forward + Vector3.right;
    }

    private void Update()
    {
        if (health <= 0)
        {
            
            StartCoroutine(deathSequence());
            GetComponent<SphereCollider>().enabled = false;
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 15f*Time.deltaTime);
        }
        if (Vector3.Distance(transform.position, objToFollow.transform.position)< maxDistToKill&&health>0)
        {
            GameObject.FindGameObjectWithTag("Manager").GetComponent<SwapTimeManager>().startDeath();

        }
    }
    void FixedUpdate()
    {
        objToFollow = GameObject.FindGameObjectWithTag("Player");
        if (follower)
        {
            transform.LookAt(new Vector3(objToFollow.transform.position.x, transform.position.y, objToFollow.transform.position.z));
            Vector3 dir = objToFollow.transform.position - transform.position;
            if (!GameObject.FindGameObjectWithTag("Manager").GetComponent<SwapTimeManager>().paused) {
                GetComponent<Rigidbody>().velocity = dir * speed;
            }
            else
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;

            }
        }
        else
        {
           
                if (transform.position.z > markerMax.position.z)
                {
                    direction.z = -1f;
                }
                else if (transform.position.z < markerMin.position.z)
                {
                    direction.z = 1f;
                }

                if (transform.position.x > markerMax.position.x)
                {
                    direction.x = -1f;

                }
                else if (transform.position.x < markerMin.position.x)
                {
                    direction.x = 1f;
                }
            
            if (!GameObject.FindGameObjectWithTag("Manager").GetComponent<SwapTimeManager>().paused)
            {
                GetComponent<Rigidbody>().velocity = direction * speed;

            }
            else
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;

            }

        }
    }
    IEnumerator deathSequence()
    {
        if (!isDying)
        {
            GameObject.FindGameObjectWithTag("Manager").GetComponent<ScoreManager>().addScore();

        }
        isDying = true;
        yield return new WaitForSeconds(1);
       
        Destroy(this.gameObject);
    }
}
