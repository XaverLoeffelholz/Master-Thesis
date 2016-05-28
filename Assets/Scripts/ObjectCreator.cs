using UnityEngine;
using System.Collections;

public class ObjectCreator : MonoBehaviour {

    public GameObject triangle;
    public GameObject square;
    public GameObject hexagon;
    public GameObject octagon;

    public Transform objects;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.F))
        {
            createNewObject(ModelingObject.ObjectType.square);
        }

    }

    public void createNewObject(ModelingObject.ObjectType type)
    {
        GameObject newObject = new GameObject();

        switch (type)
        {
            case ModelingObject.ObjectType.triangle:
                newObject = Instantiate(triangle);
                break;
            case ModelingObject.ObjectType.square:
                newObject = Instantiate(square);
                break;
            case ModelingObject.ObjectType.hexagon:
                newObject = Instantiate(hexagon);
                break;
            case ModelingObject.ObjectType.octagon:
                newObject = Instantiate(octagon);
                break;

        }

        newObject.transform.SetParent(objects);
        newObject.transform.localPosition = new Vector3(0, 0, 0);

        newObject.GetComponent<ModelingObject>().Initiate();
    }

}
