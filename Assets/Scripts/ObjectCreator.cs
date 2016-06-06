using UnityEngine;
using System.Collections;

public class ObjectCreator : Singleton<ObjectCreator> {
	
    public GameObject modelingObject;
    public Mesh triangle;
    public Mesh square;
    public Mesh hexagon;
    public Mesh octagon;

    public Transform objects;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.F))
        {
			createNewObject(ModelingObject.ObjectType.square, null, new Vector3(0, 0, 0f));
        }

		if (Input.GetKeyDown (KeyCode.K)) {
			Debug.DrawLine(new Vector3 (0f,0f,0f), new Vector3(20f,20f,20f), Color.red, 500f);
		}
    }

	public void createNewObject(ModelingObject.ObjectType type, Face groundface, Vector3 offSet)
    {
        GameObject newObject = new GameObject();
        newObject = Instantiate(modelingObject);
        newObject.transform.SetParent(objects);
		newObject.GetComponent<ModelingObject> ().typeOfObject = type;

        switch (type)
        {
		case ModelingObject.ObjectType.triangle:
			newObject.GetComponent<ModelingObject> ().Initiate (triangle);
                break;
            case ModelingObject.ObjectType.square:
                newObject.GetComponent<ModelingObject>().Initiate(square);
                break;
            case ModelingObject.ObjectType.hexagon:
                newObject.GetComponent<ModelingObject>().Initiate(hexagon);
                break;
            case ModelingObject.ObjectType.octagon:
                newObject.GetComponent<ModelingObject>().Initiate(octagon);
                break;
        }

		newObject.transform.localPosition = new Vector3 (0f, 0f, 0f);

		if (groundface != null) {
			newObject.GetComponent<ModelingObject> ().bottomFace.ReplaceFaceOnOtherFace (groundface, new Vector3 (0f, 0f, 0f), false);
			newObject.GetComponent<ModelingObject> ().topFace.ReplaceFaceOnOtherFace (groundface, groundface.normal, true);
		} else {
			newObject.transform.localPosition = newObject.transform.localPosition + offSet;
		}

		newObject.GetComponent<ModelingObject> ().ShowNormals ();


    }


	public void createNewObjectOnFace(Face groundface) {

		// get number of vertices
		int numberOfVertices = groundface.vertexBundles.Length;
		Vector3 offset = new Vector3(0f,0f,0f);

		switch (numberOfVertices) 
		{
		case 3:
			createNewObject (ModelingObject.ObjectType.triangle, groundface, offset);
			break;
		case 4:
			createNewObject (ModelingObject.ObjectType.square, groundface, offset);
			break;
		case 6:
			createNewObject (ModelingObject.ObjectType.hexagon, groundface, offset);
			break;
		case 8:
			createNewObject (ModelingObject.ObjectType.octagon, groundface, offset);
			break;
		}
	}

}
