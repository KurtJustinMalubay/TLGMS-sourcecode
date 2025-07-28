using UnityEngine;
using static playerScript;

public class enemyAttack : MonoBehaviour
{
    public Transform attackPosition;
    private enemyScript enemyScript;
    public LayerMask whatIsPlayer;
    private float timeToAttack;
    private float attackCooldown;
    public float attackRange;
    private int damage;
    //EFFECTS
    public Animator slashAnim;
    void OnEnable()
    {
        if (slashAnim != null)
        {
            slashAnim.Rebind(); 
            slashAnim.Update(0f);
        }
        enemyScript = GetComponent<enemyScript>();
        damage = 1;
        attackCooldown = Random.Range(.5f, .6f);
        timeToAttack = attackCooldown;
    }

    void FixedUpdate()
    {
        EnemyAttackLogic();
    }

    private void EnemyAttackLogic()
    {
        if (Vector2.Distance(transform.position, PlayerTracker.playerTransform.position) <= 1.8f)
        {
            if (timeToAttack > 0)
            {
                timeToAttack -= Time.deltaTime;
                if (timeToAttack <= 0.2)
                    slashAnim.SetBool("isEnemySlashing", false);
                return;
            }
            slashAnim.SetBool("isEnemySlashing", true);
            Collider2D[] playersToDamage = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, whatIsPlayer);
            for (int i = 0; i < playersToDamage.Length; i++)
            {
                playerScript player = playersToDamage[i].GetComponent<playerScript>();
                if (player != null)
                {
                    player.playerImg.color = new Color32(255, 50, 50, 255);
                    player.circle.color = new Color32(255, 50, 50, 255);
                    player.TakeDamage(damage);
                }
            }
            timeToAttack = attackCooldown;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosition.position, attackRange);
    }
}