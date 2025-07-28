using System.Collections;
using UnityEngine;

public class bombSpawn : MonoBehaviour
{
    private playerAttack playerAttack;
    private void Start()
    {
        playerAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<playerAttack>();
    }
    private void OnEnable()
    {
        StartCoroutine(deleteOnTime());
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerAttack != null)
            {
                playerAttack.bombCount += 1;
                gameObject.SetActive(false);
            }
        }
    }
    private IEnumerator deleteOnTime()
    {
        yield return new WaitForSeconds(12f);
        gameObject.SetActive(false);
    }
}