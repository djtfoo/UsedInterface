using UnityEngine;
using System.Collections;

public class OpenPopUp : MonoBehaviour {

    public Transform popUpPrefab;

    Transform popUp;

	// Use this for initialization
	void Start ()
    {

	}
	
	// Update is called once per frame
	void Update () {
	
        if (popUp && popUp.localScale.x < 1)
        {
            float newScale = popUp.localScale.x + 4 * Time.deltaTime;

            if (newScale > 1) {
                popUp.localScale = new Vector3(1, 1, 1);
            }
            else {
                popUp.localScale = new Vector3(newScale, newScale, 1);
            }
        }

	}

    public void CreatePopUp()
    {
        popUp = (Transform)Instantiate(popUpPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        //clone.SetParent(gameObject.transform);    // set the parent as transform of popupHandler
        popUp.SetParent(GameObject.Find("Canvas").transform);
        popUp.localPosition = popUpPrefab.localPosition;

        popUp.localScale = new Vector3(0.1f, 0.1f, 1);
    }
}
