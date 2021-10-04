using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockLogic : MonoBehaviour
{
    bool isShrink;
    public float shrinkSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isShrink)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 5f * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.tag);
        if (other.tag == "Hole")
        {
            if (!isShrink)
            {
                GameObject.FindGameObjectWithTag("Manager").GetComponent<ScoreManager>().boxScoreAdd();
            }
            isShrink = true;
            
            Destroy(this.gameObject, 3);
        }
        
    }
}
