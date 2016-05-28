using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UiElement : MonoBehaviour {

    public bool focused;
    public RectTransform title;

	// Use this for initialization
	void Start () {
        focused = false;
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    public void Focus()
    {
        focused = true;
        LeanTween.alphaText(title, 1.0f, 0.2f);
        LeanTween.scale(this.gameObject, new Vector3(1.2f, 1.2f, 1.2f), 0.3f);
    }

    public void UnFocus()
    {
        focused = false;
        LeanTween.alphaText(title, 0.0f, 0.2f);
        LeanTween.scale(this.gameObject, new Vector3(1.0f, 1.0f, 1.0f), 0.3f);
    }
}
