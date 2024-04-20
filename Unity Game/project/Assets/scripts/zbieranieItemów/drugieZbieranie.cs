using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drugieZbieranie : MonoBehaviour
{
    private drugiLicznik licznikD;
    // Start is called before the first frame update
    void Start()
    {
        licznikD = GameObject.Find("Manager").GetComponent<drugiLicznik>();
        if (licznikD == null)
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
            kolajder.czyZresetowaæ = false;
            this.gameObject.SetActive(false);
            licznikD.zebrano();
        }

    }
}
