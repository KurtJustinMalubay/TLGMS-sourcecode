using System;
using UnityEngine;
using static playerScript;
using static spawnChunk;
using Random = UnityEngine.Random;

public class enemyScript : MonoBehaviour
{
    public Rigidbody2D eRB;
    [NonSerialized] public SpriteRenderer box;
    public LayerMask whatIsPlayer;
    public Vector2 facingDirection;
    private Vector3 direction;
    private float moveSpeed;
    private float minSpeed = 4f;
    private float maxSpeed;
    public float stopDistance = 1.6f;
    private float health;
    private float distance;
    private float angle;
    private bool tookDamage;
    private float slowDuration = 0.5f;
    private void Start()
    {
        box = GetComponent<SpriteRenderer>();
        box.color = Color.white;
    }
    //TOOK DAMAGE
    void OnEnable()
    {
        facingDirection = Vector2.down;
        health = 10;
        maxSpeed = minSpeed + 3f;
        moveSpeed = Random.Range(minSpeed, maxSpeed);
        tookDamage = false;
    }
    private void FixedUpdate()
    {
        BasicLogic();   
    }
    private void BasicLogic()
    {
        distance = Vector3.Distance(PlayerTracker.playerTransform.position, transform.position);
        direction = (PlayerTracker.playerTransform.position - transform.position).normalized;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        eRB.rotation = angle;
        if (distance > stopDistance)
            moveEnemy(direction);
        else
            eRB.linearVelocity = Vector2.zero;
    }
    private void moveEnemy(Vector2 direction)
    {
        if (!tookDamage)
            eRB.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.fixedDeltaTime));
        else
        {
            eRB.MovePosition((Vector2)transform.position + (direction * (moveSpeed * 0.3f) * Time.fixedDeltaTime));
            slowDuration -= Time.deltaTime;
            if (slowDuration <= 0f)
            {
                box.color = Color.white;
                slowDuration = 0.5f;
                tookDamage = false;
            }
        }
    }
    public void TakeDamage(float damage)
    {
        tookDamage = true;
        health -= damage;
        if (health <= 0)
        {
            box.color = Color.white;
            enemyAttack attack = GetComponent<enemyAttack>();
            attack.slashAnim.Rebind();
            attack.slashAnim.Update(0f);
            attack.slashAnim.SetBool("isEnemySlashing", false);
            SpawnTracker.currEnemCnt--;
            PlayerTracker.ScoreCount++;
            int randomizer = Random.Range(0, 3);
            float randomNum = 0.2f;
            switch (randomizer)
            {
                case 0:
                    if (Random.Range(0f, 1f) < randomNum)
                        spawnChunk.spawnHealth(transform.position);
                    break;
                case 1:
                    if (Random.Range(0f, 1f) < 0.4f)
                        spawnChunk.spawnBulls(transform.position);
                    break;
                case 2:
                    if (Random.Range(0f, 1f) < randomNum)
                        spawnChunk.spawnBombs(transform.position);
                    break;
            }
            transform.parent.gameObject.SetActive(false);
        }
    }
}