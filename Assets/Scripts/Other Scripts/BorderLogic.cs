using UnityEngine;
using UnityEngine.UIElements;

public class BorderLogic : MonoBehaviour
{
    public Rigidbody2D playerRB;
    public Transform player;
    public Transform[] border;
    public Transform ReturnPoint;
    public playerScript script;
    private Vector2 direction;
    private float distance;
    private float angle;

    private void FixedUpdate()
    {
        if (player.position.x <= border[0].position.x || player.position.y >= border[0].position.y || player.position.y <= border[1].position.y || player.position.x >= border[1].position.x)
            moveToPoint();
    }

    private void moveToPoint()
    {
        Vector2 direction = (ReturnPoint.position - player.position).normalized;
        playerRB.MovePosition((Vector2)player.position + direction * script.moveSpeed * Time.deltaTime);
    }
}
