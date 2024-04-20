using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class menuSkrypt : MonoBehaviour
{
    
    public void Poziom£atwy()
    {
        SceneManager.LoadScene(1);
    }

    public void PoziomTrudny()
    {
        SceneManager.LoadScene(2);
    }

    public void Poziom2()
    {
        SceneManager.LoadScene(3);
    }

    public void Wyjdz()
    {
        Application.Quit();
    }
}
