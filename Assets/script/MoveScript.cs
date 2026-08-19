using UnityEngine;

public class MoveScript : MonoBehaviour
{
    [Header("Key settings")]
    public KeyCode upKey = KeyCode.W;
    public KeyCode downKey = KeyCode.S;

    [Header("Speed Of Players")]
    public float speed = 10f;

    [Header("Sınırlandırma")]
    public float yBound = 4f;

    void Update()
    {
        MovePaddle();
        Boundries();
    }

    void MovePaddle()
    {
        float inputY = 0;
        if (Input.GetKey(upKey))
        {
            inputY = 1f;
        }
        else if (Input.GetKey(downKey))
        {
            inputY = -1f;
        }
        transform.Translate(Vector2.up * inputY * speed * Time.deltaTime);
    }
    void Boundries()
    {
        //tr: sütunların dışarısına çıkmaması için
        //en: this is for do not pass the boundries
        float clamedY = Mathf.Clamp(transform.position.y, -yBound,yBound);

        transform.position = new Vector2(transform.position.x, clamedY);
    }
}
