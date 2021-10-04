using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public bool canMove;
    public float speed;
    float xval;
    float yval;
    Vector3 moveVal;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Time.deltaTime);
            if (canMove&& !GameObject.FindGameObjectWithTag("Manager").GetComponent<SwapTimeManager>().paused)
            {
                xval = Input.GetAxis("Horizontal");
                yval = Input.GetAxis("Vertical");
            }
            else
            {
                xval = 0;
                yval = 0;
            }
        Vector2 moveNorm;
        if (Mathf.Abs( xval )+ Mathf.Abs( yval) > 1)
        {
             moveNorm = new Vector2(xval, yval).normalized;
        }
        else
        {
             moveNorm = new Vector2(xval, yval);
        }
        
            moveVal = new Vector3(moveNorm.x * speed, 0f, moveNorm.y * speed);
        
        

       
    }
   
    private void FixedUpdate()
    {
        
            GetComponent<Rigidbody>().velocity = moveVal;
        
    }
}
