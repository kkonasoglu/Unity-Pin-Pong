using System.Security.Cryptography;
using UnityEngine;

public enum AIDifficulty { Easy, Medium, Hard }
public class AIPaddle : MonoBehaviour
{
    public AIDifficulty difficulty = AIDifficulty.Medium;
    public Transform ballTransform;
    public float yBound = 4f;

    private float speed;
    private float reactionDelay;
    private float targetOffsetY;
    private float delayTimer;


    void Start()
    {
        ApplyDifficultySettings();
    }

    public void SetDifficulty(AIDifficulty newDifficulty)
    {
        difficulty = newDifficulty;
        ApplyDifficultySettings();
    }

    void ApplyDifficultySettings()
    {
        switch (difficulty)
        {
            case AIDifficulty.Easy:
                speed = 9.5f;
                reactionDelay = 0.25f;
                break;
            case AIDifficulty.Medium:
                speed = 15f;
                reactionDelay = 0.10f;
                break;
            case AIDifficulty.Hard:
                speed = 18.5f;
                reactionDelay = 0.0f;
                break;
        }
    }


    void Update()
    {
        if (ballTransform == null) return;
        delayTimer += Time.deltaTime;
        if (delayTimer >= reactionDelay)
        {
            delayTimer = 0f;
            targetOffsetY = difficulty == AIDifficulty.Hard ? 0f : Random.Range(-0.8f, 0.8f);
        }

        float targetY = ballTransform.position.y + targetOffsetY;
        Vector2 targetPosition = new Vector2(transform.position.x, targetY);

        transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, Mathf.Clamp(targetPosition.y, -yBound, yBound)), speed * Time.deltaTime);
    }
}
