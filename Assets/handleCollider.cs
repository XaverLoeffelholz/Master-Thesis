using UnityEngine;
using System.Collections;

public class handleCollider : MonoBehaviour
{
    private handle handle;

    // Use this for initialization
    void Start()
    {
        handle = this.GetComponentInParent<handle>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Selector") && !Selection.Instance.mousePressed)
        {
            handle.Focus();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Selector") && !Selection.Instance.mousePressed)
        {
            handle.UnFocus();
        }
    }
}