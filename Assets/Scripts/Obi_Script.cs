using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;

public class Obi_Script : MonoBehaviour
{

    [Range(0, 1)]
    public float FloatingSpeed;
    public enum ObiStates { RUTA1, STOP1, RUTA2, STOP2, RUTA3, STOP3, AVISO, INICIO_SALTO, REGRESO, BALL, INICIOATL, EVENT1, LOOKATFRONT, LOOKATFRONT2, LOOKATTHING, LOOKATCAMERA, CHANGESIDE, CAIDA, MONSTER, OBIUP, OBIUP2, OBIDOWN, OBIDOWN2 }
    public ObiStates State;
    public GameObject ObiAnimObj, VR_CAM, EnergyBall;
    public AudioSource Movidito;
    public CelulaManager_Script Manager;
    private Game_Manager GM;
    public List<GameObject> Vuelta, R1, R2, R3, STOPS, RJump, ATLWayPoints;
    public float Speed, RSpeed, Radius, StopClock, ObiSaltoTime, StayTime;
    private int CureentWayPoint;
    private bool Floating, Stay;
    public bool Salto, Atlantis;
    public int ObiSalto;
    public Animator ObiAnim;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (ObiAnimObj.transform.localPosition == new Vector3(ObiAnimObj.transform.localPosition.x, 0.10f, ObiAnimObj.transform.localPosition.z))
        {
            Floating = false;
        }

        if (ObiAnimObj.transform.localPosition == new Vector3(ObiAnimObj.transform.localPosition.x, -0.10f, ObiAnimObj.transform.localPosition.z))
        {
            Floating = true;
        }

        if (Floating)
        {
            ObiAnimObj.transform.localPosition = Vector3.MoveTowards(ObiAnimObj.transform.localPosition, new Vector3(ObiAnimObj.transform.localPosition.x, 0.10f, ObiAnimObj.transform.localPosition.z), FloatingSpeed * Time.deltaTime);
        }

        else
        {
            ObiAnimObj.transform.localPosition = Vector3.MoveTowards(ObiAnimObj.transform.localPosition, new Vector3(ObiAnimObj.transform.localPosition.x, -0.10f, ObiAnimObj.transform.localPosition.z), FloatingSpeed * Time.deltaTime);
        }

        if (Stay)
        {
            StayTime += Time.deltaTime;

            if(StayTime > 4.5f)
            {
                ObiAnim.SetBool("Desplazamiento", false);
                ObiAnim.SetBool("Locura", true);
                ChangeState(ObiStates.OBIDOWN);

                if (StayTime > 6.2f)
                {
                    ObiAnim.SetBool("Desplazamiento", false);
                    ObiAnim.SetBool("Locura", true);
                    ChangeState(ObiStates.OBIDOWN2);
                }
            }
        }

        if (!Atlantis)
        {

            GM = GameObject.Find("GameManager").GetComponent<Game_Manager>();

            if (!Salto)
            {
                switch (State)
                {

                    case ObiStates.RUTA1:
                        ObiAnim.SetBool("Desplazamiento", true);
                        float Dist1 = Vector3.Distance(transform.position, R1[CureentWayPoint].transform.position);
                        LookAt(R1[CureentWayPoint]);
                        if (Dist1 < Radius)
                        {
                            CureentWayPoint++;
                            if (CureentWayPoint > R1.Count - 1)
                            {
                                CureentWayPoint = 0;
                                ChangeState(ObiStates.STOP1);
                            }
                        }

                        break;

                    case ObiStates.RUTA2:

                        LookAt(R2[CureentWayPoint]);
                        ObiAnim.SetBool("Desplazamiento", true);
                        float Dist2 = Vector3.Distance(transform.position, R2[CureentWayPoint].transform.position);
                        if (Dist2 < Radius)
                        {
                            CureentWayPoint++;
                            if (CureentWayPoint > R2.Count - 1)
                            {
                                CureentWayPoint = 0;
                                ChangeState(ObiStates.STOP2);
                            }
                        }

                        break;

                    case ObiStates.RUTA3:

                        LookAt(R3[CureentWayPoint]);
                        ObiAnim.SetBool("Desplazamiento", true);
                        float Dist3 = Vector3.Distance(transform.position, R3[CureentWayPoint].transform.position);
                        if (Dist3 < Radius)
                        {
                            CureentWayPoint++;
                            if (CureentWayPoint > R3.Count - 1)
                            {
                                CureentWayPoint = 0;
                                ChangeState(ObiStates.STOP3);
                            }
                        }
                        break;

                    case ObiStates.STOP1:
                        ObiAnim.SetBool("Desplazamiento", false);
                        float Dist4 = Vector3.Distance(transform.position, STOPS[0].transform.position);
                        LookAt(STOPS[0]);
                        if (Dist4 < Radius)
                        {
                            ObiAnim.SetBool("Desplazamiento", false);
                            StopClock += Time.deltaTime;
                            if (StopClock > 3)
                            {
                                ChangeState(ObiStates.AVISO);
                                StopClock = 0;
                            }
                        }
                        break;
                    case ObiStates.STOP2:

                        float Dist5 = Vector3.Distance(transform.position, STOPS[1].transform.position);
                        LookAt(STOPS[1]);
                        if (Dist5 < Radius)
                        {
                            StopClock += Time.deltaTime;
                            ObiAnim.SetBool("Desplazamiento", false);
                            if (StopClock > 3)
                            {
                                ChangeState(ObiStates.AVISO);
                                StopClock = 0;
                            }
                        }
                        break;
                    case ObiStates.STOP3:


                        float Dist6 = Vector3.Distance(transform.position, STOPS[2].transform.position);
                        LookAt(STOPS[2]);
                        if (Dist6 < Radius)
                        {
                            StopClock += Time.deltaTime;
                            ObiAnim.SetBool("Desplazamiento", false);
                            if (StopClock > 3)
                            {
                                ChangeState(ObiStates.AVISO);
                                StopClock = 0;
                            }
                        }
                        break;
                    case ObiStates.AVISO:
                        ObiAnim.SetBool("Desplazamiento", true);
                        float Dist7 = Vector3.Distance(transform.position, STOPS[3].transform.position);
                        if (Dist7 < Radius)
                        {
                            ObiAnim.SetBool("Desplazamiento", false);
                            StopClock += Time.deltaTime;
                            LookAtCamera(STOPS[4]);
                            GM.ObiLab[4].enabled = true;
                            if (StopClock > 6)
                            {
                                int RandomNum;
                                RandomNum = Random.Range(1, 4);

                                if (RandomNum == 1)
                                {
                                    ChangeState(ObiStates.RUTA1);
                                    GM.ObiLab[4].enabled = false;
                                }

                                if (RandomNum == 2)
                                {
                                    ChangeState(ObiStates.RUTA2);
                                    GM.ObiLab[4].enabled = false;
                                }

                                if (RandomNum == 3)
                                {
                                    ChangeState(ObiStates.RUTA3);
                                    GM.ObiLab[4].enabled = false;
                                }
                                StopClock = 0;
                            }
                        }
                        else
                        {
                            LookAt(STOPS[3]);
                        }

                        break;

                    case ObiStates.REGRESO:
                        ObiAnim.SetBool("Desplazamiento", true);
                        float Dist8 = Vector3.Distance(transform.position, Vuelta[0].transform.position);
                        if (Dist8 < Radius)
                        {
                            ObiAnim.SetBool("Desplazamiento", false);

                            LookAtCamera(Vuelta[1]);
                            GM.Regreso[0].enabled = true;
                            StopClock += Time.deltaTime;
                            if (!GM.Regreso[0].isPlaying)
                            {
                                GM.Regreso[1].enabled = true;
                            }
                            if (!GM.Regreso[1].isPlaying && StopClock > 2)
                            {
                                GM.Regreso[2].enabled = true;
                            }
                            if (!GM.Regreso[2].isPlaying && StopClock > 6.8f)
                            {
                                GM.Regreso[3].enabled = true;
                            }
                            if (!GM.Regreso[3].isPlaying)
                            {
                                if (StopClock > 9.5f)
                                {
                                    int RandomNum;
                                    RandomNum = Random.Range(1, 4);

                                    if (RandomNum == 1)
                                    {
                                        ChangeState(ObiStates.RUTA1);
                                        GM.Regreso[0].enabled = false;
                                        GM.Regreso[1].enabled = false;
                                        GM.Regreso[2].enabled = false;
                                        GM.Regreso[3].enabled = false;
                                    }

                                    if (RandomNum == 2)
                                    {
                                        ChangeState(ObiStates.RUTA2);
                                        GM.Regreso[0].enabled = false;
                                        GM.Regreso[1].enabled = false;
                                        GM.Regreso[2].enabled = false;
                                        GM.Regreso[3].enabled = false;
                                    }

                                    if (RandomNum == 3)
                                    {
                                        ChangeState(ObiStates.RUTA3);
                                        GM.Regreso[0].enabled = false;
                                        GM.Regreso[1].enabled = false;
                                        GM.Regreso[2].enabled = false;
                                        GM.Regreso[3].enabled = false;
                                    }
                                    StopClock = 0;
                                }
                            }
                        }

                        else
                        {
                            LookAt(Vuelta[0]);
                        }

                        break;
                }
            }

            else
            {
                RutaObiSalto();
                ObiSaltoTime += Time.deltaTime;
            }
        }

        else
        {



            switch (State)
            {

                case ObiStates.INICIOATL:

                    ObiAnim.SetBool("Desplazamiento", true);
                    LookAt(ATLWayPoints[3]);
                    LookAtCamera(ATLWayPoints[1]);

                    break;


                case ObiStates.EVENT1:


                    ObiAnim.SetBool("Locura", false);
                    ObiAnim.SetBool("Desplazamiento", false);
                    ObiAnim.SetBool("Saludo1", true);
                    LookAt(ATLWayPoints[5]);
                    LookAtCamera(ATLWayPoints[0]);

                    break;


                case ObiStates.LOOKATFRONT:

                    ObiAnim.SetBool("Locura", false);
                    ObiAnim.SetBool("Saludo1", false);
                    ObiAnim.SetBool("Desplazamiento", true);
                    LookAtCamera(ATLWayPoints[1]);

                    break;


                case ObiStates.LOOKATFRONT2:

                    ObiAnim.SetBool("Locura", false);
                    ObiAnim.SetBool("Saludo1", false);
                    ObiAnim.SetBool("Desplazamiento", true);
                    LookAtCamera(ATLWayPoints[2]);

                    break;

                case ObiStates.LOOKATTHING:

                    ObiAnim.SetBool("Locura", false);
                    ObiAnim.SetBool("Saludo1", false);
                    ObiAnim.SetBool("Desplazamiento", false);
                    ObiAnim.SetBool("Señalar", true);

                    LookAtCamera(ATLWayPoints[2]);

                    break;


                case ObiStates.LOOKATCAMERA:


                    ObiAnim.SetBool("Locura", false);
                    ObiAnim.SetBool("Desplazamiento", true);
                    LookAtCamera(ATLWayPoints[0]);

                    break;


                case ObiStates.CHANGESIDE:

                    ObiAnim.SetBool("Desplazamiento", false);
                    transform.position = Vector3.MoveTowards(transform.position, ATLWayPoints[6].transform.position, Speed * Time.deltaTime);
                    LookAtCamera(ATLWayPoints[2]);

                    break;

                case ObiStates.CAIDA:

                    ObiAnim.SetBool("Saludo1", false);
                    ObiAnim.SetBool("Desplazamiento", false);
                    ObiAnim.SetBool("Locura", true);
                    LookAtCamera(ATLWayPoints[0]);

                    break;

                case ObiStates.MONSTER:

                    ObiAnim.SetBool("Desplazamiento", false);
                    ObiAnim.SetBool("Saludo1", true);
                    LookAtCamera(ATLWayPoints[0]);

                    break;

                case ObiStates.OBIUP:

                    ObiAnim.SetBool("Desplazamiento", false);
                    ObiAnim.SetBool("Saludo1", true);
                    LookAtCamera(ATLWayPoints[8]);
                    transform.position = Vector3.MoveTowards(transform.position, ATLWayPoints[8].transform.position, Speed * Time.deltaTime);

                    if (transform.position == ATLWayPoints[8].transform.position)
                    {
                        transform.parent = GameObject.Find("Atlantis").transform;
                        ChangeState(ObiStates.OBIUP2);
                    }

                    break;


                case ObiStates.OBIUP2:

                    ObiAnim.SetBool("Desplazamiento", false);
                    ObiAnim.SetBool("Saludo1", true);
                    LookAtCamera(ATLWayPoints[9]);
                    transform.position = Vector3.MoveTowards(transform.position, ATLWayPoints[9].transform.position, Speed * Time.deltaTime);

                    if (transform.position == ATLWayPoints[9].transform.position)
                    {
                        LookAtCamera(ATLWayPoints[10]);
                    }

                    break;

                case ObiStates.OBIDOWN:

                    LookAtCamera(ATLWayPoints[0]);
                    transform.position = Vector3.MoveTowards(transform.position, ATLWayPoints[8].transform.position, Speed * Time.deltaTime);

                    if (transform.position == ATLWayPoints[8].transform.position)
                    {
                        ChangeState(ObiStates.OBIDOWN2);
                    }

                    break;

                case ObiStates.OBIDOWN2:

                    ObiAnim.SetBool("Locura", false);
                    ObiAnim.SetBool("Desplazamiento", true);
                    Speed = 2.5f;    
                    LookAtCamera(ATLWayPoints[11]);
                    transform.position = Vector3.MoveTowards(transform.position, ATLWayPoints[11].transform.position, Speed * Time.deltaTime);

                    break;

                case ObiStates.BALL:

                    EnergyBall.transform.localScale = Vector3.MoveTowards(EnergyBall.transform.localScale, new Vector3(0, 0, 0), 0.2f * Time.deltaTime);
                    EnergyBall.transform.position = Vector3.MoveTowards(EnergyBall.transform.position, transform.position, 0.5f * Time.deltaTime);

                    if (EnergyBall.transform.localScale == new Vector3(0, 0, 0))
                    {
                        Stay = true;
                    }

                    break;

            }

        }

    }

    private void LookAt(GameObject obj)
    {
        transform.position = Vector3.MoveTowards(transform.position, obj.transform.position, Speed * Time.deltaTime);
        Vector3 Dir = obj.transform.position - transform.position;
        float step = RSpeed * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, Dir, step, 0.0F);
        transform.rotation = Quaternion.LookRotation(newDir);
    }

    private void LookAtCamera(GameObject obj)
    {
        Vector3 Dir = obj.transform.position - transform.position;
        float step = RSpeed * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, Dir, step, 0.0F);
        transform.rotation = Quaternion.LookRotation(newDir);
    }

    public void ChangeState(ObiStates NewState)
    {
        State = NewState;
    }

    public void RutaObiSalto()
    {
        switch (State)
        {
            case ObiStates.INICIO_SALTO:
                RutaObiSalto2();
                break;
        }
    }
    public void RutaObiSalto2()
    {
        Manager.InstantiateSFX();
        if (ObiSalto == 0)
        {
            CureentWayPoint = 0;
            LookAtCamera(STOPS[4]);
            if (ObiSaltoTime >= 3.5f)
            {
                ObiSalto++;
            }
        }
        if (ObiSalto == 1)
        {
            LookAt(RJump[0]);
            float Dist = Vector3.Distance(transform.position, RJump[CureentWayPoint].transform.position);
            if (Dist < Radius)
            {
                float Dist_1 = Vector3.Distance(transform.position, STOPS[0].transform.position);
                LookAt(STOPS[0]);
                if (Dist_1 < Radius)
                {
                    if (ObiSaltoTime >= 6f)
                    {
                        Manager.Scene_SFX[2] = true;
                        ObiSalto++;
                    }
                }
            }
        }
        if (ObiSalto == 2)
        {
            Manager.ColumnaLightDown();
            if (ObiSaltoTime >= 20.5f)
            {
                Manager.Scene_SFX[3] = true;
                ObiSalto++;
            }
        }
        if (ObiSalto == 3)
        {
            LookAtCamera(STOPS[4]);
            if (ObiSaltoTime >= 30.5f)
            {
                Manager.Scene_SFX[4] = true;
                CureentWayPoint++;
                ObiSalto++;
            }
        }
        if (ObiSalto == 4)
        {
            float Dist1 = Vector3.Distance(transform.position, RJump[CureentWayPoint].transform.position);
            LookAt(RJump[CureentWayPoint]);
            if (Dist1 < Radius)
            {
                if (CureentWayPoint == 1)
                {
                    CureentWayPoint++;
                }
                if (ObiSaltoTime >= 38f)
                {
                    Manager.Scene_SFX[5] = true;
                    transform.position = RJump[3].transform.position;
                    transform.rotation = Quaternion.Euler(0f, 37.33f, 0);
                    ObiSalto++;
                }
            }
        }
        if (ObiSalto == 5)
        {
            Vector3 Dir = VR_CAM.transform.position - transform.position;
            float step = RSpeed * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, Dir, step, 0.0F);
            transform.position = Vector3.MoveTowards(transform.position, RJump[4].transform.position, Speed * Time.deltaTime);
            if (transform.position == RJump[4].transform.position)
            {
                if (ObiSaltoTime >= 43f)
                {
                    ObiSalto++;
                }
            }
        }
        if (ObiSalto == 6)
        {
            float Dist2 = Vector3.Distance(transform.position, RJump[5].transform.position);
            LookAt(RJump[5]);
            if (Dist2 <= Radius)
            {
                Movidito.enabled = true;
                GameObject NewSilla = GameObject.Find("SillaPlayer");
                transform.parent = NewSilla.transform;
                if (ObiSaltoTime >= 51f)
                {
                    ObiSalto++;
                }
            }

        }
        if (ObiSalto == 7)
        {
            Movidito.enabled = false;
            GameObject NewSilla2 = GameObject.Find("SillaPlayer");
            transform.parent = NewSilla2.transform;
            Manager.Silla.SetBool("Viajar", true);
        }


    }
}


