using UnityEngine;
using System.Collections;

public class handle : MonoBehaviour {

    public enum handleType
    {
        ScaleFace,
        PositionCenter,
        Height,
        RotationX,
        RotationY,
        RotationZ,
    };

    public GameObject connectedObject;

    public handleType typeOfHandle;
    public GameObject colliderHandle;
    public handles handles;
    public Face face;
    private bool clicked;
	public bool focused = false;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
     
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
            case handleType.RotationX:
                RotateX();
                break;
            case handleType.RotationY:
                RotateY();
                break;
            case handleType.RotationZ:
                RotateZ();
                break;
        }
    }

    private void ScaleFace()
    {
        float input = Input.GetAxis("Mouse X") * 0.2f;

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

        Vector3 position = colliderHandle.transform.parent.localPosition;
        position.y += input;
        colliderHandle.transform.parent.localPosition = position;
        face.center.coordinates = position;
    }

    private void RotateX()
    {
        float input = Input.GetAxis("Mouse Y") * 6f;

        // rotate
        Vector3 rotation = new Vector3(0,0,0);

        rotation.x += input;
        connectedObject.transform.Rotate(rotation);
    }

    private void RotateY()
    {
        float input = Input.GetAxis("Mouse X") * 6f;

        // rotate
        Vector3 rotation = new Vector3(0, 0, 0);

        rotation.y += input;
        connectedObject.transform.Rotate(rotation);
    }

    private void RotateZ()
    {
        float input = Input.GetAxis("Mouse X") * 6f;

        // rotate
        Vector3 rotation = new Vector3(0, 0, 0);

        rotation.z += input;
        connectedObject.transform.Rotate(rotation);
    }

    public void updateHandlePosition()
    {
        Vector3 rotation = connectedObject.transform.localRotation.eulerAngles;
        transform.Rotate(rotation);
    }

    public void Focus()
    {
		if (!focused) {
			if (!clicked && !handles.objectFocused)	{
				
				Selection.Instance.AssignCurrentFocus(transform.gameObject);
				handles.objectFocused = true;

				LeanTween.color(this.gameObject, Color.cyan, 0.2f);
				focused = true;
			}
		}

    }

    public void UnFocus()
    {
		if (focused) {
			if(!Selection.Instance.mousePressed)
			{
				Selection.Instance.DeAssignCurrentFocus(transform.gameObject);
				handles.objectFocused = false;
				//LeanTween.scale(this.gameObject, new Vector3(1.0f, 1.0f, 1.0f), 0.2f);
				LeanTween.color(this.gameObject, Color.white, 0.2f);
				focused = false;
			}
		}
    }
}
