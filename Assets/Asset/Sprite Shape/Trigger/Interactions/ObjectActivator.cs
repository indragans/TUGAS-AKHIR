using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectActivator : MonoBehaviour
{
    private void OTriggerEnter2D(Collider2D collision)
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
