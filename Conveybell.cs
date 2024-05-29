using Unity.VisualScripting;
using UnityEngine;

public class ConveyBell : MonoBehaviour{
    void OnCollisionEnter2D(Collision2D col){
        Debug.Log("Hit2");
        Debug.Log(col.gameObject.name);
        int pushforce = Random.Range(220, 230);
        int height = Random.Range(0, 10);

        col.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(pushforce, height));
    }
}   