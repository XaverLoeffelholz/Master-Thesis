using UnityEngine;
using System.Collections;

public class ModelingObject : MonoBehaviour {

    public Vertex[] vertices;
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

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Initiate()
    {
        Mesh mesh = this.transform.GetChild(0).GetComponent<MeshFilter>().mesh;
        Vector3[] coordinatesVertices = mesh.vertices;

        vertices = new Vertex[coordinatesVertices.Length];

        // Vector3[] normals = mesh.normals;
        int i = 0;

        Vector3 centerTopVector = new Vector3(0, 0, 0);
        Vector3 centerBottomVector = new Vector3(0, 0, 0);

        while (i < coordinatesVertices.Length)
        {
            GameObject vert = Instantiate(VertexPrefab);

            if (coordinatesVertices[i].y > 0.0f)
            {
                vert.transform.SetParent(topFace.gameObject.transform);
                centerTopVector.x += coordinatesVertices[i].x;
                centerTopVector.y += coordinatesVertices[i].y;
                centerTopVector.z += coordinatesVertices[i].z;
            }
            else
            {
                vert.transform.SetParent(bottomFace.gameObject.transform);
                centerBottomVector.x += coordinatesVertices[i].x;
                centerBottomVector.y += coordinatesVertices[i].y;
                centerBottomVector.z += coordinatesVertices[i].z;
            }

            vert.transform.localPosition = coordinatesVertices[i];
            vertices[i] = vert.GetComponent<Vertex>();
            i++;
        }

        centerBottomVector.x = centerBottomVector.x / (coordinatesVertices.Length / 2.0f);
        centerBottomVector.y = centerBottomVector.y / (coordinatesVertices.Length / 2.0f);
        centerBottomVector.z = centerBottomVector.z / (coordinatesVertices.Length / 2.0f);

        centerTopVector.x = centerTopVector.x / (coordinatesVertices.Length / 2.0f);
        centerTopVector.y = centerTopVector.y / (coordinatesVertices.Length / 2.0f);
        centerTopVector.z = centerTopVector.z / (coordinatesVertices.Length / 2.0f);



        // Similar Vertices should be bundled

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

        mesh.vertices = coordinatesVertices;

        bundleSimilarVertices(topFace.gameObject.transform);
        bundleSimilarVertices(bottomFace.gameObject.transform);
    }
 

    public void bundleSimilarVertices(Transform face)
    {

        Transform[] allChildren = face.GetComponentsInChildren<Transform>();

        for (int i = 0; i < allChildren.Length; i++)
        {
            Vector3 position = allChildren[i].localPosition;

            // get all Vertex bundles
            foreach (Transform vertexBundle in face) if (vertexBundle.CompareTag("VertexBundle")) {

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

                if (vertexBundle.transform.childCount==0)
                {
                    Destroy(vertexBundle);
                }
            }



        }


    }
}