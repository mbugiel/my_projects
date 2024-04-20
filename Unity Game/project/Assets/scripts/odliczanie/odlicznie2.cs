using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class odlicznie2 : MonoBehaviour
{

    public GameObject tekst;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Uderzenie.czyUderzono == 1)
        {
            tekst.SetActive(true);

        }
    }
}
