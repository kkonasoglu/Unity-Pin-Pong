using UnityEngine;

public class Rotating : MonoBehaviour
{
    public float rotationSpeed = 10f;
    void Update()
    {
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}
