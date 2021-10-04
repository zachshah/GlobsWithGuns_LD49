using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerGun : MonoBehaviour
{
    public bool canShoot;
    public float speed;  
    public Camera mainCam;
    public GameObject objToFollow;
    public Vector3 lookShift;
    Vector3 pointToLook;
    public GameObject vizualizeMouse;
    public GameObject visualizeShot;
    public Transform muzzlePos;
    public LayerMask layerMask;
    public float timeBetweenShots;
    public float shotRadius;

    public float shakeMagnitude;
    public float shakeDuration;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray camRay = mainCam.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if (groundPlane.Raycast(camRay, out rayLength))
        {
             pointToLook = camRay.GetPoint(rayLength) + lookShift;
            Debug.DrawLine(camRay.origin, pointToLook, Color.red);

            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
            vizualizeMouse.transform.position = pointToLook;
        }
        
            if (Input.GetButtonDown("Fire1"))
            {
                InvokeRepeating("shoot", 0f, timeBetweenShots);
            }

            if (Input.GetButtonUp("Fire1"))
            {
                CancelInvoke("shoot");
            }
        
    }
    private void FixedUpdate()
    {
        follow();
    }

    void shoot()
    {
        if (canShoot&&!GameObject.FindGameObjectWithTag("Manager").GetComponent<SwapTimeManager>().paused)
        {
            RaycastHit hit;
            if (Physics.SphereCast(transform.position, shotRadius, new Vector3(pointToLook.x,transform.position.y,pointToLook.z) - transform.position, out hit,200,layerMask))
            {
                if (hit.transform.gameObject.tag == "Enemy")
                {
                    hit.transform.gameObject.GetComponent<EnemyAI>().health -= 1;
                }
                Debug.Log(hit.transform.gameObject);
            }
            GameObject shotFired = Instantiate(visualizeShot, Vector3.zero, Quaternion.identity);
            Destroy(shotFired, timeBetweenShots / 2);
            LineRenderer lrShot = shotFired.GetComponent<LineRenderer>();
            lrShot.SetPosition(0, muzzlePos.position);
            
                lrShot.SetPosition(1, hit.point);

            
            StartCoroutine(Shake());
        }
    }

    

    void follow()
    {
        Vector3 direction = Vector3.zero;
       

        if (Vector3.Distance(transform.position, objToFollow.transform.position) > 1.5f)
        {
            direction = objToFollow.transform.position - transform.position;
            if (Vector3.Distance(transform.position, objToFollow.transform.position) > 3.5f)
            {
                speed = 4;
            }
            else
            {
                speed = 2;
            }
            if (!GameObject.FindGameObjectWithTag("Manager").GetComponent<SwapTimeManager>().paused)
            {
                GetComponent<Rigidbody>().velocity = direction * speed;
            }
        }
        else
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    IEnumerator Shake()
    {
        Vector3 orignialPos = mainCam.transform.localPosition;

        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            mainCam.transform.localPosition += new Vector3(x, y, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }
        mainCam.transform.localPosition = orignialPos;
    }
}
