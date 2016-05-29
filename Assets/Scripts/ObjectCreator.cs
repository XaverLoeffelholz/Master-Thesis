using UnityEngine;
using System.Collections;

public class ObjectCreator : MonoBehaviour {

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
            createNewObject(ModelingObject.ObjectType.triangle);
            createNewObject(ModelingObject.ObjectType.square);
        }

  
    }

    public void createNewObject(ModelingObject.ObjectType type)
    {
        GameObject newObject = new GameObject();
        newObject = Instantiate(modelingObject);
        newObject.transform.SetParent(objects);
 

        switch (type)
        {
            case ModelingObject.ObjectType.triangle:
                newObject.GetComponent<ModelingObject>().Initiate(triangle);
                newObject.transform.localPosition = new Vector3(-2, 0, 0);
                break;
            case ModelingObject.ObjectType.square:
                newObject.GetComponent<ModelingObject>().Initiate(square);
                newObject.transform.localPosition = new Vector3(2, 0, 0);
                break;
            case ModelingObject.ObjectType.hexagon:
                newObject.GetComponent<ModelingObject>().Initiate(hexagon);
                break;
            case ModelingObject.ObjectType.octagon:
                newObject.GetComponent<ModelingObject>().Initiate(octagon);
                break;
        }




    }

}
