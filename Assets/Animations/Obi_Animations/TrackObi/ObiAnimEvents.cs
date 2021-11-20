using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ObiAnimEvents : MonoBehaviour
{
    public Animator Lab, Vagoneta;
    private Animator Anim;
    public GameObject Portal,Holograma, Huevo;

    public float SpeedTime = 5;

    void Start()
    {
        Anim = transform.GetChild(0).GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<Animator>().enabled = true;
        }

        if (Input.GetKey(KeyCode.D))
        {
            Time.timeScale = SpeedTime;
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            Time.timeScale = 1;
        }
    }


    public void ActiveLocura()
    {
        Anim.SetBool("Locura", true);
        print("Locura:on");
    }
    public void DesactiveLocura()
    {
        Anim.SetBool("Locura", false);
        print("Locura:off");
    }

    public void ActiveSaludo()
    {
        Anim.SetBool("Saludo",true);
    }
    public void DesactiveSaludo()
    {
        Anim.SetBool("Saludo", false);
    }

    public void ActiveDesplazamiento()
    {
        Anim.SetBool("Desplazamiento", true);
        print("Levita:on");
    }
    public void DesactiveDesplazamiento()
    {
        Anim.SetBool("Desplazamiento", false);
        print("Levita:off");
    }

    public void ActiveAsustado()
    {
        Anim.SetBool("Asustado", true);
    }
    public void DesactiveAsustado()
    {
        Anim.SetBool("Asustado", false);
    }

    public void ActiveEsese()
    {
        Anim.SetBool("Es ese", true);
    }
    public void DesactiveEsese()
    {
        Anim.SetBool("Es ese", false);
    }
    public void ActiveSuspiro()
    {
        Anim.SetBool("Suspiro", true);
        
    }
    public void DesactiveSuspiro()
    {
        Anim.SetBool("Suspiro", false);
       
    }
    public void ActiveDespedida()
    {
        Anim.SetBool("Despedida", true);

    }
    public void DesactiveDespedida()
    {
        Anim.SetBool("Despedida", false);

    }
    public void ActiveSeñalar()
    {
        Anim.SetBool("Señalar", true);

    }
    public void DesactiveSeñalar()
    {
        Anim.SetBool("Señalar", false);

    }


    public void ActiveLab()
    {
        Lab.enabled = true;
    }

    public void ActiveVagoneta()
    {
        Vagoneta.enabled = true;
    }

    public void ActivePortal()
    {
        Portal.SetActive(true);
    }
    public void ActiveHolograme()
    {
        Holograma.SetActive(true);
    }
    public void DesactiveHolograme()
    {
        Holograma.SetActive(false);
    }

    public void ActiveHuevoHolograme()
    {
        Huevo.SetActive(true);
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
