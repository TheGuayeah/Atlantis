using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CelulaManager_Script : MonoBehaviour
{

    private GameObject VirtualPark;
    private Game_Manager GM;
    public GameObject Aspas, EnergyTube, Obi, Fluorescentes;
    public List<GameObject> Particles, SFX, LuzColumna;
    public VR_Manager HTC;
    public List<Light> LightColummna;
    public List<float> TimeActiveParticles;
    public float RotationSpeed, Multiply, ParticlesInitTime, PortalLoad, CelulaLuz_Cargado, 
        CelulaLuz_inicio, LuzColumna_Alta, LuzColumna_Baja;
    private float LightDown_Columna, AlphaPlane;
    public bool InitPortal, Glittering;
    private Obi_Script ObiScript;
    public List<bool> Scene_SFX;
    public Animator Cristalera, Silla;
    public AudioSource Compuertas;
    public Light LuzCentral;
    public GameObject SillaP;
    private bool portalLoadYES;
    private AsyncOperation async;
    public Image Desvanecimiento;


    // Use this for initialization
    void Start()
    {
        AlphaPlane = 1;
        async = SceneManager.LoadSceneAsync(1);
        async.allowSceneActivation = false;
        VirtualPark = GameObject.Find("VirtualPark");
        GM = GameObject.Find("GameManager").GetComponent<Game_Manager>();
        HTC = GameObject.Find("[CameraRig]").GetComponent<VR_Manager>();
        ObiScript = Obi.GetComponent<Obi_Script>();
        ObiScript.Manager = transform.GetComponent<CelulaManager_Script>();



        portalLoadYES = true;

        for (int i = 0; i < 8; i++)
        {
            if (i == 0)
            {
                Scene_SFX.Add(true);
            }
            else
            {
                Scene_SFX.Add(false);
            }
        }

        for (int i = 0; i < LuzColumna.Count; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                LuzColumna[i].transform.GetChild(j).transform.GetComponent<Light>().intensity = LuzColumna_Alta;
                LightColummna.Add(LuzColumna[i].transform.GetChild(j).transform.GetComponent<Light>());
            }
        }

    }

    // Update is called once per frame
    void Update()
    { 
        if(ObiScript.State==Obi_Script.ObiStates.REGRESO)
        {
            AlphaPlane -= Time.deltaTime*0.5f;
            Color NewAlpha = new Color(Desvanecimiento.color.r,
       Desvanecimiento.color.g,
       Desvanecimiento.color.b, AlphaPlane);
            Desvanecimiento.color = NewAlpha; 
            if (Desvanecimiento.color.a<=0)
            {
                Desvanecimiento.color=new Color(Desvanecimiento.color.r,
       Desvanecimiento.color.g,
       Desvanecimiento.color.b, 0);
                AlphaPlane = 0;
            }
        }
        if (SillaP.transform.localPosition.x <= -10f)
        {
            AlphaPlane += Time.deltaTime * 1.5f;
            Color NewAlpha = new Color(Desvanecimiento.color.r,
                  Desvanecimiento.color.g,
                  Desvanecimiento.color.b, AlphaPlane);
            Desvanecimiento.color = NewAlpha;
            if (Desvanecimiento.color.a >= 1)
            {
                async.allowSceneActivation = true;
            }
            //SceneManager.LoadScene(1);
        }
        if (!Scene_SFX[6] && ObiScript.ObiSaltoTime >= 37.5f)
        {
            if (portalLoadYES)
            {
                PortalLoad += Time.deltaTime;
                if (PortalLoad >= 4)
                {
                    GameObject NewSFX = (GameObject)Instantiate(SFX[4]);
                    NewSFX.transform.SetParent(VirtualPark.transform);
                    Scene_SFX[6] = true;
                    portalLoadYES = false;
                }
            }

        }
        else
        {
            PortalLoad = 0;

        }
        Aspas.transform.Rotate(Vector3.up * RotationSpeed * Time.deltaTime, Space.World);
        Fluorescentes.transform.Rotate(Vector3.up * 5 * Time.deltaTime, Space.World);

        if (!InitPortal)
        {
            ParticlesInitTime = 0;

            if (EnergyTube.transform.localScale == new Vector3(0.3f, 2.269398f, 0.3f))
            {
                Glittering = false;
            }

            if (EnergyTube.transform.localScale == new Vector3(0.2f, 2.269398f, 0.2f))
            {
                Glittering = true;
            }

            if (Glittering)
            {
                EnergyTube.transform.localScale = Vector3.MoveTowards(EnergyTube.transform.localScale, new Vector3(0.3f, 2.269398f, 0.3f), 0.3f * Time.deltaTime);
            }

            else
            {
                EnergyTube.transform.localScale = Vector3.MoveTowards(EnergyTube.transform.localScale, new Vector3(0.2f, 2.269398f, 0.2f), 0.3f * Time.deltaTime);
            }
        }

        if (InitPortal)
        {
            float Intens = LuzCentral.intensity;
            ParticlesInitTime += Time.deltaTime;
            EnergyTube.transform.localScale = Vector3.MoveTowards(EnergyTube.transform.localScale, new Vector3(0.9f, 2.269398f, 0.9f), 0.2f * Time.deltaTime);
            if (Intens < CelulaLuz_Cargado)
            {
                Intens = CelulaLuz_inicio + ((CelulaLuz_Cargado - CelulaLuz_inicio) * (ParticlesInitTime / CelulaLuz_Cargado));
                LuzCentral.intensity = Intens;
            }
            else
            {
                Intens = CelulaLuz_Cargado;
                LuzCentral.intensity = Intens;
            }
            RotationSpeed = Mathf.MoveTowards(RotationSpeed, 200, Multiply * Time.deltaTime);

        }

        else
        {
            RotationSpeed = Mathf.MoveTowards(RotationSpeed, 10, Multiply * Time.deltaTime);
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow) || HTC.start == true) //Activa el evento del salto
        {
            Obi.transform.position = ObiScript.STOPS[3].transform.position;
            ObiScript.Salto = true;
            ObiScript.ChangeState(Obi_Script.ObiStates.INICIO_SALTO);
            HTC.start = false;
        }


        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            ObiScript.Salto = false;
            InitPortal = false;
            EnergyTube.transform.localScale = Vector3.MoveTowards(EnergyTube.transform.localScale, new Vector3(0.2f, 2.269398f, 0.2f), 0.5f * Time.deltaTime);

            for (int Num = 0; Num < Particles.Count; Num++)
            {
                StartCoroutine(ActivarParticulas(Particles[Num], 5, false));
            }

            //EL GRAN FOR DE SERGIO

            for (int Num = 0; Num < Particles.Count; Num++)
            {
                if (Particles[Num] != null)
                {
                    ParticleSystem AllParticles = Particles[Num].GetComponent<ParticleSystem>();
                    var Emision = AllParticles.emission;
                    Emision.enabled = false;
                }
            }
        }
    }

    IEnumerator ActivarParticulas(GameObject ParticleObj, float Time, bool Acive)
    {
        yield return new WaitForSeconds(Time);
        ParticleObj.SetActive(Acive);
    }

    public void InstantiateSFX()//Activa sonidos y particulas en los diferentes estados en los que se encuentra Obi
    {
        if (Scene_SFX[0])
        {
            GM.ObiLab[4].enabled = false;
            GM.ObiLab[0].enabled = true;
            Scene_SFX[0] = false;
        }
        if (Scene_SFX[1])
        {
            GM.ObiLab[1].enabled = true;
        }
        if (Scene_SFX[2])
        {
            GM.ObiLab[2].enabled = true;
            Scene_SFX[2] = false;
        }

        if (Scene_SFX[3])
        {
            GameObject NewSFX = (GameObject)Instantiate(SFX[3]);
            NewSFX.transform.SetParent(VirtualPark.transform);
            GM.ObiLab[3].enabled = true;
            InitPortal = true;
            StartCoroutine(ActivarParticulas(Particles[0], TimeActiveParticles[0], true));
            ParticleSystem AllParticles = Particles[0].GetComponent<ParticleSystem>();
            var Emision = AllParticles.emission;
            Emision.enabled = true;
            Scene_SFX[3] = false;
        }
        if (Scene_SFX[4])
        {

            Cristalera.SetBool("CloseDoor", true);
            Compuertas.enabled = true;
            Scene_SFX[4] = false;

        }
        if (Scene_SFX[5])
        {
            for (int Num = 0; Num < Particles.Count; Num++)
            {
                StartCoroutine(ActivarParticulas(Particles[Num], TimeActiveParticles[Num], true));

                if (Particles[Num] != null)
                {
                    ParticleSystem AllParticles = Particles[Num].GetComponent<ParticleSystem>();
                    var Emision = AllParticles.emission;
                    Emision.enabled = true;
                }
            }
            GameObject NewSFX = (GameObject)Instantiate(SFX[5]);
            NewSFX.transform.SetParent(VirtualPark.transform);
            Scene_SFX[6] = false;
            Scene_SFX[5] = false;
        }
        if (Scene_SFX[6])

        {
            Scene_SFX[6] = false;
        }
    }
    public void ColumnaLightDown()
    {
        LightDown_Columna += Time.deltaTime;
        if (LightColummna[LightColummna.Count - 1].intensity > LuzColumna_Baja)
        {
            for (int i = 0; i < LightColummna.Count; i++)
            {
                LightColummna[i].intensity = LuzColumna_Alta - ((LuzColumna_Alta - LuzColumna_Baja) * (LightDown_Columna / 3));
            }
        }
    }//Baja la intensidad de las luces de las columnas
}

