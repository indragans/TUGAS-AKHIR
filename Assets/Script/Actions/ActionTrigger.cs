using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTrigger : MonoBehaviour
{
    [SerializeField] private SceneAction sceneAction = null;
    private Collider2D hitbox;
    private Vector2 hitpoint;

    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = sceneAction.GetActionIcon();

        hitbox = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            hitpoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (hitbox.OverlapPoint(hitpoint))
            {
                sceneAction.interaction();

                this.gameObject.SetActive(false);
            }
        }
    }
}
