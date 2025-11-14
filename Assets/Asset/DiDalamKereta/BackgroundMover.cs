using UnityEngine;

public class BackgroundMover : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float destroyDelay = 5f;

    void Start()
    {
        // auto-destroy setelah 20 detik
        Destroy(this.gameObject, destroyDelay); 
    }
    void Update()
    {
        // Gerak terus ke kiri
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;
    }
}
