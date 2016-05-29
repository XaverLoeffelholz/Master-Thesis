using UnityEngine;
using System.Collections;

public class meshCollisionHandler : MonoBehaviour
{
    public ModelingObject parent;
    private bool mousePressed;
    private float temps;

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