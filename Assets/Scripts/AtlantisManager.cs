using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class AtlantisManager : MonoBehaviour
{

    public bool FixRotation, ChangeSpeedDown1, ChangeSpeedUp1, ChangeSpeedUp2, Acel_Bool, MonsterBool;
    public float MonsterTimer, ChangeSpeedTime, Acel_Timer, TimeGame, EndTime, waterOut, AlphaPlane, BossSound;
    public List<GameObject> BoxEvents, Fauna, OptimizationGroups, UnderWaterOptimization, ObiPoints;
    public List<AudioSource> Obi_Dialogos;
    public List<float> SpeedPlayer;
    public Animator Boss, Shell;
    public GameObject Player, EventsParent, InmersionEffect, RockEventObj, Niebla, Teleport;
    public SplineAnimatorCamera PlayerScript;
    public Vector3 PlayerRotation;
    public Game_Manager GM;
    public Obi_Script OM;
    public Image Desvanecimiento;
    private Scene CurrentScene;
    private bool End, WaterOut, Rugido;
    private string Escena;
    private AsyncOperation async;
    public List<AudioSource> Cascada_Sound, Boss_Sound;

    // Use this for initialization
    void Start()
    {
        AlphaPlane = 1;
        async = SceneManager.LoadSceneAsync(0);
        async.allowSceneActivation = false;
        Physics.gravity = new Vector3(0, -9.8f, 0);
        GM = GameObject.Find("GameManager").GetComponent<Game_Manager>();

        FixRotation = false;
        PlayerScript = Player.transform.GetComponent<SplineAnimatorCamera>();
        for (int i = 0; i < EventsParent.transform.childCount; i++)
        {
            BoxEvents.Add(EventsParent.transform.GetChild(i).transform.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Rugido)
        {
            BossSound += Time.deltaTime;
            if (BossSound >= 6.15f)
            {
                Boss_Sound[0].enabled = true;
            }
            if (BossSound >= 8.5f)
            {
                Boss_Sound[2].enabled = true;
            }
            if (BossSound >= 9.7f)
            {
                Boss_Sound[3].enabled = true;
            }
            if (BossSound >= 11.5f)
            {
                Boss_Sound[1].enabled = true;
            }
        }
        if (WaterOut)
        {
            waterOut += Time.deltaTime;
            if (waterOut >= 1.3f)
            {
                RenderSettings.fogDensity = 0.002f;
                Obi_Dialogos[9].enabled = true;
                WaterOut = false;
            }
        }
        if (End)
        {
            EndTime += Time.deltaTime;
            if (EndTime > 1.5f)
            {
                if (GM.Atlantida[9].enabled)
                {
                    if (EndTime >= 2.0f)
                    {
                        AlphaPlane += Time.deltaTime;
                        Color NewAlpha = new Color(Desvanecimiento.color.r,
                              Desvanecimiento.color.g,
                              Desvanecimiento.color.b, AlphaPlane);
                        Desvanecimiento.color = NewAlpha;
                        if (Desvanecimiento.color.a >= 1)
                        {
                            async.allowSceneActivation = true;
                        }
                    }
                }
            }
        }
        else
        {
            AlphaPlane -= Time.deltaTime * 0.5f;
            Color NewAlpha = new Color(Desvanecimiento.color.r,
       Desvanecimiento.color.g,
       Desvanecimiento.color.b, AlphaPlane);
            Desvanecimiento.color = NewAlpha;
            if (Desvanecimiento.color.a <= 0)
            {
                Desvanecimiento.color = new Color(Desvanecimiento.color.r,
       Desvanecimiento.color.g,
       Desvanecimiento.color.b, 0);
                AlphaPlane = 0;
            }
        }


        TimeGame += Time.deltaTime;

        if (MonsterBool)
        {
            MonsterTimer += Time.deltaTime;
        }

        if (Boss.GetBool("Start") == true && MonsterTimer > 5f)
        {
            GM.Atlantida[8].enabled = true;

            if (MonsterTimer >= 9)
            {
                transform.GetComponent<SplineAnimatorCamera>().speed = -0.0045f;
                transform.GetComponent<SplineAnimatorCamera>().enabled = true;
                MonsterBool = false;
                MonsterTimer = 0;
            }
        }

        if (Acel_Bool)
        {
            Acel_Timer += Time.deltaTime;
        }

        if (Acel_Timer > 2.7f)
        {
            Obi_Dialogos[1].enabled = true;
            Acel_Bool = false;
            Acel_Timer = 0;
        }

        if (ChangeSpeedDown1)
        {
            ChangeSpeedTime += Time.deltaTime;
            PlayerScript.speed = 0.005f + ((0.026f - (0.005f)) * (ChangeSpeedTime / 5));

            if (PlayerScript.speed >= 0.026f)
            {
                PlayerScript.speed = 0.026f;
                ChangeSpeedTime = 0;
                ChangeSpeedDown1 = false;
            }
        }

        if (ChangeSpeedUp1)
        {
            ChangeSpeedTime += Time.deltaTime;
            PlayerScript.speed = 0.026f - ((0.026f - (0.005f)) * (ChangeSpeedTime / 3f));

            if (PlayerScript.speed >= 0.005f)
            {
                PlayerScript.speed = 0.005f;
                ChangeSpeedTime = 0;
                ChangeSpeedUp1 = false;
            }
        }

        if (ChangeSpeedUp2)
        {
            ChangeSpeedTime += Time.deltaTime;
            PlayerScript.speed = 0.005f + ((0.007f - 0.005f) * (ChangeSpeedTime / 6f));

            if (PlayerScript.speed >= 0.007f)
            {
                PlayerScript.speed = 0.007f;
                ChangeSpeedTime = 0;
                ChangeSpeedUp2 = false;
            }
        }
    }

    public void ActiveScaredFishes()
    {
        for (int i = 3; i < Fauna.Count; i++)
        {
            if (i != 19 || i != 20)
            {
                Fauna[i].GetComponent<SplineAnimator>().speed = 0.12f;
            }
        }
    }

    public void OnTriggerEnter(Collider Col)
    {

        if (Col.name == "BirdsLook" || Col.name == "Water" || Col.name == "Algo_Terrible" || Col.name == "Bioluminiscencia")
        {
            Fauna[19].GetComponent<SplineAnimator>().speed = 0.03f;
            OM.GetComponent<Obi_Script>().ChangeState(Obi_Script.ObiStates.EVENT1);

        }

        if (Col.name == "Desaceleracion1")
        {
            GM.Atlantida[0].enabled = true;
            PlayerScript.speed = 0.005f;
        }

        if (Col.name == "Aceleracion1")
        {
            Fauna[0].GetComponent<SplineAnimator>().speed = 0.013f;
            Acel_Bool = true;
            ChangeSpeedDown1 = true;
            FixRotation = true;
        }

        if (Col.name == "OEVENT2" || Col.name == "OEVENT6" || Col.name == "Desboquear_Camara" || Col.name == "ScaredFishes")
        {
            OM.GetComponent<Obi_Script>().ChangeState(Obi_Script.ObiStates.LOOKATFRONT);
        }

        if (Col.name == "OEVENT3" || Col.name == "OEVENT5" || Col.name == "OEVENT13")
        {
            OM.GetComponent<Obi_Script>().ChangeState(Obi_Script.ObiStates.LOOKATCAMERA);
        }

        if (Col.name == "OEVENT7")
        {
            OM.GetComponent<Obi_Script>().ChangeState(Obi_Script.ObiStates.CHANGESIDE);
        }

        if (Col.name == "OEVENT8")
        {
            OM.GetComponent<Obi_Script>().ChangeState(Obi_Script.ObiStates.LOOKATTHING);
        }

        if (Col.name == "OEVENT9" || Col.name == "OEVENT11")
        {
            OM.GetComponent<Obi_Script>().ChangeState(Obi_Script.ObiStates.LOOKATFRONT2);
        }

        if (Col.name == "OEVENT10")
        {
            OM.GetComponent<Obi_Script>().ChangeState(Obi_Script.ObiStates.MONSTER);
        }

        if (Col.name == "OEVENT12")
        {
            OM.GetComponent<Obi_Script>().ChangeState(Obi_Script.ObiStates.CAIDA);
        }

        if (Col.name == "OBIUP")
        {
            OM.GetComponent<Obi_Script>().ChangeState(Obi_Script.ObiStates.OBIUP);
        }

        if (Col.name == "Desaceleracion2")
        {
            ChangeSpeedUp1 = true;
            Niebla.SetActive(false);
            GM.Atlantida[2].enabled = true;
            InmersionEffect.SetActive(true);
            RenderSettings.fogDensity = 0.02f;
            for (int i = 0; i < Cascada_Sound.Count; i++)
            {
                Cascada_Sound[i].volume = 0.1f;
            }
        }

        if (Col.name == "Fuente_Energia")
        {
            FixRotation = false;
            GM.Atlantida[2].enabled = true;
        }

        if (Col.name == "Desboquear_Camara")
        {
            ChangeSpeedUp2 = true;
            FixRotation = false;
        }

        if (Col.name == "FinSpline")
        {
            OM.GetComponent<Obi_Script>().ChangeState(Obi_Script.ObiStates.BALL);
            MonsterBool = true;
            Rugido = true;
            Boss.SetBool("Start", true);
            transform.GetComponent<SplineAnimatorCamera>().enabled = false;
        }

        if (Col.name == "Vamos_Por_ese_camino")
        {
            Shell.SetBool("Open", true);
            GM.Atlantida[3].enabled = true;
            Fauna[20].SetActive(true);
        }

        if (Col.name == "Bioluminiscencia")
        {
            GM.Atlantida[4].enabled = true;
        }

        if (Col.name == "No_Estamos_Solos")
        {
            Boss.SetBool("Cola", true);
            GM.Atlantida[6].enabled = true;
        }

        if (Col.name == "Algo_Terrible")
        {
            RockEventObj.SetActive(true);
            GM.Atlantida[5].enabled = true;
            for (int i = 0; i < Cascada_Sound.Count; i++)
            {
                Cascada_Sound[i].enabled = false;
            }
        }

        if (Col.name == "Salida_Agua")
        {
            //RenderSettings.fogDensity = 0.002f;
            //Obi_Dialogos[9].enabled = true; 
            WaterOut = true;
            Fauna[19].SetActive(true);
            Fauna[19].GetComponent<SplineAnimator>().speed = 0.03f;
        }

        if (Col.name == "Energia_Sound")
        {
            Obi_Dialogos[10].enabled = true;
        }

        if (Col.name == "Water")
        {

            for (int i = 0; i < UnderWaterOptimization.Count; i++)
            {
                UnderWaterOptimization[i].SetActive(false);
            }

            Fauna[1].GetComponent<SplineAnimator>().speed = 0.022f;
            Fauna[2].GetComponent<SplineAnimator>().speed = 0.022f;
            Obi_Dialogos[11].enabled = true;

        }

        if (Col.name == "Water_Out")
        {
            Obi_Dialogos[11].enabled = false;
        }

        if (Col.name == "FinRecorrido")
        {
            if (End)
            {
                //SceneManager.LoadScene(0);
                //    if (EndTime > 2f)
                //    {
                //        GM.ObiLab[9].enabled = true;
                //        if (GM.ObiLab[9].enabled)
                //        {
                //            if(EndTime >= 3.5f)
                //            {
                //                SceneManager.LoadScene(0);
                //            }
                //        }
                //    }
            }
        }

        if (Col.name == "ActiveEvent1")
        {
            for (int i = 0; i < OptimizationGroups.Count; i++)
            {
                if (i == 7 || i == 10 || i == 15 || i == 19)
                {
                    OptimizationGroups[i].SetActive(true);
                }
                else if (i != 19)
                {
                    OptimizationGroups[i].SetActive(false);
                }
            }

            Fauna[19].SetActive(false);
        }

        if (Col.name == "ActiveEvent2")
        {
            for (int i = 0; i < OptimizationGroups.Count; i++)
            {
                if (i == 0 || i == 1 || i == 2 || i == 3 || i == 8 || i == 11 || i == 16 || i == 19)
                {
                    OptimizationGroups[i].SetActive(true);
                }
            }

            Fauna[17].GetComponent<SplineAnimator>().speed = 0.12f;
            Fauna[18].GetComponent<SplineAnimator>().speed = 0.12f;
        }

        if (Col.name == "ActiveEvent3")
        {
            for (int i = 0; i < OptimizationGroups.Count; i++)
            {
                if (i == 4 || i == 5 || i == 6 || i == 12 || i == 17 || i == 20)
                {
                    OptimizationGroups[i].SetActive(true);
                }
                else if (i == 7 || i == 9 || i == 10 || i == 13 || i == 14 || i == 15 || i == 18)
                {
                    OptimizationGroups[i].SetActive(false);
                }
            }

            Fauna[17].SetActive(false);
            Fauna[18].SetActive(false);
        }

        if (Col.name == "ActiveEvent4")
        {
            for (int i = 0; i < OptimizationGroups.Count; i++)
            {
                if (i == 9 || i == 13 || i == 18)
                {
                    OptimizationGroups[i].SetActive(true);
                }
            }
        }

        if (Col.name == "ActiveEvent5")
        {
            for (int i = 0; i < UnderWaterOptimization.Count; i++)
            {
                UnderWaterOptimization[i].SetActive(true);
            }
        }

        if (Col.name == "ScaredFishes")
        {
            ActiveScaredFishes();
        }

        if (Col.tag == "Teleport")
        {
            End = true;
            GM.Atlantida[9].enabled = true;
        }
    }

    public void OnTriggerExit(Collider Col)
    {
        if (Col.name == "FinRecorrido")
        {
            Teleport.SetActive(true);
        }
    }

    public void fixRotation(Quaternion rotation)
    {
        transform.rotation = rotation;
    }


}
