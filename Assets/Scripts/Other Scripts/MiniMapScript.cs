using UnityEngine;

public class MiniMapScript : MonoBehaviour
{
    public Transform player;

    void FixedUpdate()
    {
        transform.position = new Vector3(player.position.x, player.position.y, -2.3f);
    }
}
