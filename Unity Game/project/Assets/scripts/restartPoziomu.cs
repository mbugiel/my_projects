using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class restartPoziomu : MonoBehaviour
{
    public Text text;
    public static bool odnowienie = false;
    int potem = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (potem == 1)
        {
            odnowienie = false;
            potem = 0;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            
            text.text = "0";
            licznikitemów.liczbaItemów = 0;


            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            odnowienie = true;
            potem = 1;
        }
    }
}
