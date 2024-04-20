using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aktywowanie2 : MonoBehaviour
{
    public GameObject poziom2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (NewBehaviourScript.przyciskPoziom == 1)
        {
            poziom2.SetActive(true);
        }
    }
}
