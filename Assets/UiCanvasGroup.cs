using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UiCanvasGroup : MonoBehaviour {
    public bool visible;
    public GameObject player;
    private CanvasGroup canvGroup;

	// Use this for initialization
	void Start () {
        visible = false;
        canvGroup = this.GetComponent<CanvasGroup>();
    }
	
	// Update is called once per frame
	void Update () {
        if (visible)
        {
            transform.LookAt(player.transform);
        }

    }

    public void Show()
    {
        LeanTween.alphaCanvas(canvGroup, 1f, 0.3f);
        visible = true;
    }

    public void Hide()
    {
        LeanTween.alphaCanvas(canvGroup, 0f, 0.3f);
        visible = false;
    }
}
