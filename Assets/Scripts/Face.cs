using UnityEngine;
using System.Collections;

public class Face : MonoBehaviour {

    public VertexBundle[] vertexBundles;
    public VertexBundle center;
    public Vector3 centerPosition;
    public handle scaleHandle;
    public handle centerHandle;
    public handle heightHandle;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
	    if (center != null && centerPosition != center.coordinates)
        {
            UpdateFaceFromCenter();
        }
	}

    public void scaleFace(float amount)
    {
        for (int i = 0; i < vertexBundles.Length; i++)
        {
            if (vertexBundles[i] != null)
            {
                Vector3 VertexToCenter = vertexBundles[i].coordinates - centerPosition;
                vertexBundles[i].coordinates = vertexBundles[i].coordinates + VertexToCenter * amount;
            }

        }
    }

    public void UpdateFaceFromCenter()
    {
        Vector3 difference = center.coordinates - centerPosition;
        centerPosition = center.coordinates;

        // go through each vertex and apply a similar translation
        for (int i=0; i<vertexBundles.Length; i++)
        {
            if (vertexBundles[i] != center)
            {
                vertexBundles[i].coordinates += difference;
            }

        }

        scaleHandle.transform.localPosition = centerPosition;
        centerHandle.transform.localPosition = centerPosition;
        if (heightHandle!= null)
        {
            heightHandle.transform.localPosition = centerPosition;
        }

    }


}
