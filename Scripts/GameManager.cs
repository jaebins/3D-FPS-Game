using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Scripts")]
    public ObjectManager objectManager;
    public GameObject puaseUI;
    public AudioSource sound;
    Image bloodyImage;

    [Header("UI")]
    public Scrollbar mouseScrollBar;
    public Slider healthBar;
    public Text healthText;
    public Text bulletText;
    public Image bloodScreen;

    [Header("Gun && Health")]
    public int maxHealth;
    public int nowHealth;
    public int maxBullet;
    public int nowBullet;
    public int reloadCount;

    [Header("Player")]
    public int playerSpeed;
    public int jumpPower;

    [Header("Game")]
    public AudioSource randomSound;
    public float cameraSpeed;
    public float enemySpawnDelay;
    public float randomSoundDelay;
    float randomSoundTimer;

    [Header("Source")]
    public AudioClip gunSound;
    public AudioClip gunReloadSound;
    public AudioClip hitSound;
    public AudioClip monsterDieSound;
    public AudioClip[] randomScaredSounds;
    public AudioClip playerWalkSound;
    public AudioClip playerRunSound;

    float enemySpawnTimer;

    void Start()
    {
        nowBullet = maxBullet;
        bulletText.text = nowBullet + "/" + maxBullet;

        cameraSpeed = (mouseScrollBar.value * 100) / 6;

        nowHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.minValue = 0;
        healthBar.value = nowHealth;
        healthText.text = nowHealth + "/" + maxHealth;

        randomSoundDelay = Random.Range(10, 17);

        sound = GetComponent<AudioSource>();
        bloodyImage = bloodScreen.GetComponent<Image>();
    }

    void Update()
    {
        enemySpawnTimer += Time.deltaTime;
        if(enemySpawnTimer > enemySpawnDelay)
        {
            GameObject enemy = objectManager.getObj("enemy");
            int ranX = Random.Range(20, 200);
            int ranZ = Random.Range(20, 200);
            enemy.transform.position = new Vector3(ranX, 0, ranZ);
            enemySpawnTimer = 0;
            StartCoroutine(EnemyDelete(enemy));
        }

        randomSoundTimer += Time.deltaTime;
        if(randomSoundTimer > randomSoundDelay)
        {
            int ran = Random.Range(0, randomScaredSounds.Length);
            randomSound.clip = randomScaredSounds[ran];
            randomSound.Play();
            randomSoundTimer = 0;
            randomSoundDelay = Random.Range(10, 25);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            puaseUI.SetActive(true);
        }
    }

    IEnumerator EnemyDelete(GameObject enemy)
    {
        yield return new WaitForSeconds(40f);
        enemy.SetActive(false);
    }
    
    public void ChangeScreen_HitStart()
    {
        float a = 0.15f;
        StartCoroutine(ChangeScreen_Hit(a));
    }

    IEnumerator ChangeScreen_Hit(float a)
    {
        yield return new WaitForSeconds(0.1f);
        bloodyImage.color = new Color(1, 0, 0, a);
        a -= 0.01f;
        if(a > 0)
        {
            StartCoroutine(ChangeScreen_Hit(a));
        }
    }

    public void GameContinue()
    {
        Time.timeScale = 1;
        puaseUI.SetActive(false);
    }

    public void GameQuit()
    {
        Application.Quit();
    }

    public void MovingMouseConfirm()
    {
        cameraSpeed = (mouseScrollBar.value * 100) / 6;
    }
}
