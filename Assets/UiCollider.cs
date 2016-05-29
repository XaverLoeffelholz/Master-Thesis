using UnityEngine;
using System.Collections;

public class UiCollider : MonoBehaviour
{
    public UiElement parent;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Selector") && !Selection.Instance.mousePressed)
        {
            parent.Focus();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Selector") && !Selection.Instance.mousePressed)
        {
            parent.UnFocus();
        }
    }
}

