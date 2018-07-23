using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Plane : MonoBehaviour {

	public GameObject linePrefab;

	Line activeLine;
	bool done = false;
    List<Vector2> points;
    private Vector3 currentAngle;

    // Use this for initialization
    void Start () {
        currentAngle = transform.eulerAngles;
    }
	
	// Update is called once per frame
	void Update () 
	{
        currentAngle = new Vector3(currentAngle.x, currentAngle.y, 90);

        transform.eulerAngles = currentAngle;

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
			yield return null; 
		}
	}


}
