using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class drugiLicznik : MonoBehaviour
{
    public static int liczbaItem�w2;
    public Text licznik2;
    // Start is called before the first frame update



    public void zebrano()
    {
        liczbaItem�w2++;
        licznik2.text = liczbaItem�w2.ToString();
    }
    void Start()
    {
        liczbaItem�w2 = NewBehaviourScript.IlePoSkonczeniuPoziomu;
        licznik2.text = liczbaItem�w2.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (kolajder.czyZresetowa� == true)
        {

            liczbaItem�w2 = NewBehaviourScript.IlePoSkonczeniuPoziomu;
            licznik2.text = liczbaItem�w2.ToString();
        }
    }

}
