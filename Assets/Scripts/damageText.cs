using UnityEngine;

public class damageText : MonoBehaviour
{
    private float dmgTimer = 0.8f;


    // Update is called once per frame
    void Update()
    {
        dmgTimer -= Time.deltaTime;
        if (dmgTimer <= 0)
        {
            dmgTimer = 0.8f;
            Destroy(gameObject);
        }
    }
}
