using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour {

	public LineRenderer lineRenderer;

	List<Vector2> points;

	public void UpdateLine (Vector2 mousePos)
	{
		if (points == null)
		{
			points = new List<Vector2>();
			SetPoint(mousePos);
			return;
		}

		if (Vector2.Distance(points.Last(), mousePos) > .1f)
			SetPoint(mousePos);
	}

	void SetPoint (Vector2 point)
	{
		points.Add(point);

		lineRenderer.positionCount = points.Count;
		lineRenderer.SetPosition(points.Count - 1, point);

	}

	public void removeOldLine()
	{

		lineRenderer.SetVertexCount(0);
		if(points != null)
			points.RemoveRange(0,points.Count);
		
	}

	public List<Vector2> getPoints()
	{
		return points;
	}

}
