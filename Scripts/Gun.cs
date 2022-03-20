using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public AudioSource sound;
    public Animator anime;
    public CameraManager cameraManager;

    void Start()
    {
        sound = GetComponent<AudioSource>();
        anime = GetComponent<Animator>();
    }

    void Update()
    {
        transform.localEulerAngles = new Vector3(0, 80, -cameraManager.mouseY);
    }
}
