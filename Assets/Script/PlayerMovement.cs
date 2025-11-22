using UnityEngine;

public class PlayerMovementSimpleAnim : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private float originalScaleX;

    private Rigidbody2D rb;
    private Animator anim;
    private bool isGrounded = true;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        originalScaleX = transform.localScale.x;
    }

    void Update()
    {
        float move = Input.GetAxisRaw("Horizontal");

        // Gerak kiri kanan
        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);

        // Animasi jalan / idle
        anim.SetFloat("Speed", Mathf.Abs(move));

        // Flip kiri-kanan sederhana (TIDAK mengubah ukuran)
        if (move != 0)
        {
            transform.localScale = new Vector3(
                originalScaleX * Mathf.Sign(move),
                transform.localScale.y,
                transform.localScale.z
            );
        }

        // Lompat
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            anim.SetBool("IsJumping", true);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.5f)
        {
            isGrounded = true;
            anim.SetBool("IsJumping", false);
        }
    }
}
