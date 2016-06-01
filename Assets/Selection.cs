using UnityEngine;
using System.Collections;

public class Selection : Singleton<Selection>{

    public GameObject currentFocus;
    public GameObject currentSelection;
    public bool mousePressed;
    private float temps;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (currentFocus != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                mousePressed = true;
                temps = Time.time;
            }

            if (mousePressed && currentFocus.CompareTag("ModelingObject") && Time.time - temps > 0.2f)
            {
                currentFocus.GetComponent<ModelingObject>().MoveObject();
                UiCanvasGroup.Instance.Hide();
            }
            else if (mousePressed && currentFocus.CompareTag("Handle"))
            {
                currentFocus.GetComponent<handle>().ApplyChanges();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            mousePressed = false; 

            if (currentFocus != null)
            {
                if (currentFocus.CompareTag("ModelingObject") && Time.time - temps <= 0.2f)
                {
                    UiCanvasGroup.Instance.Show();
                    currentFocus.GetComponent<ModelingObject>().Select();

                } else if (currentFocus.CompareTag("Handle"))
                {
                    currentFocus.GetComponent<handle>().updateHandlePosition();
                    currentFocus.GetComponent<handle>().UnFocus(); 

                } else if (currentFocus.CompareTag("UiElement"))
                {
                    currentFocus.GetComponent<UiElement>().goal.ActivateMenu();
                }

            } else
            {
                UiCanvasGroup.Instance.Hide();
            }

            DeAssignCurrentSelection(currentSelection);

            temps = 0;
        }
    }

    public void AssignCurrentFocus(GameObject newCollider)
    {

        currentFocus = newCollider;
        temps = 0f;
    }

    public void DeAssignCurrentFocus(GameObject collider)
    {
        if (currentFocus == collider)
        {
            currentFocus = null;
        }

        temps = 0f;
    }

    public void AssignCurrentSelection(GameObject newCollider)
    {
        currentSelection = newCollider;
    }

    public void DeAssignCurrentSelection(GameObject collider)
    {
        if (currentSelection == collider)
        {
            currentSelection = null;
        }
    }


}
