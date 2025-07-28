using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class playerScript : MonoBehaviour
{
    //MAIN FUNCTIONS
    public RawImage playerImg;
    public SpriteRenderer circle;
    public Rigidbody2D rb;
    private Transform player;
    [NonSerialized] public Vector2 facingDirection;
    
    //UI
    public GameObject gameOver;
    public TextMeshProUGUI Score;
    public Slider healthUI;
    public Slider sprintUI;
    public float Health;
    private float maxHealth;
    public SceneMgr sceneMgr;
    public GameObject mobileControls;
    public GameObject botUI;

    //MOBILE CONTROLS
    public FixedJoystick joystick;
    public FixedJoystick shootJoystick;

    //MOVEMENT
    private Vector2 moveDirection;
    [NonSerialized] public bool isSprintCD = true;
    private bool canSprint;
    [NonSerialized]public float moveSpeed = 5.2f;
    [NonSerialized] public float sprintCount;
    private float stamina  = 2.5f;
    private float speedMult;
    [NonSerialized] private float sprintCD = 1.5f;
    [NonSerialized] public float sprintCDctr;
    private float moveX;
    private float moveY;
    
    //EFFECTS
    public TrailRenderer trail;
    public AudioSource hit;
    public AudioSource sprintSFX;
    private bool isHit = false;
    private float hitTime = 0.5f;
    
    //REGEN
    [NonSerialized]public float RegenTime = 0f;
    [NonSerialized] public float maxRegenTime = 3f;
    [NonSerialized] public bool canHeal = false;
    public AudioSource regenSFX;

    //TP
    public GameObject tpObj;
    public GameObject tpDir;
    public Slider tpSlider;
    public Slider tpSlider2;
    private float skillActive = 5f;
    private bool canTeleport = false;
    private bool tapOnce = false;
    private float tempTime = 0f;
    private float distance;
    private float distanceDir;
    public LayerMask whatIsNull;

    //CONTROLS
    public optionsScript sett;
    public Vector2 mousePos;
    private Vector2 prevMousePos;
    private bool tpButtonHeld = false;
    private bool sprintButtonHeld = false;
    void Start()
    {
        if (sett.sett.isMobile)
        {
            mobileControls.SetActive(true);
            botUI.SetActive(false);
        }
        else
        {
            mobileControls.SetActive(false);
            botUI.SetActive(true);
            sprintButtonHeld = false;
        }
        speedMult = 2.2f;
        sprintCount = stamina;
        facingDirection = Vector2.down;
        gameOver.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player").transform;
        canSprint = true;
        healthUI.maxValue = Health;
        sprintUI.maxValue = stamina;
        sprintCDctr = sprintCD;
        canHeal = false;
        maxHealth = Health;
        tpSlider.maxValue = skillActive;
        tpSlider2.maxValue = 3f;
    }
    private void Awake()
    {
        PlayerTracker.ScoreCount = 0;
        PlayerTracker.playerTransform = transform;
    }
    private void FixedUpdate()
    {
        if (sett.sett.isMobile)
        {
            moveX = joystick.Horizontal;
            moveY = joystick.Vertical;
            mousePos = new Vector2(shootJoystick.Horizontal, shootJoystick.Vertical) + (Vector2)transform.position;
            if (mousePos != Vector2.zero)
                prevMousePos = new Vector2(shootJoystick.Horizontal, shootJoystick.Vertical) + (Vector2)transform.position;
            PlayerTracker.direction = (prevMousePos - (Vector2)player.position).normalized;
        }
        else
        {
            moveX = Input.GetAxis("Horizontal");
            moveY = Input.GetAxis("Vertical");
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            PlayerTracker.direction = (mousePos - (Vector2)player.position).normalized;
        }
            movement();
            BasicLogic();
    }
    private void BasicLogic()
    {
        Score.text = PlayerTracker.ScoreCount.ToString();
        healthUI.value = Health;
        sprintUI.value = sprintCount;
        float angle = Mathf.Atan2(PlayerTracker.direction.y, PlayerTracker.direction.x) * Mathf.Rad2Deg - 90;
        rb.rotation = angle;
        if ((Input.GetKey(KeyCode.LeftShift) || sprintButtonHeld) && sprintCount > 0 && canSprint)
        {
            if (!sprintSFX.isPlaying)
                sprintSFX.Play();
        }
        else
        {
            if (sprintSFX.isPlaying)
                sprintSFX.Stop();
        }
        if (canHeal)
        {
            Regeneration();
        }
        if (isHit)
        {
            hitTime -= Time.deltaTime;
            if (hitTime <= 0)
            {
                hitTime = 0.5f;
                circle.color = Color.white;
                playerImg.color = Color.white;
                isHit = false;
            }
        }
        tpSlider2.value = tempTime;
        if (tapOnce)
            teleportLogic();

        if (canTeleport && !tapOnce)
        {
            if (Input.GetKey(KeyCode.T) || tpButtonHeld)
            {
                tpObj.SetActive(true);
                tpObj.transform.position = transform.position;
                tapOnce = true;
            }
        }
        else if(!canTeleport && !tapOnce)
        {
            tpSlider.value = skillActive;
            skillActive -= Time.deltaTime;
            if (skillActive <= 0)
            {
                canTeleport = true;
            }
        }
    }
    void movement()
    {
        PlayerTracker.playerTransform = transform;
        if (!isSprintCD)
        {
            sprintChecker();
        }
        if (!canSprint && isSprintCD)
        {
            sprintCount += Time.deltaTime * 0.7f;
            if (sprintCount >= stamina)
                canSprint = true;
        }
        if (sprintCount <= stamina && canSprint)
        {
            if (isSprintCD)
                sprintCount += Time.deltaTime * .6f;
            else
            {
                sprintChecker();
            }   
        }
        if (canSprint)
        {
            if (Input.GetKey(KeyCode.LeftShift) || sprintButtonHeld && sprintCount > 0 && isSprintCD)
            {
                PlayerTracker.isSprinting = true;
                moveSpeed = moveSpeed * speedMult;
                sprintCount -= Time.deltaTime;
            }
            if (sprintCount < 0)
            {
                PlayerTracker.isSprinting = false;
                isSprintCD = false;
                canSprint = false;
            }
            if (sprintCount <= stamina && !(Input.GetKey(KeyCode.LeftShift) || sprintButtonHeld))
            {
                PlayerTracker.isSprinting = false;
                isSprintCD = false;
                canSprint = false;
            }
        }
        
        moveDirection = new Vector2(moveX, moveY).normalized;
        if (isHit && hitTime > 0.2f)
        {
            rb.linearVelocity = moveDirection * (moveSpeed * .7f);
        }
        else
            rb.linearVelocity = moveDirection * moveSpeed;
        if (moveDirection != Vector2.zero)
        {
            facingDirection = moveDirection;
        }
        moveSpeed = 5.2f;
    }
    public void TPButton()
    {
        tpButtonHeld = true;
        StartCoroutine(buttonStop());
    }
    public void SprintButtonHeld()
    {
        sprintButtonHeld = true;
    }
    public void SprintButtonRaised()
    {
        sprintButtonHeld = false;
    }
    IEnumerator buttonStop()
    {
        yield return null;
        tpButtonHeld = false;
    }
    void teleportLogic()
    {
        if (tapOnce)
        {
            distance = Vector2.Distance(transform.position, tpObj.transform.position);
            if (tempTime <= 3f)
                tempTime += Time.deltaTime;
            else
            {
                if (distance > 4f)
                {
                    tpDir.SetActive(true);
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, tpObj.transform.position, 4f);
                    tpDir.transform.position = hit.point * 0.5f;
                }
                else
                    tpDir.SetActive(false);
                if (Input.GetKey(KeyCode.T) || tpButtonHeld)
                {
                    skillActive = 5f;
                    transform.position = tpObj.transform.position;
                    canTeleport = false;
                    tapOnce = false;
                    tpObj.SetActive(false);
                    tempTime = 0f;
                }
            }
        }
    }
    public void TakeDamage(float damage)
    {
        if (Health >= 0)
        {
            isHit = true;
            hit.Play();
            Health -= damage;
            if (Health < 0)
                Health = 0;
            if (Health < 1)
            {
                Die();
                gameOver.SetActive(true);
            }
        }
 
    }
    void Die()
    {
        healthUI.value = Health;
        Time.timeScale = 0f;
        if (sceneMgr.sets.maxScore < PlayerTracker.ScoreCount)
            sceneMgr.sets.maxScore = PlayerTracker.ScoreCount;
        sceneMgr.SaveData();
    }
    void sprintChecker()
    {
        sprintCDctr -= Time.deltaTime;
        if (sprintCDctr < 0)
        {
            isSprintCD = true;
            sprintCDctr = sprintCD;
        }
    }

    public void Regeneration()
    {
        if (Health >= maxHealth)
        {
            canHeal = false;
            RegenTime = 0f;
        }
        if (RegenTime >= 0)
        {
            RegenTime -= Time.deltaTime * 1.25f;
            Health += Time.deltaTime * 1.25f;
        }
        else
        {
            regenSFX.Stop();
            canHeal = false;
        }
    }
    public void playAgain()
    {
        SceneManager.LoadSceneAsync(1);
    }
    public void menu()
    {
        SceneManager.LoadSceneAsync(0);
    }
    public static class PlayerTracker
    {
        public static Transform playerTransform;
        public static int ScoreCount = 0;
        public static Vector2 direction;
        public static Transform gun;
        public static bool isSprinting = false;
    }
}