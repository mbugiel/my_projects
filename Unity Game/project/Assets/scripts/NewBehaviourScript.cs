using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class NewBehaviourScript : MonoBehaviour


{

    public static int IlePoSkonczeniuPoziomu = 0;
    public static int IlePoDrugimPoziomie = 0;




    public static int resetowanieLicznika = 0;
    public static int dolicznika = 0;
    private Animator anim;

    public Transform testerPolozeniaPostaci;

    public LayerMask maskaWarstwyDoTestowania;

    private bool naPlatformie = true;

    public bool kierunekWprawo = true;
     private Rigidbody2D rbBody;

    private Vector3 respawnPoint;
    public GameObject FallDetector; 
    // Start is called before the first frame update

    public float silaSkoku = 500;
    public float silaRuchu = 5;

    public static int przyciskPoziom = 0;

    void Flip()
    {
        kierunekWprawo = !kierunekWprawo;
        Vector3 skala = gameObject.transform.localScale;
        skala.x *= -1;
        gameObject.transform.localScale = skala;
    }

    void Start()
    {
       

        anim = GetComponent<Animator>();
        rbBody = GetComponent<Rigidbody2D>();
        respawnPoint = transform.position;
    }

    // Update is called once per frame
    int pokolizji = 0;

    void Update()
    {
        

        
        
           

       
        

        float ruchpoziomy = Input.GetAxis("Horizontal");
        float ruchpionowy = Input.GetAxis("Vertical");

        rbBody.velocity = new Vector2(ruchpoziomy * silaRuchu, rbBody.velocity.y);

        Vector3 skala = gameObject.transform.localScale;

        if(ruchpoziomy < 0 && kierunekWprawo == true)
        {
            Flip();
        }

        if(ruchpoziomy > 0 && kierunekWprawo == false)
        {
            Flip();
        }

        naPlatformie = Physics2D.OverlapCircle((Vector2)testerPolozeniaPostaci.position, 0.1f, maskaWarstwyDoTestowania);

        

        if (Input.GetKeyDown(KeyCode.Space) && naPlatformie == true)
        {
           
            rbBody.AddForce(new Vector2(0f, silaSkoku));
            
        }

       

        
        



        

        if (ruchpoziomy < 0 || ruchpoziomy > 0)
        {
            anim.SetInteger("FazaAnimacji", 1);
        }
        if(ruchpoziomy == 0 && ruchpionowy == 0)
        {
            anim.SetInteger("FazaAnimacji", 0);
        }
        anim.SetBool("skok", naPlatformie);

        anim.SetFloat("yVelocity", rbBody.velocity.y);

        FallDetector.transform.position = new Vector2(transform.position.x, FallDetector.transform.position.y);
        
        if (pokolizji == 1)
        {
            resetowanieLicznika = 0;
            pokolizji = 0;
        }
    }

   
    void OnTriggerEnter2D(Collider2D collision)
    {
       


        if (collision.tag == "FallDetector")
        {
            transform.position = respawnPoint;

            resetowanieLicznika = 1;
            pokolizji = 1;
        }

        if (collision.tag == "ZagrajPonownie")
        {
            SceneManager.LoadScene(0);
        }

        if (collision.tag == "WyjdŸzgry")
        {
            Application.Quit();
        }






        if (collision.tag == "Checkpoint")
        {
            respawnPoint = transform.position;
        }
        if (collision.tag == "finish")
        {
            IlePoDrugimPoziomie = drugiLicznik.liczbaItemów2;
            przyciskPoziom = 1;
            if(SceneManager.sceneCountInBuildSettings > SceneManager.GetActiveScene().buildIndex + 1)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else
            {
                SceneManager.LoadScene(0);
            }

        }

        if (collision.tag == "zmianaMapy")
        {

            IlePoSkonczeniuPoziomu = licznikitemów.liczbaItemów;

            SceneManager.LoadScene(3);
           
        }

            if (collision.tag == "poziom2")
        {
            SceneManager.LoadScene(3);
           

        }

        
    }

   

}
