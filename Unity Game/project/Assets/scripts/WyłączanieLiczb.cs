using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WyłączanieLiczb : MonoBehaviour
{
    public GameObject[] tekst;
    // Start is called before the first frame update
    void Start()
    {
        
            
    }

    // Update is called once per frame
    void Update()
    {
        


        for(int i = 0; i < 3; i++)
        {
            if (Uderzenie4.czyUderzono == 1)
            {
                tekst[i].SetActive(false);

            }
        }

        


    }
}
