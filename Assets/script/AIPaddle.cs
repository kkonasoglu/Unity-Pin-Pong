using UnityEngine;

public class AIPaddle : MonoBehaviour
{
    public float speed = 8f;
    public Transform ball;
    public float yBound = 4f;
    void Update()
    {
        if(ball == null)return;
        float direction = 0f;

        if(ball.position.y > transform.position.y + 0.2)
        {
            direction = 1f;
        }
        else if(ball.position.y < transform.position.y - 0.2f)
        {
            direction = -1f;
        }

        transform.Translate(Vector2.up*direction*speed*Time.deltaTime);

        float clampedY = Mathf.Clamp(transform.position.y, -yBound,yBound);
        transform.position = new Vector2(transform.position.x,clampedY);
    }
}
