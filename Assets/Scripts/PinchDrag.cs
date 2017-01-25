using UnityEngine;
using System.Collections;

public class PinchDrag : MonoBehaviour {

    public GameObject image;
    public float imageWidth;
    public float imageHeight;

    private Vector2 originalCenter;

    private float scaling = 1f;

    private Vector2 firstPos;
    private Vector2 secPos;
    private Vector2 prevPos = Vector2.zero;
    private Vector2 panVelocity = Vector2.zero;
    private float decelerator = 1000f;

    // panning limit
    private float limitX = 0f;
    private float limitY = 0f;

    private float halfWidth;
    private float halfHeight;

    private bool pinching = false;

	// Use this for initialization
	void Start () {
        halfWidth = 0.5f * imageWidth;
        halfHeight = 0.5f * imageHeight;

        originalCenter = image.transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.touchCount == 2)  // pinching; zooming in/out
        {
            pinching = true;

            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            if (touch0.phase == TouchPhase.Ended || touch1.phase == TouchPhase.Ended) {
                pinching = false;
                if (touch1.phase == TouchPhase.Ended) // if it was the 2nd touch that ended
                    prevPos = touch0.position;
                else if (touch0.phase == TouchPhase.Ended)
                    prevPos = touch1.position;

                return;
            }

            Vector2 touch0PrevPos = touch0.position - touch0.deltaPosition;
            Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;

            // get the magnitudes
            float prevTouchDeltaMag = (touch0PrevPos - touch1PrevPos).magnitude;
            float touchDeltaMag = (touch0.position - touch1.position).magnitude;

            float deltaMagDiff = prevTouchDeltaMag - touchDeltaMag;
            if (deltaMagDiff < 0 && scaling < 2.5f /*&& limitX <  640f && limitY < 340f*/)    // zoom in
            {
                float newScale = image.transform.localScale.x - deltaMagDiff * 0.5f * Time.deltaTime;
                if (newScale > 2.5f)
                    newScale = 2.5f;

                image.transform.localScale = new Vector3(newScale, newScale, 1);

                // update the storage values
                UpdateValuesAfterZoom();
            }
            else if (deltaMagDiff > 0 && scaling > 1f)  // zoom out
            {
                float newScale = image.transform.localScale.x - deltaMagDiff * 0.5f * Time.deltaTime;
                if (newScale < 1f)
                    newScale = 1f;
                image.transform.localScale = new Vector3(newScale, newScale, 1);

                // update the storage values
                UpdateValuesAfterZoom();
            }
        }
        //else if (Input.touchCount == 1)
        //{
        //    if (scaling > 1f)
        //    {
        //        Touch touch = Input.GetTouch(0);
        //        if (touch.phase == TouchPhase.Began)
        //            firstPos = new Vector2(touch.position.x, touch.position.y);
        //        else if (touch.phase == TouchPhase.Moved)
        //        {
        //            Vector2 secPos = new Vector2(touch.position.x, touch.position.y);
        //
        //            // pan according to swipe/drag
        //            float xDiff = secPos.x - firstPos.x;
        //            float yDiff = secPos.y - firstPos.y;
        //
        //            if (xDiff > 0f && deltaPosX < limitX)
        //            {
        //                deltaPosX += xDiff * Time.deltaTime;
        //                image.transform.localPosition = new Vector2(deltaPosX, deltaPosY);
        //            }
        //            else if (xDiff < 0f && deltaPosX > -limitX)
        //            {
        //                deltaPosX += xDiff * Time.deltaTime;
        //                image.transform.localPosition = new Vector2(deltaPosX, deltaPosY);
        //            }
        //
        //            if (yDiff > 0f && deltaPosY < limitY)
        //            {
        //                deltaPosY += yDiff * Time.deltaTime;
        //                image.transform.localPosition = new Vector2(deltaPosX, deltaPosY);
        //            }
        //            else if (yDiff < 0f && deltaPosY > -limitY)
        //            {
        //                deltaPosY += yDiff * Time.deltaTime;
        //                image.transform.localPosition = new Vector2(deltaPosX, deltaPosY);
        //            }
        //        }
        //    }
        //}

        /*else if (Input.touchCount == 1)
        {
            if (scaling > 1f)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    firstPos = new Vector2(touch.position.x, touch.position.y);
                    if (!panVelocity.Equals(Vector2.zero))
                    {
                        if (panVelocity.x > 0f)
                        {
                            panVelocity.x -= 2f * decelerator * Time.deltaTime;
                            if (panVelocity.x < 0f)
                                panVelocity.x = 0f;
                        }
                        else if (panVelocity.x < 0f)
                        {
                            panVelocity.x += 2f * decelerator * Time.deltaTime;
                            if (panVelocity.x > 0f)
                                panVelocity.x = 0f;
                        }

                        if (panVelocity.y > 0f)
                        {
                            panVelocity.y -= 2f * decelerator * Time.deltaTime;
                            if (panVelocity.y < 0f)
                                panVelocity.y = 0f;
                        }
                        else if (panVelocity.y < 0f)
                        {
                            panVelocity.y += 2f * decelerator * Time.deltaTime;
                            if (panVelocity.y > 0f)
                                panVelocity.y = 0f;
                        }
                    }
                }

                else if (touch.phase == TouchPhase.Moved)
                {
                    secPos = new Vector2(touch.position.x, touch.position.y);
                    panVelocity = secPos - firstPos;
                }

            }
        }

        if (!panVelocity.Equals(Vector2.zero))
        {
            // set the new position of the image
            float deltaX = panVelocity.x * Time.deltaTime;
            float deltaY = panVelocity.y * Time.deltaTime;

            image.transform.localPosition += new Vector3(deltaX, deltaY, 0);

            // change the velocity
            if (panVelocity.x > 0f)
            {
                panVelocity.x -= decelerator * Time.deltaTime;
                if (panVelocity.x < 0f)
                    panVelocity.x = 0f;
            }
            else if (panVelocity.x < 0f)
            {
                panVelocity.x += decelerator * Time.deltaTime;
                if (panVelocity.x > 0f)
                    panVelocity.x = 0f;
            }

            if (panVelocity.y > 0f)
            {
                panVelocity.y -= decelerator * Time.deltaTime;
                if (panVelocity.y < 0f)
                    panVelocity.y = 0f;
            }
            else if (panVelocity.y < 0f)
            {
                panVelocity.y += decelerator * Time.deltaTime;
                if (panVelocity.y > 0f)
                    panVelocity.y = 0f;
            }
        }*/

        else if (Input.touchCount == 1)
        {
            if (pinching == true)   // player let go of 1 finger only
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Stationary)
                {
                    pinching = false;
                    prevPos = new Vector2(touch.position.x, touch.position.y);
                    return;
                }
            }

            if (scaling > 1f)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                    prevPos = new Vector2(touch.position.x, touch.position.y);

                if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
                {
                    Vector2 tempPos = new Vector2(touch.position.x, touch.position.y);

                    //float deltaX = tempPos.x - prevPos.x;
                    //float deltaY = tempPos.y - prevPos.y;
                    //
                    //image.transform.localPosition += new Vector3(deltaX, deltaY, 0);

                    panVelocity = 10f * (tempPos - prevPos);
                    prevPos = tempPos;
                }

                else if (touch.phase == TouchPhase.Ended)
                {
                    secPos = new Vector2(touch.position.x, touch.position.y);
                    panVelocity = 10f * (secPos - prevPos);

                    prevPos = Vector2.zero;
                }
                
            }
        }

        if (!panVelocity.Equals(Vector2.zero))
        {
            // set the new position of the image
            float newX = image.transform.localPosition.x + panVelocity.x * Time.deltaTime;
            float newY = image.transform.localPosition.y + panVelocity.y * Time.deltaTime;

            if (newX < -limitX)
                newX = -limitX;
            else if (newX > limitX)
                newX = limitX;
            if (newY < -limitY)
                newY = -limitY;
            else if (newY > limitY)
                newY = limitY;

            image.transform.localPosition = new Vector3(newX, newY, 0);
            
            // change the velocity
            if (panVelocity.x > 0f)
            {
                panVelocity.x -= decelerator * Time.deltaTime;
                if (panVelocity.x < 0f)
                    panVelocity.x = 0f;
            }
            else if (panVelocity.x < 0f)
            {
                panVelocity.x += decelerator * Time.deltaTime;
                if (panVelocity.x > 0f)
                    panVelocity.x = 0f;
            }

            if (panVelocity.y > 0f)
            {
                panVelocity.y -= decelerator * Time.deltaTime;
                if (panVelocity.y < 0f)
                    panVelocity.y = 0f;
            }
            else if (panVelocity.y < 0f)
            {
                panVelocity.y += decelerator * Time.deltaTime;
                if (panVelocity.y > 0f)
                    panVelocity.y = 0f;
            }
        }


    }

    private void UpdateValuesAfterZoom()
    {
        scaling = image.transform.localScale.x;
        limitX = (scaling - 1f) * halfWidth + originalCenter.x;
        limitY = (scaling - 1f) * halfHeight + originalCenter.y;
    }
}
