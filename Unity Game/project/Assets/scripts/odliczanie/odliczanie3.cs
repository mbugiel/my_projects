using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class odliczanie3 : MonoBehaviour
{
    public GameObject tekst;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Uderzenie2.czyUderzono == 1)
        {
            tekst.SetActive(true);

        }
    }
}
