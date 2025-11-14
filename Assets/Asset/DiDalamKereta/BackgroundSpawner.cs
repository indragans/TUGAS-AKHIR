using UnityEngine;

public class BackgroundSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] backgroundPrefabs;
    [SerializeField] private float spawnInterval = 1f;
    [SerializeField] private float heightRange = 0.5f;

    [SerializeField] private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnBackground();
            timer = 0f;
        }
    }

    void SpawnBackground()
    {
        // pilih background random dari array
        int randomIndex = Random.Range(0, backgroundPrefabs.Length);

        // posisi spawn acak di sumbu Y
        Vector3 spawnPos = transform.position + new Vector3(0, Random.Range(-heightRange, heightRange), 0);

        // spawn prefab
        GameObject background = Instantiate(backgroundPrefabs[randomIndex], spawnPos, Quaternion.identity);

        
    }
}
