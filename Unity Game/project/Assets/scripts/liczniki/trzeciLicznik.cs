using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class trzeciLicznik : MonoBehaviour
{
    public Text licznik3;
    public static int liczbaItem�w3 = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        liczbaItem�w3 = drugiLicznik.liczbaItem�w2;
        licznik3.text = liczbaItem�w3.ToString();
    }
}
