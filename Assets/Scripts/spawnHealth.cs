using System.Collections;
using UnityEngine;

public class spawnHealth : MonoBehaviour
{
    private playerScript playerScript;
    private void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<playerScript>();
    }
    private void OnEnable()
    {
        StartCoroutine(deleteOnTime());
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerScript.regenSFX.Play();
            playerScript.canHeal = true;
            playerScript.RegenTime += playerScript.maxRegenTime;
            gameObject.SetActive(false);
        }
    }
    private IEnumerator deleteOnTime()
    {
        yield return new WaitForSeconds(15f);
        gameObject.SetActive(false);
    }
}