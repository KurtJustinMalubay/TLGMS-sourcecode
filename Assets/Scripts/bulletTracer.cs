using UnityEngine;
using static playerAttack;
using static playerScript;

public class bulletTracer : MonoBehaviour
{
    private LineRenderer BulletTracer;
    private float disTime = 0.2f;

    private void Start()
    {
        BulletTracer = GetComponent<LineRenderer>();
    }
    public void OnEnable()
    {
        if (BulletTracer == null)
            BulletTracer = GetComponent<LineRenderer>();
        else
        {
            BulletTracer.enabled = true;
            BulletTracer.SetPositions(new Vector3[] { Attack.start, Attack.end });
            disTime = 0.2f;
        }
    }
    void Update()
    {
        disTime -= Time.deltaTime;
        if (disTime < 0)
        {
            disTime = 0.2f;
            HideTracer();
            BulletTracer.enabled = false;
            gameObject.SetActive(false);
        }
    }
    void HideTracer()
    {
        BulletTracer.SetPosition(0, Vector3.zero);
        BulletTracer.SetPosition(1, Vector3.zero);
    }
}
