using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody rigid;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            rigid.velocity = Vector3.zero;
            this.gameObject.SetActive(false);
            this.gameObject.transform.position = new Vector3(0, 0, 0);
            Debug.Log("Àû Ãæµ¹");
        }
        if (other.tag == "Wall")
        {
            rigid.velocity = Vector3.zero;
            this.gameObject.SetActive(false);
            this.gameObject.transform.position = new Vector3(0, 0, 0);
        }
    }
}
