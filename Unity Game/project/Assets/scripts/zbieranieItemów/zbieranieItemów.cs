using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zbieranieItemów : MonoBehaviour
{
    private licznikitemów licznikI;
    // Start is called before the first frame update
    void Start()
    {
        licznikI = GameObject.Find("Manager").GetComponent<licznikitemów>();
        if (licznikI == null)
        {
            Debug.LogError("LicznikItemów nie znaleziony");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("GameObject1 collided with " + col.name);
        if (col.gameObject.name == "Idle (1)")
        {
            this.gameObject.SetActive(false);
            licznikI.zebranoItem();
        }
            
    }
}
