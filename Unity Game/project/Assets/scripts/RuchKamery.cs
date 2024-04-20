using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuchKamery : MonoBehaviour
{
    public GameObject bohater;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 nowaPozycjaKamery = new Vector3(bohater.transform.position.x, bohater.transform.position.y, transform.position.z);

        transform.position = nowaPozycjaKamery;
    }
}
