using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using static playerScript;

public class playerAttack : MonoBehaviour
{
    //MAIN
    private GameObject playerObj;
    private playerScript playerScript;
    private Transform player;
    public LayerMask whatIsEnemies;
    public LayerMask whatIsObstacles;
    public LayerMask bombBot;
    [NonSerialized]public Vector3 direction;
    public int damage = 10;
    private float timeToAttack;
    public float attackCooldown = 0.3f;
    public static float attackRange = 10f;
    private Vector3 mousePos;

    //SOUNDS
    public AudioSource pew;
    public AudioSource bombEffect;
    public AudioSource MeleeSFX;
    
    //EFFECTS
    public GameObject bulletEff;
    public GameObject gunObj;
    public EffectsScript BombExp;
    public GameObject BombEffect;
    public CursorScript cursor;
    public Transform Gun;
    public LineRenderer bulletTrail;
    public Animator slashAnim;
    [NonSerialized]public static RaycastHit2D hitBull;
    private float currScale;
    public static playerAttack instance;
    private List<GameObject> pooledObjects = new List<GameObject>();
    private int amountToPool = 30;

    //AMMO
    public TextMeshProUGUI maxA;
    public TextMeshProUGUI currA;
    public  int maxAmmo;
    private int ammo;
    private int magSize;
    private bool canShoot;
    private float reloadTime = 1f;
    private float reload = 1f;
    public bool needToReload = true;
    public bool reloading = false;
    
    //BOMB
    public GameObject bombArea;
    public TextMeshProUGUI bomb;
    public Slider BombSlider;
    private Transform bombPos;
    private Vector3 currentPos;
    private bool isThrowing = false;
    private bool canThrowBomb = false;
    [NonSerialized] public int bombCount;
    private float bombDamage = 20f;
    private float bombTime;
    private float bombTimeMax;
    public Joystick bombJS;
    
    //MELEE
    public Slider MelAttack;
    private bool canMeleeAtt = false;
    private bool isSlashing = false;
    private float meleeAttackCD = 1f;
    private float tempTime = 0.5f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = Instantiate(bulletEff);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
        bombPos = bombArea.transform;
        playerObj = GameObject.FindGameObjectWithTag("Player");
        playerScript = playerObj.GetComponent<playerScript>();
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        maxAmmo = 100;
        ammo = 10;
        canShoot = true;
        magSize = ammo;
        bombTimeMax = 4f;
        bombTime = bombTimeMax;
        MelAttack.maxValue = 0.92f;
        BombSlider.maxValue = bombTimeMax - 0.08f;
        bombCount = 4;
    }

    private void FixedUpdate()
    {
        Attacks();
        BasicLogics();
    }

    //void OnDrawGizmos()
    //{
    //    if (player != null)
    //    {
    //        Gizmos.color = Color.red;
    //        Gizmos.DrawLine(player.position, player.position + direction * attackRange);
    //        Gizmos.DrawWireSphere(player.position + direction, 1.2f);
    //        Gizmos.color = Color.blue;
    //        Gizmos.DrawWireSphere(mousePos, 2f);
    //    }
    //}
    private void BasicLogics()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        direction = PlayerTracker.direction;
        if (Input.GetKeyUp(KeyCode.R) || ammo == 0 && maxAmmo > 0)
            reloadLogic();
        if (!needToReload)
        {
            reload -= Time.deltaTime;
            needToReload = true;
            reloading = true;
            reloadLogic();
        }
        hitBull = Physics2D.Raycast(player.position, direction, attackRange, whatIsEnemies | whatIsObstacles);
        if (isSlashing)
        {
            tempTime -= Time.deltaTime;
            if (tempTime <= 0)
            {
                isSlashing = false;
                gunObj.SetActive(true);
                tempTime = 0.5f;
            }
        }
    }
    private void Attacks()
    {
        maxA.text = maxAmmo.ToString();
        currA.text = ammo.ToString();
        bomb.text = bombCount.ToString();
        MelAttack.value = meleeAttackCD;
        BombSlider.value = bombTime;

        //BOMB//
        if (bombCount > 0)
        {
            if (playerScript.sett.sett.isMobile)
            {
                Vector2 input = new Vector2(bombJS.Horizontal, bombJS.Vertical);
                bool hasInput = input != Vector2.zero;
                if (hasInput && bombTime >= bombTimeMax - 0.05f)
                {
                    bombPos.position =  7f * input + (Vector2)transform.position;
                    bombArea.SetActive(true);
                    currentPos = bombPos.position;
                    canThrowBomb = true;
                }
                else
                {
                    if (canThrowBomb)
                    {
                        currScale = 0f;
                        canThrowBomb = false;
                        BombEffect.transform.position = currentPos;
                        isThrowing = true;
                        BombEffect.transform.localScale = Vector2.zero;
                        BombEffect.SetActive(true);
                    }
                    bombArea.SetActive(false);
                }
            }
            else
            {
                if (Input.GetKey(KeyCode.Q) && bombTime >= bombTimeMax - 0.05f)
                {
                    bombPos.position = mousePos;
                    bombArea.SetActive(true);
                    canThrowBomb = true;
                }
                else
                {
                    if (canThrowBomb)
                    {
                        currScale = 0f;
                        currentPos = mousePos;
                        canThrowBomb = false;
                        BombEffect.transform.position = currentPos;
                        isThrowing = true;
                        BombEffect.transform.localScale = Vector2.zero;
                        BombEffect.SetActive(true);
                    }
                    bombArea.SetActive(false);
                }
            }
            if (bombTime <= bombTimeMax)
            {
                bombTime += Time.deltaTime;
            }
        }
        if (isThrowing)
        {
            if (currScale <= 4.063167)
            {
                BombEffect.transform.localScale += new Vector3(Time.deltaTime * 7, Time.deltaTime * 7, default);
                currScale += Time.deltaTime * 7;
            }
            else
                bombLogic();
        }
        //BOMB//

        //GUN//
        timeToAttack -= Time.deltaTime;
        if (timeToAttack <= 0 && needToReload && !isSlashing)
        {
            gunLogic();
        }
        //GUN//

        //MELEE//
        if (canMeleeAtt && playerScript.sprintCount / 0.3f >= 1)
        {
            if (!playerScript.sett.sett.isMobile)
                meleeAttackLogic();
        }
        else
        {
            meleeAttackCD += Time.deltaTime;
            if (meleeAttackCD >= 1f)
            {
                canMeleeAtt = true;
            }
            slashAnim.SetBool("IsSlashing", false);
        }
        //MELEE//
    }
    private void gunLogic()
    {
        bool shoot = false;
        if (playerScript.sett.sett.isMobile)
        {
            Vector2 input = new Vector2(playerScript.shootJoystick.Horizontal, playerScript.shootJoystick.Vertical);
            bool hasInput = input != Vector2.zero;
            if ((Math.Abs(playerScript.mousePos.x - player.transform.position.x) > 0.5f || Math.Abs(playerScript.mousePos.y - player.transform.position.y) > 0.5f) && hasInput)
            {
                shoot = true;
            }
        }
       else
       {
            if (Input.GetMouseButton(0))
            {
                shoot = true;
            }
        }  
        if (shoot)
        {
            ammoLogic();
            if (canShoot)
            {
                GameObject bull = playerAttack.instance.GetPooledObject();
                Attack.start = PlayerTracker.playerTransform.position;
                Attack.end = PlayerTracker.playerTransform.position + (Vector3)(PlayerTracker.direction * Attack.attackRangeGun);
                Attack.gun = Gun;
                LayerMask combinedMask = whatIsEnemies | whatIsObstacles;
                RaycastHit2D[] hits = new RaycastHit2D[1];
                int hitCount = Physics2D.RaycastNonAlloc(player.position, direction, hits, attackRange, combinedMask);
                if (hitCount > 0)
                {
                    Transform hitTransform = hits[0].transform;

                    if (hitTransform.CompareTag("Enemy") || hitTransform.CompareTag("EnemyBullet"))
                    {
                        GameObject hitObj = hitTransform.gameObject;
                        if (hitObj.TryGetComponent(out bombBOt basicEnemy))
                        {
                            Attack.end = basicEnemy.transform.position; 
                            basicEnemy.TakeDamage(damage);
                        }
                        else if (hitObj.TryGetComponent(out enemyScript shootingEnemy))
                        {
                            Attack.end = shootingEnemy.transform.position;
                            shootingEnemy.box.color = new Color32(255, 50, 50, 255);
                            shootingEnemy.TakeDamage(damage);
                        }
                    }
                }
                timeToAttack = attackCooldown;
                if (bull != null)
                {
                    bull.SetActive(true);
                }
            }
        }
    }

    void ammoLogic()
    {
        if (ammo <= 0) canShoot = false;
        if (ammo >= 1)
        {
            canShoot = true;
            ammo--;
            pew.Play();
        }
    }

    void reloadLogic()
    {
        if (reload > 0)
        {
            if (!playerScript.sett.sett.isMobile) cursor.setReloadCursor();
            needToReload = false;
        }
        if (needToReload)
        {
            int temp;
            if (magSize > maxAmmo) temp = (maxAmmo % magSize);
            else temp = (magSize - ammo);
            ammo += temp;
            maxAmmo -= temp;
            reload = reloadTime;
            if (!playerScript.sett.sett.isMobile) cursor.setShootingCursor();
            reloading = false;
        }
    }

    public void bombLogic()
    {
            bombTime = 0;
            isThrowing = false;
            BombEffect.SetActive(false);
            bombCount--;
            bombEffect.Play();
            BombExp.SpawnExplosion(currentPos);
            Collider2D[] bombHits = Physics2D.OverlapCircleAll((Vector2)currentPos, 2f, whatIsEnemies);
            foreach (Collider2D hit in bombHits)
            {
                enemyScript enemy = hit.GetComponent<enemyScript>();
                enemy.box.color = new Color32(255, 50, 50, 255);
                if (enemy != null) enemy.TakeDamage(bombDamage);
            }
            Collider2D[] BombHitsBomb = Physics2D.OverlapCircleAll((Vector2)currentPos, 2f, whatIsObstacles);
            foreach (Collider2D hit in BombHitsBomb)
            {
                bombBOt bot = hit.GetComponent<bombBOt>();
                if (bot != null)
                {
                    bot.TakeDamage(bombDamage);
                }
            }
            bombPos.position = Vector2.zero;
            currentPos = Vector2.zero;
    }

    private void meleeAttackLogic()
    {
        if (Input.GetMouseButton(1))
        {
            gunObj.SetActive(false);
            isSlashing = true;
            slashAnim.SetBool("IsSlashing", true);
            MeleeSFX.Play();
            Collider2D[] meleeHits = Physics2D.OverlapCircleAll((Vector2)player.position + (Vector2)direction, 1.2f, whatIsEnemies);
            foreach (Collider2D hit in meleeHits)
            {
                enemyScript enemy = hit.GetComponent<enemyScript>();
                enemy.box.color = new Color32(255, 50, 50, 255);
                if (enemy != null)
                {
                    enemy.TakeDamage(4f);
                }
            }
            Collider2D[] meleeHitsBomb = Physics2D.OverlapCircleAll((Vector2)player.position + (Vector2)direction, 1.2f, whatIsObstacles);
            foreach (Collider2D hit in meleeHitsBomb)
            {
                bombBOt bot = hit.GetComponent<bombBOt>();
                if (bot != null)
                {
                    bot.TakeDamage(4f);
                }
            }
            canMeleeAtt = false;
            meleeAttackCD = 0f;
            playerScript.sprintCount -= 0.3f;
            playerScript.isSprintCD = false;
            playerScript.sprintCDctr = 1f;
        }
    }
    public void meleeAttackMobile()
    {
        if (canMeleeAtt && playerScript.sprintCount / 0.3f >= 1)
        {
            gunObj.SetActive(false);
            isSlashing = true;
            slashAnim.SetBool("IsSlashing", true);
            MeleeSFX.Play();
            Collider2D[] meleeHits = Physics2D.OverlapCircleAll((Vector2)player.position + (Vector2)direction, 1.2f, whatIsEnemies);
            foreach (Collider2D hit in meleeHits)
            {
                enemyScript enemy = hit.GetComponent<enemyScript>();
                enemy.box.color = new Color32(255, 50, 50, 255);
                if (enemy != null)
                {
                    enemy.TakeDamage(4f);
                }
            }
            Collider2D[] meleeHitsBomb = Physics2D.OverlapCircleAll((Vector2)player.position + (Vector2)direction, 1.2f, whatIsObstacles);
            foreach (Collider2D hit in meleeHitsBomb)
            {
                bombBOt bot = hit.GetComponent<bombBOt>();
                if (bot != null)
                {
                    bot.TakeDamage(4f);
                }
            }
            canMeleeAtt = false;
            meleeAttackCD = 0f;
            playerScript.sprintCount -= 0.3f;
            playerScript.isSprintCD = false;
            playerScript.sprintCDctr = 1f;
        }
    }
    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
                return pooledObjects[i];
        }
        return null;
    }
    public static class Attack
    {
        public static Transform gun;
        public static float attackRangeGun = attackRange;
        public static Vector3 start;
        public static Vector3 end;
    }
}
