using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class drugiLicznik : MonoBehaviour
{
    public static int liczbaItemów2;
    public Text licznik2;
    // Start is called before the first frame update



    public void zebrano()
    {
        liczbaItemów2++;
        licznik2.text = liczbaItemów2.ToString();
    }
    void Start()
    {
        liczbaItemów2 = NewBehaviourScript.IlePoSkonczeniuPoziomu;
        licznik2.text = liczbaItemów2.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (kolajder.czyZresetowaæ == true)
        {

            liczbaItemów2 = NewBehaviourScript.IlePoSkonczeniuPoziomu;
            licznik2.text = liczbaItemów2.ToString();
        }
    }

}
