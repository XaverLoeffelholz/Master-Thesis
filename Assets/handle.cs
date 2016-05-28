using UnityEngine;
using System.Collections;

public class handle : MonoBehaviour {

    public enum handleType
    {
        ScaleFace,
        PositionCenter,
        Height
    };

    public handleType typeOfHandle;
    public GameObject colliderHandle;
    public handles handles;
    public Face face;
    private bool focused;
    private bool clicked;

	// Use this for initialization
	void Start () {
        focused = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            clicked = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            clicked = false;
            UnFocus();
        }

        if (focused)
        {
            if (clicked)
            {
                ApplyChanges();

            }
               
        }
    }

    public void ApplyChanges()
    {
        switch (typeOfHandle)
        {
            case handleType.ScaleFace:
                ScaleFace();
                break;
            case handleType.PositionCenter:
                MoveCenterPosition();
                break;
            case handleType.Height:
                ChangeHeight();
                break;
        }
    }

    private void ScaleFace()
    {
        float input = Input.GetAxis("Mouse X") * 0.2f;

        Debug.Log("ChangePosition2");
        Vector3 position = colliderHandle.transform.parent.localPosition;
        position.x += input;
        colliderHandle.transform.parent.localPosition = position;
        face.scaleFace(input);
    }

    private void MoveCenterPosition()
    {
        float inputX = Input.GetAxis("Mouse X") * 0.2f;
        float inputZ = Input.GetAxis("Mouse Y") * 0.2f;

        Vector3 position = colliderHandle.transform.parent.localPosition;
        position.x += inputX;
        position.z += inputZ;
        colliderHandle.transform.parent.localPosition = position;
        face.center.coordinates = position;
    }

    private void ChangeHeight()
    {
        float input = Input.GetAxis("Mouse Y") * 0.2f;

        Debug.Log("ChangePosition2");
        Vector3 position = colliderHandle.transform.parent.localPosition;
        position.y += input;
        colliderHandle.transform.parent.localPosition = position;
        face.center.coordinates = position;
    }

    public void Focus()
    {
        if (!clicked && !handles.objectFocused)
        {
            focused = true;
            handles.objectFocused = true;
            LeanTween.scale(this.gameObject, new Vector3(1.0f, 1.3f, 1.3f), 0.2f);
            LeanTween.color(this.gameObject, Color.cyan, 0.2f);
        }
    }

    public void UnFocus()
    {
        if(!clicked)
        {
            focused = false;
            handles.objectFocused = false;
            LeanTween.scale(this.gameObject, new Vector3(1.0f, 1.0f, 1.0f), 0.2f);
            LeanTween.color(this.gameObject, Color.white, 0.2f);
        }
    }
}
