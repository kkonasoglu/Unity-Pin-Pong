using UnityEngine;
public class MoveScript : MonoBehaviour
{
    [Header("Key settings")]
    public KeyCode upKey = KeyCode.W;
    public KeyCode downKey = KeyCode.S;
    public bool IsAI = false;

    [Header("Key Settings - Rotation")]
    public KeyCode tiltLeftKey = KeyCode.A;
    public KeyCode tiltRightKey = KeyCode.D;

    [Header("Angle & Rotation Settings")]
    public float maxTiltAngle = 25f;
    public float tiltSpeed = 10f;


    [Header("Speed Of Players")]
    public float speed = 10f;

    [Header("Limit")]
    public float yBound = 4f;
    private float currentZangle = 0f;
    void Update()
    {
        MovePaddle();
        HandleRotationInput();
        ApplySmoothRotation();
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

        transform.Translate(Vector2.up * inputY * speed * Time.deltaTime, Space.World);
    }

    void HandleRotationInput()
    {
        float targetTilt = 0f;

        if (Input.GetKey(tiltLeftKey))
        {
            targetTilt = maxTiltAngle;
        }
        else if (Input.GetKey(tiltRightKey))
        {
            targetTilt = -maxTiltAngle;
        }

        currentZangle = Mathf.Lerp(currentZangle,targetTilt,Time.deltaTime * tiltSpeed);

    }

    void ApplySmoothRotation()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, currentZangle);
    }
    void Boundries()
    {
        //tr: sütunların dışarısına çıkmaması için
        //en: this is for do not pass the boundries
        float clamedY = Mathf.Clamp(transform.position.y, -yBound, yBound);
        transform.position = new Vector2(transform.position.x, clamedY);
    }
}
