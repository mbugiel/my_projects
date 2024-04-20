using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bariera : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("GameObject1 collided with " + col.tag);
        if (col.tag == "laser")
        {
            Destroy(this.gameObject);


        }

    }
}
