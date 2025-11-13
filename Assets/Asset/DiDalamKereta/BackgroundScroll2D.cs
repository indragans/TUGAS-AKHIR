using UnityEngine;

public class BackgroundScroll2D : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 0.2f; // kecepatan geser background
    private Material mat;
    private float offset;

    void Start()
    {
        // ambil material dari sprite renderer
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        mat = sr.material;
    }

    void Update()
    {
        // tambahkan offset tiap frame
        offset += scrollSpeed * Time.deltaTime;

        // ubah offset tekstur pada material
        mat.mainTextureOffset = new Vector2(offset, 0);
    }
}
