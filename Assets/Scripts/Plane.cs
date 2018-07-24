using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Plane : MonoBehaviour {

	public GameObject linePrefab;

	Line activeLine;
	bool done = false;
    List<Vector2> points;
    Rigidbody2D body;
    // Use this for initialization
    void Start () {
        body = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () 
	{

        if (Input.GetMouseButtonUp(0))
		{
			//activeLine = null;
			done = true;
            points = activeLine.getPoints();
            StartCoroutine(StartFollowing());
        }

		if (activeLine != null && done != true)
		{
			Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			activeLine.UpdateLine(mousePos);
		}
	}

	void OnMouseDown() 
	{
		if (activeLine != null) 
		{
			activeLine.removeOldLine ();
			activeLine = null;
		}

		GameObject lineGO = Instantiate(linePrefab);
		activeLine = lineGO.GetComponent<Line>();
		done = false;
		Debug.Log ("asdasd");

	}

	IEnumerator StartFollowing () {

		int  counter = 0;
		Vector3 pointA = transform.position;
		Vector3 pointB = points[counter];
		Debug.Log ("pointA:"+pointA);
		Debug.Log ("pointB:"+pointB);
		while (counter<= points.Count-1) {
            FaceMoveDirection(pointB);
            yield return StartCoroutine(MoveObject(transform, pointA, pointB, 3.0f));
			counter++;
			if(counter<= points.Count-1)
			{
				pointA=pointB;
				pointB = points[counter];
			}

			//yield return StartCoroutine(MoveObject(transform, pointB, pointA, 3.0f));
		}
	}

	IEnumerator MoveObject (Transform thisTransform, Vector3 startPos, Vector3 endPos, float time) {
		var i= 0.0f;
		var rate= 6.0f/time;
		while (i < 1.0f) {
			i += Time.deltaTime * rate;
			thisTransform.position = Vector3.Lerp(startPos, endPos, i);

            //FaceMoveDirection(endPos);
            //TransformArrow(startPos,endPos);
            // transform.Rotate(Vector3.down * 10f * Time.deltaTime);
             Vector3 temp = new Vector3(endPos.x,endPos.y,0.0f);
            //transform.rotation = Quaternion.LookRotation(temp);
            /*if (temp != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(temp),
                    Time.deltaTime * 1f
                );
            }*/
            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(transform.position, endPos - transform.position), Time.deltaTime);
            //LookAt(endPos);


            yield return null; 
		}
	}

    public void FaceMoveDirection(Vector3 target)
    {
        Vector3 diff = target - transform.position;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, (rot_z - 90));
    }

    void TransformArrow(Vector3 start, Vector3 end)
    {
        // This is similar to what you already have.
        Vector3 arrowPosition = (start + end) / 2f;
        transform.position = arrowPosition;

        Vector3 direction = (end - start);

        // If you want to prevent the arrow from tilting up or down on slopes,
        // uncomment this line.
        // direction.y = 0f;

        Quaternion arrowOrientation = Quaternion.LookRotation(direction);

        // Your prefab arrow points along -x ("left"), with +z ("forward") pointing up.
        // We want to rotate this to Unity's standard: +z forward, y+ up.
        // We'll construct the orientation facing "left" with "forward" up, then invert it.

        Quaternion correction = Quaternion.Inverse(
                                   Quaternion.LookRotation(Vector3.left, Vector3.right)
                                );

        // Now we apply our orientation and the correction for the prefab's orientation.
        transform.rotation = arrowOrientation * correction;
    }

    protected void LookAt(Vector2 point)
    {

        float angle = AngleBetweenPoints(transform.position, point);
        var targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime);

    }
    float AngleBetweenPoints(Vector2 a, Vector2 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }


}
