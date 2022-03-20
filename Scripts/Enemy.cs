using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Animator anime;
    AudioSource sound;
    GameManager gameManager;
    GameObject player;

    public int speed;
    public int maxHealth;
    public int nowHealth;

    void Start()
    {
        nowHealth = maxHealth;
        anime = GetComponent<Animator>();
        sound = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * speed);
        transform.eulerAngles = new Vector3(0, -180, 0) + player.transform.rotation.eulerAngles;
        anime.SetInteger("moveDir", 1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Bullet")
        {
            nowHealth--;
            if(nowHealth == 0)
            {
                EnemyDead();
            }
        }
    }

    void EnemyDead()
    {
        Debug.Log("Àû Ã³Ä¡");
        sound.clip = gameManager.monsterDieSound;
        sound.Play();
        this.gameObject.SetActive(false);
        nowHealth = maxHealth;
    }
}
