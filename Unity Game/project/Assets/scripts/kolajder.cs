using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kolajder : MonoBehaviour
{
    public static bool czyZresetować = false;
    int poResecie = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            czyZresetować = true;
           
        }
    }
}
