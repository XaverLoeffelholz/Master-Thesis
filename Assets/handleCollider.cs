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
        handle.Focus();
    }

    void OnTriggerExit(Collider other)
    {
        handle.UnFocus();
    }
}