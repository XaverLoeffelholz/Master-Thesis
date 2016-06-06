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

    public GameObject VertexPrefab;
    public GameObject VertexBundlePrefab;

	public GameObject NormalPrefab;
	public GameObject Vertex2Prefab;
	public GameObject FacePrefab;

	bool focused = false;

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

		int normalCount = 0;
		int vertexCount = 0;

        MeshUV = mesh.uv;

		vertices = new Vertex[MeshCordinatesVertices.Length];


		for (int i=0; i < MeshCordinatesVertices.Length; i++)
        {
            GameObject vert = Instantiate(VertexPrefab);

            vert.transform.localPosition = MeshCordinatesVertices[i];
            vertices[i] = vert.GetComponent<Vertex>();

			// the normals in the mesh have the same Ids as the vertices, so we can safe a normal for every vertex
			vertices [i].normal = mesh.normals [i];
        }

		GetFacesBasedOnNormals ();

		for (int i=0; i < MeshCordinatesVertices.Length; i++)
		{
			if (MeshCordinatesVertices [i].y > 0.0f) {
				vertices[i].transform.SetParent (topFace.gameObject.transform);
			} else {
				vertices[i].transform.SetParent (bottomFace.gameObject.transform);
			}

			vertices [i].Initialize ();
		}

		mesh.vertices = MeshCordinatesVertices;

        BundleSimilarVertices(topFace.gameObject.transform);
        BundleSimilarVertices(bottomFace.gameObject.transform);

		AssignVertexBundlesToFaces ();

		for (int j = 0; j < faces.Length; j++) {
			faces[j].CalculateCenter();
		}
			
		InitiateHandles();
		InitializeVertices ();

		// always group 4 vertexbundles to new faces

		/*

		while(vertexCount<mesh.triangles.Length && normalCount < mesh.normals.Length) {
			// go through all triangles (for example 12 in cube)

			// check the normal, if there is already a triangle with this normal, don't do anything

			// otherwise create new normal and take the normal of this triangle as the normal of this face

			// compare it to the normals of other triangles, if similar ad its vertices to a face

			// calculate center when we have all vertices

			// when we have all faces we need to check for top and bottom face
GetFaceFromCollisionCoordinate
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

    }

	public void InitializeVertices () {
		for (int i = 0; i < vertices.Length; i++) {
			vertices [i].Initialize ();
		}
	}

	public void ShowNormals(){
		for (int i = 0; i < faces.Length; i++) {
			Debug.DrawLine(faces[i].center.transform.position, faces[i].center.transform.position + faces[i].normal*3.0f, Color.red, 500f);
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

		handles.faceTopScale.GetComponent<handle> ().face = topFace;
		handles.faceBottomScale.GetComponent<handle> ().face = bottomFace;

		handles.CenterTopPosition.GetComponent<handle> ().face = topFace;
		handles.CenterBottomPosition.GetComponent<handle> ().face = bottomFace;

		handles.Height.GetComponent<handle> ().face = topFace;

		topFace.centerHandle = handles.CenterTopPosition.GetComponent<handle> ();
		topFace.heightHandle = handles.Height.GetComponent<handle> ();
		topFace.scaleHandle = handles.faceTopScale.GetComponent<handle> ();

		bottomFace.centerHandle = handles.CenterBottomPosition.GetComponent<handle> ();
		//bottomFace.heightHandle = handles.Height.GetComponent<handle> ();
		bottomFace.scaleHandle = handles.faceBottomScale.GetComponent<handle> ();

    }

    public void Focus()
    {
		if (!focused) {
			Selection.Instance.AssignCurrentFocus(transform.gameObject);
			focused = true;
		}
    }

    public void UnFocus()
    {
		if (focused) {
			Selection.Instance.DeAssignCurrentFocus(transform.gameObject);
			focused = false;
		}
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


	public void AssignVertexBundlesToFaces(){
		
		// go through all vertices
		for (int i = 0; i < vertices.Length; i++) {

			// check normal of vertice and compare with normal of face
			for (int j = 0; j < faces.Length; j++) {

				// if normals are similar, linke parent vertex bundle to face
				if (vertices [i].normal == faces [j].normal) {
					faces [j].AddVertexBundle (vertices [i].transform.GetComponentInParent<VertexBundle> ());
				} 

			}
		}
	}


	public void GetFacesBasedOnNormals(){

		//currently we have top and bottom face 2 times

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

			if (faceFound == null) {
				GameObject newFace = Instantiate (FacePrefab);
				newFace.transform.SetParent (transform.GetChild(0));
				faces [faceCount] = newFace.GetComponent<Face> ();

				// Check if it is top face? Or not create new face if it is the top/bottom face
				if (vertices [i].normal.x == 0 && vertices [i].normal.z == 0) {

					switch (typeOfObject) {
					case ObjectType.triangle:
						faces [faceCount].InitializeFace (3);
						break;
					case ObjectType.square:
						faces [faceCount].InitializeFace (4);
						break;
					case ObjectType.octagon:
						faces [faceCount].InitializeFace (6);
						break;
					case ObjectType.hexagon:
						faces [faceCount].InitializeFace (8);
						break;
					}
						
					//if direction is up, it is the top face (number of vertices depends on type)
					if (vertices [i].normal.y > 0) {
						faces [faceCount].SetType (Face.faceType.TopFace);
						topFace = newFace.GetComponent<Face>();

					//if direction is down, it is the bottom face (number of vertices depends on type)
					} else {
						faces [faceCount].SetType (Face.faceType.BottomFace);
						bottomFace = newFace.GetComponent<Face>();
					}

				// others are side faces (4 vertices)
				}  else {
					faces [faceCount].InitializeFace (4);
					faces [faceCount].SetType (Face.faceType.SideFace);
				}

				faces [faceCount].normal = vertices [i].normal;

				faceCount++;
			}
		}
	}


	public Face GetFaceFromCollisionCoordinate(Vector3 pointOfCollision) {

		// use dot product to check if point lies on a face

		for (int i = 0; i < faces.Length; i++) {

			Debug.Log ("Collision Point " + pointOfCollision);
			Debug.Log ("Other Point " + faces[i].vertexBundles[0].transform.GetChild(0).position);

			Debug.Log("Dot product: " + (Vector3.Dot((faces[i].vertexBundles[0].transform.GetChild(0).position - pointOfCollision), faces[i].normal)));

			if (Mathf.Abs(Vector3.Dot((faces[i].vertexBundles[0].transform.GetChild(0).position - pointOfCollision), faces[i].normal)) <= 0.1){
				return faces [i];
			}
		}

		return null;
	}
		
}