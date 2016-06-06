using UnityEngine;
using System.Collections;

public class Selection : Singleton<Selection>{

    public GameObject currentFocus;
    public GameObject currentSelection;
	public Vector3 pointOfCollision;
	public Face collidingFace;
    public bool mousePressed;
    private float temps;
	private bool faceSelection;

    // Use this for initialization
    void Start () {
	
	}

	void DeFocusCurrent(){

		if (currentFocus != null) {
			if (currentFocus.CompareTag ("ModelingObject")) {
				currentFocus.GetComponent<ModelingObject> ().UnFocus ();
			} else if (currentFocus.CompareTag ("Handle")) {
				Debug.Log ("Defocus handle");
				currentFocus.GetComponent<handle> ().UnFocus ();
			} else if (currentFocus.CompareTag ("UiElement")) {
				currentFocus.GetComponent<UiElement> ().UnFocus ();
			}

			currentFocus = null;
		}

	}

	
	// Update is called once per frame
	void Update () {

		RaycastHit hit;

		// Fix point for raycast!
		Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));


		if (Physics.Raycast (ray, out hit)) {
			
			if (hit.rigidbody != null) {
				if (currentFocus != hit.rigidbody.transform.parent.gameObject) {
					DeFocusCurrent ();
					// Set new focus
					currentFocus = hit.rigidbody.transform.parent.gameObject;

					// focus of Object
					if (currentFocus.CompareTag ("ModelingObject")) {
						currentFocus.GetComponent<ModelingObject> ().Focus ();
					} else if (currentFocus.CompareTag ("Handle")) {
						currentFocus.GetComponent<handle> ().Focus ();
					} else if (currentFocus.CompareTag ("UiElement")) {
						currentFocus.GetComponent<UiElement> ().Focus ();
					}
				}

				// Set position of collision
				pointOfCollision = hit.point;

			} else {

				if (currentFocus != null) {
					DeFocusCurrent ();
				}
			}
		} else {
			if (currentFocus != null) {
				DeFocusCurrent ();
			}
		}

			

        if (currentFocus != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                mousePressed = true;
                temps = Time.time;
            }

            if (mousePressed && currentFocus.CompareTag("ModelingObject") && Time.time - temps > 0.2f)
            {
				currentFocus.GetComponent<ModelingObject>().MoveObject();
                UiCanvasGroup.Instance.Hide();
            }
            else if (mousePressed && currentFocus.CompareTag("Handle"))
            {
                currentFocus.GetComponent<handle>().ApplyChanges();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            mousePressed = false; 

            if (currentFocus != null)
            {
                if (currentFocus.CompareTag("ModelingObject") && Time.time - temps <= 0.2f)
                {
					if (!faceSelection) {
						UiCanvasGroup.Instance.Show ();
						currentFocus.GetComponent<ModelingObject> ().Select ();
						collidingFace = null;
					} else {
						FindCollidingFace (pointOfCollision);
						collidingFace.CreateNewModelingObject ();
					}


                } else if (currentFocus.CompareTag("Handle"))
                {
                    currentFocus.GetComponent<handle>().updateHandlePosition();
                    currentFocus.GetComponent<handle>().UnFocus(); 

                } else if (currentFocus.CompareTag("UiElement"))
                {
                    currentFocus.GetComponent<UiElement>().goal.ActivateMenu();
				} 

            } else
            {
                UiCanvasGroup.Instance.Hide();
            }

            DeAssignCurrentSelection(currentSelection);

            temps = 0;
        }
    }

	public void FindCollidingFace(Vector3 pointOfCollision){
		collidingFace = UiCanvasGroup.Instance.currentModelingObject.GetFaceFromCollisionCoordinate (pointOfCollision);
		Debug.Log("Selected face " + collidingFace);
	}

    public void AssignCurrentFocus(GameObject newCollider)
    {
		currentFocus = newCollider;
        temps = 0f;
    }

    public void DeAssignCurrentFocus(GameObject collider)
    {
        if (currentFocus == collider)
        {
            currentFocus = null;
        }

        temps = 0f;
    }

    public void AssignCurrentSelection(GameObject newCollider)
    {
        currentSelection = newCollider;
    }

    public void DeAssignCurrentSelection(GameObject collider)
    {
        if (currentSelection == collider)
        {
            currentSelection = null;
        }
    }

	public void enableFaceSelection(bool value){
		faceSelection = value;	
	}


}
