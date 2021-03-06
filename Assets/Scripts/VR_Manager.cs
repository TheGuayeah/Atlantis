using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VR_Manager : MonoBehaviour
{

    [HideInInspector]
    public RaycastHit hitInfo;

    public List <GameObject> Interact;
    public List <Renderer> Rend;
    [HideInInspector]
    public float OutWidth1 = 0;
    [HideInInspector]
    public float OutWidth2 = 0;
    //Para la carga de los saltos
    public GameObject Charge;
    public Image Cargando;
    public bool CoolingDown,start;
    public float WaitTime;

    // Use this for initialization
    void Start()
    {
        start = false;
        WaitTime = 3;
        Cargando.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Cargando
        if (CoolingDown)
        {
            Cargando.fillAmount = Mathf.MoveTowards(Cargando.fillAmount, 1, 1 / WaitTime * Time.deltaTime);
        }
        else
        {
            Cargando.fillAmount = Mathf.MoveTowards(Cargando.fillAmount, 0, 1 / (WaitTime - 3) * Time.deltaTime);
        }

        Vector3 FWD = transform.TransformDirection(Vector3.forward);/*Para que el Raycast siga a la camara*/
        Rend[0].material.SetFloat("_Outline", OutWidth1);/*Cojes la propiedad del Shader desde el renderer y le aplicas un valor*/
        Rend[1].material.SetFloat("_Outline2", OutWidth2);

        if (Physics.Raycast(transform.position, FWD, out hitInfo))
        {
            if (Interact[0] != hitInfo.collider && Interact[0].tag == hitInfo.collider.tag
                && Charge.activeSelf == true)
            {
                OutWidth1 = Mathf.MoveTowards(OutWidth1, 0.7f, 2 * Time.deltaTime);
                CoolingDown = true;
                //Debug.Log(hitInfo.collider.name);
                if (Cargando.fillAmount == 1)
                {                    
                    start = true;
                    Cargando.fillAmount = 0;
                    CoolingDown = false;
                    Charge.SetActive(false);
                    transform.GetComponent<VR_Manager>().enabled = false;
                    //Insertar cambio de escena correspondiente al salto seleccionado
                }
            }
        }
        else
        {
            OutWidth1 = Mathf.MoveTowards(OutWidth1, 0f, 2 * Time.deltaTime);
            CoolingDown = false;
        }

        if (Physics.Raycast(transform.position, FWD, out hitInfo))
        {

            if (Interact[1] != hitInfo.collider && Interact[1].tag == hitInfo.collider.tag)
            {
                OutWidth2 = Mathf.MoveTowards(OutWidth2, 0.7f, 2 * Time.deltaTime);
                CoolingDown = true;
                if (Cargando.fillAmount == 1)
                {
                    Application.Quit();
                }
            }
        }
        else
        {
            OutWidth2 = Mathf.MoveTowards(OutWidth2, 0f, 2 * Time.deltaTime);
            CoolingDown = false;
        }
    }
}
