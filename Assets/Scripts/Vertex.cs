using UnityEngine;
using System.Collections;

public class Vertex : MonoBehaviour {

    private VertexBundle parentVertexBundle;
    private ModelingObject parentObject;
    public bool moving;

	// Use this for initialization
	void Start () {
        moving = false;
        parentVertexBundle = transform.parent.GetComponent<VertexBundle>();
        parentObject = transform.parent.parent.parent.parent.GetComponent<ModelingObject>();
    }
	
	// Update is called once per frame
	void Update () {
	    if (parentVertexBundle.coordinates != this.transform.localPosition)
        {
            if (moving)
            {
                UpdateVertexBundle();
                Debug.Log("Update mesh");
            } else
            {
                UpdatePositionFromVertexBundle();
            }

            parentObject.UpdateMesh();
        }
	}

    public void UpdatePositionFromVertexBundle()
    {
        this.transform.localPosition = parentVertexBundle.coordinates;
    }

    public void UpdateVertexBundle()
    {
        parentVertexBundle.coordinates = this.transform.localPosition;
    }
}
