using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class trzeciLicznik : MonoBehaviour
{
    public Text licznik3;
    public static int liczbaItemów3 = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        liczbaItemów3 = drugiLicznik.liczbaItemów2;
        licznik3.text = liczbaItemów3.ToString();
    }
}
