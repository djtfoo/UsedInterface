using UnityEngine;
using System.Collections;

public class ClosePopUp : MonoBehaviour {

    public GameObject toDestroy;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ClosePopup()
    {
        Destroy(toDestroy);
    }
}
