using UnityEngine;
using System.Collections;

public class ModelingObject : MonoBehaviour
{
    public enum ObjectType
    {
        triangle = 3,
        square = 4,
        hexagon = 6,
        octagon = 8
    }

    public ObjectType typeOfObject;

    public handles handles;
    [HideInInspector]
    public bool selected;

    private Mesh mesh;
    private MeshCollider meshCollider;
    private Vector3[] MeshCordinatesVertices;
    private Vector2[] MeshUV;
    private int[] MeshTriangles;

	public Vertex[] vertices;
	public Face[] faces;
    public GameObject[] vertexBundles;
    public Face topFace;
    public Face bottomFace;
    public Color color;
    public Vertex centerTop;
    public Vertex centerBottom;

    public GameObject VertexPrefab;
    public GameObject VertexBundlePrefab;

	public GameObject NormalPrefab;
	public GameObject Vertex2Prefab;
	public GameObject FacePrefab;

 

    // Use this for initialization
    void Start()
    {
        handles.gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Initiate(Mesh initialShape)
    {
        this.transform.GetChild(0).GetComponent<MeshFilter>().mesh = initialShape;

        mesh = this.transform.GetChild(0).GetComponent<MeshFilter>().mesh;
        meshCollider = this.transform.GetChild(0).GetComponent<MeshCollider>();
        meshCollider.sharedMesh = initialShape;

        MeshCordinatesVertices = mesh.vertices;
        MeshTriangles = mesh.triangles;
		mesh.RecalculateNormals ();

		Debug.Log ("Number of Triangles: " + mesh.triangles.Length);
		Debug.Log ("NUmber of Normals: " + mesh.normals.Length);
		Debug.Log ("NUmber of vertices: " + mesh.vertices.Length);


		int normalCount = 0;
		int vertexCount = 0;

        MeshUV = mesh.uv;

		vertices = new Vertex[MeshCordinatesVertices.Length];

		int i = 0;

		switch (typeOfObject) {
		case ObjectType.triangle:
			topFace.InitializeFace (3);
			bottomFace.InitializeFace (3);
			break;
		case ObjectType.square:
			topFace.InitializeFace (4);
			bottomFace.InitializeFace (4);
			break;
		case ObjectType.octagon:
			topFace.InitializeFace (6);
			bottomFace.InitializeFace (6);
			break;
		case ObjectType.hexagon:
			topFace.InitializeFace (8);
			bottomFace.InitializeFace (8);
			break;
		}

        Vector3 centerTopVector = new Vector3(0, 0, 0);
        Vector3 centerBottomVector = new Vector3(0, 0, 0);

		while (i < MeshCordinatesVertices.Length)
        {
            GameObject vert = Instantiate(VertexPrefab);

            if (MeshCordinatesVertices[i].y > 0.0f)
            {
                vert.transform.SetParent(topFace.gameObject.transform);
				centerTopVector += MeshCordinatesVertices[i];
            }
            else
            {
                vert.transform.SetParent(bottomFace.gameObject.transform);
                centerBottomVector += MeshCordinatesVertices[i];
            }

            vert.transform.localPosition = MeshCordinatesVertices[i];
            vertices[i] = vert.GetComponent<Vertex>();

			// the normals in the mesh have the same Ids as the vertices, so we can safe a normal for every vertex
			vertices [i].normal = mesh.normals [i];

            i++;
        }

        centerBottomVector = centerBottomVector / (MeshCordinatesVertices.Length / 2.0f);
        centerTopVector = centerTopVector / (MeshCordinatesVertices.Length / 2.0f);

        GameObject centerBottomPrefab = Instantiate(VertexPrefab);
        GameObject centerTopPrefab = Instantiate(VertexPrefab);

        centerBottomPrefab.name = "Center Bottom";
        centerTopPrefab.name = "Center Top";

        centerBottomPrefab.transform.SetParent(bottomFace.gameObject.transform);
        centerTopPrefab.transform.SetParent(topFace.gameObject.transform);

        centerBottomPrefab.transform.localPosition = centerBottomVector;
        centerTopPrefab.transform.localPosition = centerTopVector;

        centerTop = centerTopPrefab.GetComponent<Vertex>();
        centerBottom = centerBottomPrefab.GetComponent<Vertex>();

        mesh.vertices = MeshCordinatesVertices;

        BundleSimilarVertices(topFace.gameObject.transform);
        BundleSimilarVertices(bottomFace.gameObject.transform);

        topFace.center = centerTop.transform.parent.GetComponent<VertexBundle>();
        bottomFace.center = centerBottom.transform.parent.GetComponent<VertexBundle>();

        topFace.centerPosition = topFace.center.coordinates;
        bottomFace.centerPosition = bottomFace.center.coordinates;



		// always group 4 vertexbundles to new faces

		/*

		while(vertexCount<mesh.triangles.Length && normalCount < mesh.normals.Length) {
			// go through all triangles (for example 12 in cube)

			// check the normal, if there is already a triangle with this normal, don't do anything

			// otherwise create new normal and take the normal of this triangle as the normal of this face

			// compare it to the normals of other triangles, if similar ad its vertices to a face

			// calculate center when we have all vertices

			// when we have all faces we need to check for top and bottom face

			// if newly created one, just by y-value

			// if it is created on top of another, put the duplicated one in the bottom group and the others in the top group



			Vector3 point1 = MeshCordinatesVertices[mesh.triangles [vertexCount]];
			Vector3 point2 = MeshCordinatesVertices[mesh.triangles [vertexCount+1]];
			Vector3 point3 = MeshCordinatesVertices[mesh.triangles [vertexCount+2]];

			Vector3 center = (point1 + point2 + point3) / 3;

			GameObject normal = Instantiate (NormalPrefab);
			normal.transform.SetParent (transform.GetChild(0));
			normal.transform.position = transform.position + mesh.normals[normalCount]*1.2f;

			GameObject normal2 = Instantiate (NormalPrefab);
			normal2.transform.SetParent (transform.GetChild(0));
			normal2.transform.position = transform.position + mesh.normals[normalCount]*1.8f;

			GameObject p1 = Instantiate (Vertex2Prefab);
			p1.transform.SetParent (transform.GetChild(0));
			p1.transform.position = point1  + mesh.normals[normalCount]*0.08f;

			GameObject p2 = Instantiate (Vertex2Prefab);
			p2.transform.SetParent (transform.GetChild(0));
			p2.transform.position = point2 + mesh.normals[normalCount]*0.08f;

			GameObject p3 = Instantiate (Vertex2Prefab);
			p3.transform.SetParent (transform.GetChild(0));
			p3.transform.position = point3 + mesh.normals[normalCount]*0.08f;



			Color randomColor = new Color (Random.value, Random.value, Random.value, 1f);
			normal.GetComponent<Renderer> ().material.color = randomColor;
			normal2.GetComponent<Renderer> ().material.color = randomColor;


			p1.GetComponent<Renderer> ().material.color = randomColor;
			p2.GetComponent<Renderer> ().material.color = randomColor;
			p3.GetComponent<Renderer> ().material.color = randomColor;

			normalCount += 2;
			vertexCount += 3;
		}

		*/

        InitiateHandles();
		ShowNormals();
		GetFacesBasedOnNormals ();
    }

	public void ShowNormals(){
		for (int i = 0; i < vertices.Length; i++) {
			vertices [i].ShowNormal ();
		}
	}

    public void BundleSimilarVertices(Transform face)
    {

        Transform[] allChildren = face.GetComponentsInChildren<Transform>();

        for (int i = 0; i < allChildren.Length; i++)
        {
            Vector3 position = allChildren[i].localPosition;

            // get all Vertex bundles
            foreach (Transform vertexBundle in face)
                if (vertexBundle.CompareTag("VertexBundle"))
                {

                    // compare position of Vertex with position of vertex bundle, if similar, set vertex bundle as parent
                    if (position == vertexBundle.GetComponent<VertexBundle>().coordinates)
                    {
                        allChildren[i].SetParent(vertexBundle);
                    }

                }

            // if no similar found, create new vertex bundle
			if (!allChildren[i].parent.gameObject.CompareTag("VertexBundle"))
            {
                GameObject vertexBundle = Instantiate(VertexBundlePrefab);
                vertexBundle.transform.SetParent(face);
                vertexBundle.transform.localPosition = new Vector3(0, 0, 0);

                vertexBundle.GetComponent<VertexBundle>().coordinates = allChildren[i].transform.localPosition;

                allChildren[i].SetParent(vertexBundle.transform);

				if (vertexBundle.transform.childCount == 0) {
					Destroy (vertexBundle);
				} else {
					face.GetComponent<Face> ().AddVertexBundle (vertexBundle.GetComponent<VertexBundle>());
				}
	         }
        }
    }

    public void UpdateMesh()
    {
        for (int i = 0; i < MeshCordinatesVertices.Length; i++)
        {
            MeshCordinatesVertices[i] = vertices[i].transform.localPosition;
        }

        mesh.Clear();
        mesh.vertices = MeshCordinatesVertices;
        mesh.uv = MeshUV;
        mesh.triangles = MeshTriangles;
        mesh.RecalculateNormals();
        meshCollider.sharedMesh = mesh;
    }

    public void InitiateHandles()
    {
        handles.faceTopScale.transform.localPosition = topFace.centerPosition;
        handles.faceBottomScale.transform.localPosition = bottomFace.centerPosition;
        handles.CenterTopPosition.transform.localPosition = topFace.centerPosition;
        handles.CenterBottomPosition.transform.localPosition = bottomFace.centerPosition;
        handles.Height.transform.localPosition = topFace.centerPosition;
    }

    public void Focus()
    {
        Selection.Instance.AssignCurrentFocus(transform.gameObject);
    }

    public void UnFocus()
    {
        Selection.Instance.DeAssignCurrentFocus(transform.gameObject);
    }

    public void Select()
    {
        Selection.Instance.AssignCurrentSelection(transform.gameObject);
        Vector3 uiPosition = transform.position;
        uiPosition.y += 2.9f;
        UiCanvasGroup.Instance.transform.position = uiPosition;
        UiCanvasGroup.Instance.OpenMainMenu(this);
        handles.gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void DeSelect()
    {
        Selection.Instance.DeAssignCurrentSelection(transform.gameObject);
    }

    public void MoveObject()
    {
        float inputX = Input.GetAxis("Mouse X") * 0.2f;
        float inputY = Input.GetAxis("Mouse Y") * 0.2f;

        Vector3 position = transform.localPosition;
        position.x += inputX;
        position.y += inputY;
        transform.localPosition = position;
    }

	public void GetFacesBasedOnNormals(){
		int arrayLength = 0;

		switch (typeOfObject) {
			case ObjectType.triangle:
				arrayLength = 5;
				break;
			case ObjectType.square:
				arrayLength = 6;
				break;
			case ObjectType.octagon:
				arrayLength = 8;
				break;
			case ObjectType.hexagon:
				arrayLength = 10;
				break;
		}

		faces = new Face[arrayLength];

		int faceCount = 0;
		Face faceFound;

		// go trough all vertices
		for (int i = 0; i < vertices.Length; i++) {
			faceFound = null;

			// check for each vertex every face and 
			for (int j = 0; j < faces.Length; j++) {
				
				if (faces [j] != null && vertices [i].normal == faces [j].normal) {
					faceFound = faces [j];
				} 

			}

			if (faceFound != null) {
				Debug.Log ("add vertex bundle with vertex at " + vertices[i].transform.localPosition);
				faceFound.AddVertexBundle (vertices [i].transform.GetComponentInParent<VertexBundle>());
			} else {
				Debug.Log ("Create new face");
				GameObject newFace = Instantiate (FacePrefab);
				newFace.transform.SetParent (transform.GetChild(0));
				faces [faceCount] = newFace.GetComponent<Face> ();
				faces [faceCount].InitializeFace (4);
				faces [faceCount].normal = vertices [i].normal;

				faces [faceCount].AddVertexBundle (vertices [i].transform.GetComponentInParent<VertexBundle>());
				Debug.Log ("add vertex bundle with vertex at " + vertices[i].transform.localPosition);
				faceCount++;
			}
		}
	}


}