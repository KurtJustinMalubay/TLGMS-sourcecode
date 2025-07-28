using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Rendering;
using static playerScript;
using Random = UnityEngine.Random;

public class spawnChunk : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject bombBot;
    public Transform player;
    public Transform spawnPosition1;
    public Transform spawnPosition2;
    public playSoundsOnSpawn sounds;
    private float spawnTime = 7f;
    public float spawnRange;
    public int enemiesToSpawn;
    private int spawnMult;
    private bool canSpawn = true;
    public static spawnChunk instance;
    private List<GameObject> pooledObjects = new List<GameObject>();
    private List<GameObject> pooledBots = new List<GameObject>();
    private List<GameObject> pooledBull = new List<GameObject>();
    private List<GameObject> pooledHealth = new List<GameObject>();
    private List<GameObject> pooledBombs = new List<GameObject>();
    public GameObject ammoDrop;
    public GameObject bombDrop;
    public GameObject healthPills;
    private int bullPool = 12;
    private int healthPool = 12;
    private int bombsPool = 12;
    private int amountToPool = 100;
    private int botsPool = 20;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        SpawnTracker.currEnemCnt = 0;
    }

    private void Start()
    {
        for (int i = 0; i < amountToPool; i++) {
            GameObject obj = Instantiate(enemyPrefab);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
        for (int i = 0; i < botsPool; i++)
        {
            GameObject bots = Instantiate(bombBot);
            bots.SetActive(false);
            pooledBots.Add(bots);
        }
        for (int i = 0; i < bullPool; i++)
        {
            GameObject bull = Instantiate(ammoDrop);
            bull.SetActive(false);
            pooledBull.Add(bull);
        }
        for (int i = 0; i < healthPool; i++)
        {
            GameObject health = Instantiate(healthPills);
            health.SetActive(false);
            pooledHealth.Add(health);
        }
        for (int i = 0; i < bombsPool; i++)
        {
            GameObject bombs = Instantiate(bombDrop);
            bombs.SetActive(false);
            pooledBombs.Add(bombs);
        }
    }
    private void FixedUpdate()
    {
        if (canSpawn)
            StartCoroutine(spawnBot());

        if (SpawnTracker.currEnemCnt == 0)
        {
            SpawnEnemies();
        }
    }

    private void SpawnEnemies()
    {
        spawnMult = PlayerTracker.ScoreCount / 10 * 2;
        sounds.PlaySounds();
        SpawnTracker.currEnemCnt = enemiesToSpawn + spawnMult;
        for (int i = 0; i < enemiesToSpawn + spawnMult; i++)
        {
            GameObject enem = spawnChunk.instance.GetPooledObject();
            if (enem != null)
            {
                Vector2 randomPosition = new Vector2(
                Random.Range(spawnPosition1.position.x, spawnPosition2.position.x),
                Random.Range(spawnPosition1.position.y, spawnPosition2.position.y));
                enem.transform.position = randomPosition;
                enem.SetActive(true);
            }
        }
    }
    public static void spawnBulls(Vector2 pos)
    {
        GameObject bulls = spawnChunk.instance.GetPooledObjectBulls();
        if (bulls != null)
        {
            bulls.transform.position = pos;
            bulls.SetActive(true);
        }
    }
    public static void spawnHealth(Vector2 pos)
    {
        GameObject health = spawnChunk.instance.GetPooledObjectHealth();
        if (health != null)
        {
            health.transform.position = pos;
            health.SetActive(true);
        }
    }
    public static void spawnBombs(Vector2 pos)
    {
        GameObject bomb = spawnChunk.instance.GetPooledObjectBombs();
        if (bomb != null)
        {
            bomb.transform.position = pos;
            bomb.SetActive(true);
        }
    }
    IEnumerator spawnBot()
    {
        canSpawn = false;
        yield return new WaitForSeconds(spawnTime);
        GameObject bots = spawnChunk.instance.GetPooledObjectBots();
        if (bots != null)
        {
            canSpawn = true;
            Vector2 randomPosition = new Vector2(
            Random.Range(spawnPosition1.position.x, spawnPosition2.position.x),
            Random.Range(spawnPosition1.position.y, spawnPosition2.position.y));
            bots.transform.position = randomPosition;
            bots.SetActive(true);
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
    public GameObject GetPooledObjectBots()
    {
        for (int i = 0; i < pooledBots.Count; i++)
        {
            if (!pooledBots[i].activeInHierarchy)
                return pooledBots[i];
        }
        return null;
    }
    public GameObject GetPooledObjectBulls()
    {
        for (int i = 0; i < pooledBull.Count; i++)
        {
            if (!pooledBull[i].activeInHierarchy)
                return pooledBull[i];
        }
        return null;
    }
    public GameObject GetPooledObjectHealth()
    {
        for (int i = 0; i < pooledHealth.Count; i++)
        {
            if (!pooledHealth[i].activeInHierarchy)
                return pooledHealth[i];
        }
        return null;
    }
    public GameObject GetPooledObjectBombs()
    {
        for (int i = 0; i < pooledBombs.Count; i++)
        {
            if (!pooledBombs[i].activeInHierarchy)
                return pooledBombs[i];
        }
        return null;
    }
    public static class SpawnTracker
    {
        public static int currEnemCnt = 0;
    }
}
