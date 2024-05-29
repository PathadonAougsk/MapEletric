using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
   void OnCollisionEnter2D(Collision2D col){
        col.gameObject.transform.position = GameObject.Find("Spawnpoint").transform.position;
    }
}
