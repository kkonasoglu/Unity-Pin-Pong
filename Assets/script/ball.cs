using UnityEngine;
public class ball : MonoBehaviour
{
    [Header("Speed Setting")]
    public float initialSpeed = 10f;
    public float SpeedIncrease = 1.08f;
    public float RingSpeedIncrease = 1.2f;
    public float maxSpeed = 25f;
    public float boostSpeed = 22f;
    public float decayRate = 3.5f;



    [Header("Color Transition Settings")]
    public float colorChangeSpeed = 5f;

    private float currentSpeed;
    private float currentBaseSpeed;
    private TrailRenderer trail;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    private Color targetColor = Color.white;
    private Color currentColor = Color.white;


    private float ringCooldownTime = 0f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        trail = GetComponent<TrailRenderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // KRİTİK DÜZELTME: İki hız da initialSpeed ile başlar
        currentBaseSpeed = initialSpeed;
        currentSpeed = initialSpeed;

        currentColor = Color.white;
        targetColor = Color.white;

        ApplyColorInstantly(Color.white);

        LaunchBall();
    }

    private void Update()
    {

        if(ringCooldownTime > 0)
        {
            ringCooldownTime -= Time.deltaTime; 
        }
        currentColor = Color.Lerp(currentColor, targetColor, Time.deltaTime * colorChangeSpeed);
        ApplyCurrentColor();
    }

    private void FixedUpdate()
    {
        // Yalnızca yay/halka patlaması varsa o anki taban hıza sönümle
        if (currentSpeed > currentBaseSpeed)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, currentBaseSpeed, decayRate * Time.fixedDeltaTime);
            EvaluateTargetColor();
        }

        // Topun asla durmaması için hızı sürekli uygula
        if (rb.linearVelocity.sqrMagnitude > 0.001f)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * currentSpeed;
        }
    }

    void LaunchBall()
    {
        if (trail != null)
        {
            trail.emitting = true;
        }
        float y = Random.Range(0, 2) == 0 ? -1 : 1;
        float x = Random.Range(0, 2) == 0 ? -1 : 1;
        rb.linearVelocity = new Vector2(x, y).normalized * currentSpeed;
    }

    void ResetBall()
    {
        if (GameManager.Instance.isGameOver) return;

        if (trail != null)
        {
            trail.emitting = false;
            trail.Clear();
        }


        transform.position = Vector2.zero;
        rb.linearVelocity = Vector2.zero;
        currentSpeed = initialSpeed;
        currentBaseSpeed = initialSpeed;

        targetColor = Color.white;
        ApplyColorInstantly(Color.white);

        Invoke(nameof(LaunchBall), 1f);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("leftgoal"))
        {
            GameManager.Instance.Player2Scored();
            ResetBall();
        }
        else if (other.CompareTag("rightgoal"))
        {
            GameManager.Instance.Player1Scored();
            ResetBall();

        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            currentBaseSpeed = Mathf.Min(currentBaseSpeed * SpeedIncrease,maxSpeed);

            currentSpeed = Mathf.Max(currentSpeed, currentBaseSpeed);

            rb.linearVelocity = rb.linearVelocity.normalized * currentSpeed;

            if (SoundManager.Instance != null) SoundManager.Instance.PlayPaddleHit();
            EvaluateTargetColor();

        }
        else if (collision.gameObject.CompareTag("wall"))
        {
            SoundManager.Instance.PlayWallBounce();
        }
        else if (collision.gameObject.CompareTag("SpeedRing"))
        {
            SoundManager.Instance.PlaySpeedBoost();
            if(ringCooldownTime <= 0)
            {
                currentSpeed = Mathf.Min(boostSpeed, maxSpeed);
                ringCooldownTime = 0.5f;
            }
            rb.linearVelocity = rb.linearVelocity.normalized * currentSpeed;
            EvaluateTargetColor();
        }
        else
        {
            rb.linearVelocity = rb.linearVelocity.normalized *currentSpeed;
        }

        
    }

    void EvaluateTargetColor()
    {
        if (currentSpeed <= 11f)
        {
            targetColor = Color.white;
        }
        else if (currentSpeed <= 16f)
        {
            targetColor = Color.yellow;
        }
        else if (currentSpeed <= 22f)
        {
            targetColor = new Color(1f, 0.5f, 0f);
        }
        else
        {
            targetColor = Color.red;
        }
    }

    void ApplyCurrentColor()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = currentColor;
        }

        if (trail != null)
        {
            trail.startColor = currentColor;
            trail.endColor = new Color(currentColor.r, currentColor.g, currentColor.b, 0f);
        }

    }

    void ApplyColorInstantly(Color c)
    {
        currentColor = c;
        targetColor = c;
        ApplyCurrentColor();
    }

}
