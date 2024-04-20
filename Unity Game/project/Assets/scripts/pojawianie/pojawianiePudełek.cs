using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pojawianiePudełek : MonoBehaviour
{

    public GameObject[] klocki;
    

    // Update is called once per frame
    void Update()
    {
        if (kolajder.czyZresetować == true)
        {

            for (int i = 0; i < klocki.Length; i++)
            {
                klocki[i].SetActive(true);
            }


        }
        
    }
}
