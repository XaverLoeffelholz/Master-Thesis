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
            if (Input.GetMouseButtonDown(0))
            {
                if (parent.focused)
                {
                    mousePressed = true;
                    temps = Time.time;
                }
            }

            if (mousePressed && parent.focused && Time.time - temps > 0.2f)
            {
                parent.DeSelect();
                parent.MoveObject();
            }

            if (Input.GetMouseButtonUp(0))
            {
                mousePressed = false;

                if (Time.time - temps <= 0.2f)
                {
                    if (parent.focused)
                    {
                        parent.Select();
                    } else
                    {
                        parent.DeSelect();
                    }
                }

                temps = 0;
            }       
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision Object with " + other.gameObject.name);

        if (other.gameObject.CompareTag("Selector"))
        {
            Debug.Log("Collision Object");
            parent.Focus();
        } 
    }

    void OnTriggerExit(Collider other)
    {
        parent.UnFocus();
    }
}