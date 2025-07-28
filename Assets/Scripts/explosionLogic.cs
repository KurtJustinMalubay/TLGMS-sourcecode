using UnityEngine;

public class explosionLogic : MonoBehaviour
{
    public Animator AnimationBomb;
    private float explosionTime;
    void OnEnable()
    {
        explosionTime = 2f;
        AnimationBomb.SetBool("Explosion", true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (explosionTime > 0f)
            explosionTime -= Time.deltaTime;
        else
        {
            
        }
    }
}
