using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireBehaviour : MonoBehaviour
{
    public GameObject Firstobj;
    public GameObject SecondObj;
    private Vector2 FirstObjPos;
    private Vector2 SecondobjPos;
    public Transform startObject;
    public Transform endObject;
    public float maxLength = 5.0f;
     LineRenderer wire;
     private void Start()
    {
        if (Firstobj == null || SecondObj == null)
        {
            Debug.Log("One or both objects are null.");
            return;
        }

        FirstObjPos = Firstobj.transform.position;
        SecondobjPos = SecondObj.transform.position;

        wire = GetComponent<LineRenderer>();
        wire.SetPosition(0, FirstObjPos);
        wire.SetPosition(1, SecondobjPos);
    }

    private void Update()
    {
        if (Firstobj == null || SecondObj == null){
            Destroy(this.gameObject);
            return;
        }

        if (FirstObjPos != (Vector2)Firstobj.transform.position)
        {
            wire.SetPosition(0, Firstobj.transform.position);
            FirstObjPos = Firstobj.transform.position;
        }

        if (SecondobjPos != (Vector2)SecondObj.transform.position)
        {
            wire.SetPosition(1, SecondObj.transform.position);
            SecondobjPos = SecondObj.transform.position;
        }
    }
}
