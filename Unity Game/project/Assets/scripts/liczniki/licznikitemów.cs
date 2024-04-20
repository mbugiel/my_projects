using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class licznikitemów : MonoBehaviour
{
   
    public static int liczbaItemów;
    public Text licznik;
    // Start is called before the first frame update

    

    public void zebranoItem()
    {
        liczbaItemów++;
        licznik.text = liczbaItemów.ToString();
    }
    void Start()
    {
       
        licznik.text = liczbaItemów.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if(NewBehaviourScript.resetowanieLicznika == 1)
        {
            
            liczbaItemów = 0;
            licznik.text = liczbaItemów.ToString();
        }
    }

    
}
