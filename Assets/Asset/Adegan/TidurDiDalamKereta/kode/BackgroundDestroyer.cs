using UnityEngine;

public class BackgroundDestroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BackgroundDalamKerata"))
        {
            Destroy(other.gameObject);
        }
    }
}
