using UnityEngine;
using System.Collections;

public class ModelingObject : MonoBehaviour
{
    public handles handles;
    private UiCanvasGroup ui; 
    public bool focused;

    public Vertex[] vertices;
    private Mesh mesh;
    private MeshCollider meshCollider;
    private Vector3[] MeshCordinatesVertices;
    private Vector2[] MeshUV;
    private int[] MeshTriangles;

    public GameObject[] vertexBundles;
    public Face topFace;
    public Face bottomFace;
    public Color color;
    public Vertex centerTop;
    public Vertex centerBottom;

    public GameObject VertexPrefab;
    public GameObject VertexBundlePrefab;

    public enum ObjectType
    {
        triangle = 0,
        square = 1,
        hexagon = 2,
        octagon = 3
    }

    // Use this for initialization
    void Start()
    {
        focused = false;
        handles.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        ui = GameObject.Find("UI Elements").GetComponent<UiCanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Initiate()
    {
        mesh = this.transform.GetChild(0).GetComponent<MeshFilter>().mesh;
        meshCollider = this.transform.GetChild(0).GetComponent<MeshCollider>();
        MeshCordinatesVertices = mesh.vertices;
        MeshTriangles = mesh.triangles;
        MeshUV = mesh.uv;

        vertices = new Vertex[MeshCordinatesVertices.Length];

        // Vector3[] normals = mesh.normals;
        int i = 0;

        Vector3 centerTopVector = new Vector3(0, 0, 0);
        Vector3 centerBottomVector = new Vector3(0, 0, 0);

        while (i < MeshCordinatesVertices.Length)
        {
            GameObject vert = Instantiate(VertexPrefab);

            if (MeshCordinatesVertices[i].y > 0.0f)
            {
                vert.transform.SetParent(topFace.gameObject.transform);
                centerTopVector.x += MeshCordinatesVertices[i].x;
                centerTopVector.y += MeshCordinatesVertices[i].y;
                centerTopVector.z += MeshCordinatesVertices[i].z;
            }
            else
            {
                vert.transform.SetParent(bottomFace.gameObject.transform);
                centerBottomVector.x += MeshCordinatesVertices[i].x;
                centerBottomVector.y += MeshCordinatesVertices[i].y;
                centerBottomVector.z += MeshCordinatesVertices[i].z;
            }

            vert.transform.localPosition = MeshCordinatesVertices[i];
            vertices[i] = vert.GetComponent<Vertex>();
            i++;
        }

        centerBottomVector.x = centerBottomVector.x / (MeshCordinatesVertices.Length / 2.0f);
        centerBottomVector.y = centerBottomVector.y / (MeshCordinatesVertices.Length / 2.0f);
        centerBottomVector.z = centerBottomVector.z / (MeshCordinatesVertices.Length / 2.0f);

        centerTopVector.x = centerTopVector.x / (MeshCordinatesVertices.Length / 2.0f);
        centerTopVector.y = centerTopVector.y / (MeshCordinatesVertices.Length / 2.0f);
        centerTopVector.z = centerTopVector.z / (MeshCordinatesVertices.Length / 2.0f);

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

        bundleSimilarVertices(topFace.gameObject.transform);
        bundleSimilarVertices(bottomFace.gameObject.transform);

        topFace.center = centerTop.transform.parent.GetComponent<VertexBundle>();
        bottomFace.center = centerBottom.transform.parent.GetComponent<VertexBundle>();

        topFace.vertexBundles = topFace.GetComponentsInChildren<VertexBundle>();
        topFace.centerPosition = topFace.center.coordinates;
        bottomFace.vertexBundles = bottomFace.GetComponentsInChildren<VertexBundle>();
        bottomFace.centerPosition = bottomFace.center.coordinates;

        InitiateHandles();
    }


    public void bundleSimilarVertices(Transform face)
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

                if (vertexBundle.transform.childCount == 0)
                {
                    Destroy(vertexBundle);
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
        focused = true;
    }

    public void UnFocus()
    {
        focused = false;

    }

    public void Select()
    {
        ui.transform.position = transform.position;
        ui.Show();
        handles.gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void DeSelect()
    {
        ui.Hide();
        handles.gameObject.transform.GetChild(0).gameObject.SetActive(false);
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


}