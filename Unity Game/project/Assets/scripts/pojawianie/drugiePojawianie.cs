using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drugiePojawianie : MonoBehaviour
{
    public GameObject[] klocki;


    // Update is called once per frame
    void Update()
    {
        if (NewBehaviourScript.resetowanieLicznika == 1)
        {

            for (int i = 0; i < klocki.Length; i++)
            {
                klocki[i].SetActive(true);
            }


        }

    }
}
