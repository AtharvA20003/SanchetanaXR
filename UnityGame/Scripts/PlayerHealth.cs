using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{

    // Start is called before the first frame update
    private float health;
    private float lerpTimer;

    [Header("Health Bar")]
    public int maxHealth = 100;
    public float chipSpeed = 2f;
    public Image frontHealthBar;
    public Image backHealthBar;
    public TextMeshProUGUI healthText;

    public GameObject gameOver;
    public bool isDead;

    /*[Header("Damage OverLay")]
    public Image overlay;
    public float duration;
    public float fadeSpeed;
    private float durationTimer;*/


    void Start()

    {
        health = maxHealth;
        //overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0);
    }

    // Update is called once per frame
    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();
        /*if(overlay.color.a > 0)
        {
            if (health < 30)
                return;
            durationTimer += Time.deltaTime;
            if (durationTimer > duration)
            {
                float tempAlpha = overlay.color.a;
                tempAlpha -= Time.deltaTime * fadeSpeed;
                overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, tempAlpha);
            }
        }*/
    }

    public void UpdateHealthUI()
    {
     
        float fillFront = frontHealthBar.fillAmount;
        float fillBack = backHealthBar.fillAmount;
        float hFraction = health / maxHealth;
        if(fillBack > hFraction)
        {
            frontHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(fillBack, hFraction, percentComplete);
        }

       

    }

    public void TakeDamage(int damage)
    {
        
        
        health -= damage;
        if (health <= 0)
        { 
            int randomValue = Random.Range(0, 2);
            //print("Player Dead");
            PlayerDead();
            isDead = true;
            SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.playerDeath);
        }
        else
        {
            //print("Player hit");
            SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.playerHurt);
        }
        
        lerpTimer = 0f;
        //durationTimer = 0;
        //overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 1);
        healthText.text = health.ToString();
    }

    public void PlayerDead()
    {
        GetComponent<GyroscopeBasedCameraMovement>().enabled = false;
        GetComponent<PlayerMotor>().enabled = false;
        GetComponent<InputManager>().enabled = false;

        GetComponentInChildren<Animator>().enabled = true;
        GetComponent<ScreenBlackout>().StartFade();
        StartCoroutine(ShowGameOverUI());
    }

    private IEnumerator ShowGameOverUI()
    {
        yield return new WaitForSeconds(1f);
        gameOver.gameObject.SetActive(true);
    }

    public void RestoreHealth(float healAmount)
    {
        health += healAmount;
        if (health > 100)
            health = 100f;
        lerpTimer = 0f;
        //overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0);
        healthText.text = health.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ZombieHand"))
        {
            if(!isDead)
                TakeDamage(5);
        }
    }

}
