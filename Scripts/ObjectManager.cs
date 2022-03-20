using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public GameObject bulletPref;
    public GameObject enemyPref;

    GameObject[] bullets;
    GameObject[] enemys;

    void Start()
    {
        bullets = new GameObject[30];
        enemys = new GameObject[60];

        for(int i = 0; i < bullets.Length; i++)
        {
            GameObject obj = Instantiate(bulletPref);
            obj.SetActive(false);
            bullets[i] = obj;
        }
        for(int i = 0; i < enemys.Length; i++)
        {
            GameObject obj = Instantiate(enemyPref);
            obj.SetActive(false);
            enemys[i] = obj;
        }

    }

    public GameObject getObj(string name)
    {
        GameObject[] tar = null;

        switch (name)
        {
            case "bullet":
                tar = bullets;
                break;
            case "enemy":
                tar = enemys;
                break;
        }

        for (int i = 0; i < tar.Length; i++)
        {
            if (!tar[i].activeSelf)
            {
                tar[i].SetActive(true);
                return tar[i];
            }
        }

        return null;
    }
}
