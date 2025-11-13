using UnityEngine;

public class BackgroundMover : MonoBehaviour
{
    public float moveSpeed = 1f;

    void Update()
    {
        // Gerak terus ke kiri
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;
    }
}
