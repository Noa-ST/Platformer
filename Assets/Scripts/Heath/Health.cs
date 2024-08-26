using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private Animator amin;
    private GameManager gameManager;
    private bool dead;

    [Header("isFrames")]
    [SerializeField] private float isFramesDuration;
    [SerializeField] private int numberOfFlashs;
    private SpriteRenderer spriteRenderer;

    private bool invulnerable;

    [Header("Player or Enemy")]
    [SerializeField] private bool isPlayer;

    [Header("Fall Threshold")]
    [SerializeField] private float fallThreshold = -50f; // Ngưỡng rơi

    private void Awake()
    {
        currentHealth = startingHealth;
        amin = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {

        if (isPlayer && transform.position.y < fallThreshold)
        {
            amin.SetTrigger("die");
            GameManager.instance.gameOver();
        }
    }

    public void TakeDamage(float _damage)
    {
        if (invulnerable) return;
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            amin.SetTrigger("hurt");
            StartCoroutine(Invulnerability());
        }
        else
        {
            if (!dead)
            {
                amin.SetTrigger("die");

                if(GetComponent<PlayerMovement>() != null)
                    GetComponent<PlayerMovement>().enabled = false;

                if(GetComponentInParent<EnemyPatrol>() != null)
                {
                    GetComponentInParent<EnemyPatrol>().enabled = false;
                }
                   
                if(GetComponent<KnightEnemy>() != null)
                    GetComponent<KnightEnemy>().enabled = false;

                if (GetComponent<AngryPig>() != null)
                    GetComponent<AngryPig>().enabled = false;

                if (GetComponent<Trunk>() != null)
                    GetComponent<Trunk>().enabled = false;

            }
                    
                dead = true;

            if (isPlayer)
            {
                GameManager.instance.gameOver();
            }
        }
        }
    

    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }

    private IEnumerator Invulnerability()
    {
        invulnerable = true;
        Physics2D.IgnoreLayerCollision(3, 8, true);
        for (int i = 0; i < numberOfFlashs; i++)
        {
            spriteRenderer.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(isFramesDuration / (numberOfFlashs) * 2);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(isFramesDuration / (numberOfFlashs) * 2);

        }
        Physics2D.IgnoreLayerCollision(3, 8, false);
        invulnerable = false;

    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}

