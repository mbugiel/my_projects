using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Uderzenie4 : MonoBehaviour
{
    public static int czyUderzono = 0;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("GameObject1 collided with " + col.tag);
        if (col.tag == "laser")
        {
            czyUderzono = 1;
            

        }

    }
}
