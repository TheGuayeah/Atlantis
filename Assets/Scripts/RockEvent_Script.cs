using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockEvent_Script : MonoBehaviour
{

    public bool BRock;
    public float Gvty1, Gvty2, Air, Water;
    public GameObject Dust;

    private Rigidbody Rigid;

    // Use this for initialization
    void Start()
    {
        Rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider Col)
    {

        if (Col.tag == "FRTriggerCol")
        {
            if (BRock)
            {
                if(Dust != null)
                {
                    Dust.SetActive(true);
                }
                GetComponent<BoxCollider>().isTrigger = false;
            }

            else
            {
                GetComponent<MeshCollider>().isTrigger = false;
            }

            Physics.gravity = new Vector3(0, Mathf.MoveTowards(Gvty2, Gvty1, Time.deltaTime * 10), 0);
        }

        if (Col.tag == "FRWaterCol")
        {
            Rigid.velocity = new Vector3 (0, Water,0);
            Physics.gravity = new Vector3(0, Gvty2, 0);
            //print(Rigid.velocity);
        }
    }
}
