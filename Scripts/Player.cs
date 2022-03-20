using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameManager gameManager;
    public ObjectManager objectManager;
    public Gun gun;
    public ParticleSystem gunParticle;
    public CameraManager cameraManager;

    Rigidbody rigid;
    Animator anime;
    AudioSource sound;

    float mouseX = 0;
    float mouseY = 0;
    bool isGunDelayState = true;
    bool isJump = false;
    bool isReload = false;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        anime = GetComponent<Animator>();
        sound = GetComponent<AudioSource>();
    }

    void Update()
    {
        Move();
        Jump();
        Fire();
    }

    void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.LeftShift))
        {
            gameManager.playerSpeed = 12;
            sound.clip = gameManager.playerRunSound;
            cameraManager.anime.speed = 1.2f;
        }
        else
        {
            gameManager.playerSpeed = 7;
            sound.clip = gameManager.playerWalkSound;
            cameraManager.anime.speed = 0.8f;
        }

        if (v > 0 || v < 0 || h > 0 || h < 0)
        {
            anime.SetInteger("moveDir", 1);
            cameraManager.anime.SetInteger("moveDir", 1);
            if (!sound.isPlaying) 
                sound.Play();
        }
        else
        {
            anime.SetInteger("moveDir", 0);
            cameraManager.anime.SetInteger("moveDir", 0);
            sound.Stop();
        }


        Vector3 move = new Vector3(h, 0, v);
        transform.position += transform.TransformDirection(move) * gameManager.playerSpeed * Time.deltaTime;

        if(transform.position.x <= 0f)
        {
            transform.position = new Vector3(1, transform.position.y, transform.position.z);
        }
        else if(transform.position.x >= 200f)
        {
            transform.position = new Vector3(199f, transform.position.y, transform.position.z);
        }

        if (transform.position.z <= 0f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 1f);
        }
        else if (transform.position.z >= 200f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 199f);
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isJump)
        {
            rigid.AddForce(Vector3.up * gameManager.jumpPower, ForceMode.Impulse);
            isJump = true;
            Invoke("JumpDelay", 0.7f);
        }
    }

    void Fire()
    {
        mouseX += Input.GetAxisRaw("Mouse X") * gameManager.cameraSpeed;
        mouseY += Input.GetAxisRaw("Mouse Y") * gameManager.cameraSpeed;
        mouseY = Mathf.Clamp(mouseY, -55.0f, 55.0f);

        transform.eulerAngles = new Vector3(0, mouseX, 0);

        if (!Input.GetMouseButton(0))
        {
            FireStop();
        }

        if (Input.GetMouseButton(0) && isGunDelayState && !isReload)
        {
            GameObject bullet = objectManager.getObj("bullet");

            //Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
            //    Input.mousePosition.y, transform.position.z + 1));
            bullet.transform.position = transform.position + new Vector3(0, 3, 1);
            bullet.transform.eulerAngles = new Vector3(-mouseY, mouseX, 0);

            Rigidbody bulletRid = bullet.GetComponent<Rigidbody>();
            bulletRid.AddForce(bullet.transform.TransformDirection(new Vector3(0, 0, 1) * 15), ForceMode.Force);

            isGunDelayState = false;
            Invoke("GunDelay", 0.2f);

            gun.anime.SetBool("Shoot", true);
            gunParticle.Play();
            gun.sound.clip = gameManager.gunSound;
            gun.sound.Play();
            gameManager.nowBullet--;
            gameManager.bulletText.text = gameManager.nowBullet + "/" + gameManager.maxBullet;
        }

        if((gameManager.nowBullet <= 0 && !isReload) || (Input.GetKeyDown(KeyCode.R) && gameManager.nowBullet < gameManager.maxBullet))
        {
            FireStop();
            isReload = true;
            gun.anime.SetBool("DoReload", true);
            gun.sound.clip = gameManager.gunReloadSound;
            gun.sound.loop = true;
            gun.sound.Play();
            Invoke("ReloadGun", gameManager.reloadCount);
        }
    }

    void FireStop()
    {
        gunParticle.Pause();
        gunParticle.Clear();
        gun.anime.SetBool("Shoot", false);
    }

    void GunDelay()
    {
        isGunDelayState = true;
    }

    void JumpDelay()
    {
        isJump = false;
    }

    void ReloadGun()
    {
        isReload = false;
        gun.anime.SetBool("DoReload", false);
        gun.sound.loop = false;
        gameManager.nowBullet = gameManager.maxBullet;
        gameManager.bulletText.text = gameManager.nowBullet + "/" + gameManager.maxBullet;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Enemy")
        {
            gameManager.nowHealth--;
            if(gameManager.nowHealth == 0)
            {
                GameOver();
            }
            gameManager.sound.clip = gameManager.hitSound;
            gameManager.healthBar.value = gameManager.nowHealth;
            gameManager.healthText.text = gameManager.nowHealth + "/" + gameManager.maxHealth;
            gameManager.ChangeScreen_HitStart();
            cameraManager.isShake = true;
        }
    }

    void GameOver()
    {
        Time.timeScale = 0;
    }
}
