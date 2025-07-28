using System;
using UnityEngine;
using static playerScript;

public class bombBOt : MonoBehaviour
{
    public GameObject parent;
    public GameObject explosion;
    public Rigidbody2D rb;
    private playerScript player;
    private playerAttack playerAttack;
    public AudioSource bombTimerSfx;
    public SpriteRenderer redZone;
    public LayerMask whatIsPlayer;
    public LayerMask Enemy;
    [NonSerialized]public bool canMove = false;
    private float moveSpeed = 6f;
    private float bombTime;
    private float stopDistance = 1.2f;
    private float health;
    private bool thereIsPlayer;
    public float scanRange;
    private EffectsScript effect;
    private void Start()
    {
        effect = GameObject.FindGameObjectWithTag("Effects").GetComponent<EffectsScript>();
        redZone.enabled = true;
    }
    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<playerScript>();
        playerAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<playerAttack>();
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("OwnCollider"), true);
        health = 15f;
        bombTime = 2f;
        canMove = false;
        thereIsPlayer = false;
        redZone.enabled = true;
    }

    private void FixedUpdate()
    {
        if (!thereIsPlayer)
            checkPlayer();
        else
        {
            canMove = true;
            redZone.enabled = false;
        }
        if (canMove)
        {
            if (bombTime == 2f)
                bombTimerSfx.Play();
            if (bombTime > 0f)
                bombTime -= Time.deltaTime;
            else
            {
                canMove = false;
                playerAttack.bombEffect.Play();
                Collider2D bombHits = Physics2D.OverlapCircle(transform.position, 2f, whatIsPlayer);
                effect.SpawnExplosion(transform.position);
                if (bombHits != null)
                    player.TakeDamage(5f);
                transform.parent.gameObject.SetActive(false);
            }
            float distance = Vector2.Distance(PlayerTracker.playerTransform.position, transform.position);
            Vector2 direction = (PlayerTracker.playerTransform.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rb.rotation = angle;
            if (distance > stopDistance)
                rb.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
            else
                rb.linearVelocity = Vector2.zero;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canMove = true;
            bombTimerSfx.Play();
            redZone.enabled = false;
        }
    }

    public void TakeDamage(float damage)
    {
        if (health >= 0)
        {
            health -= damage;
            if (health <= 0)
            {
                transform.parent.gameObject.SetActive(false);
            }
        }

    }
    void checkPlayer()
    {
        if (Physics2D.OverlapCircle((Vector2)transform.position, scanRange, whatIsPlayer))
            thereIsPlayer = true;
    }
}
