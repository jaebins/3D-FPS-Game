using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Animator anime;
    public GameManager gameManager;
    public float mouseY;

    public bool isShake;
    public int shakeMaxCount;
    bool isShakeDelay;
    int shakeNowCount;

    void Start()
    {
        anime = GetComponent<Animator>();
    }

    void Update()
    {
        if (isShake && !isShakeDelay)
        {
            if (shakeNowCount > shakeMaxCount)
            {
                CancelInvoke("ShakeDelay");
                transform.eulerAngles = new Vector3(0, 0, 0);
                shakeNowCount = 0;
                isShake = false;
            }
            int ranX = Random.Range(0, 45);
            int ranY = Random.Range(0, 45);
            transform.eulerAngles = new Vector3(ranX, ranY, 0);
            shakeNowCount++;
            isShakeDelay = true;
            Invoke("ShakeDelay", 0.2f);
        }
        else if(!isShake)
        {
            mouseY += Input.GetAxisRaw("Mouse Y") * gameManager.cameraSpeed;
            mouseY = Mathf.Clamp(mouseY, -55.0f, 55.0f);
            transform.localEulerAngles = new Vector3(-mouseY, 0, 0);
        }
    }

    void ShakeDelay()
    {
        isShakeDelay = false;
    }
}
