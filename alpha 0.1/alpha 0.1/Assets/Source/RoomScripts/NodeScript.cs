using UnityEngine;
using System.Collections;

public class NodeScript : MonoBehaviour {

	public NodeScript[] links;
	public Color color;
	public string name;
	
	private Vector3 pos;
	
	void Awake ()
	{
		pos = this.gameObject.transform.position;
		name = "node";
	}

	public NodeScript[] Links
	{
		get {return links;}
	}
	
	public GameObject ThisObject
	{
		get {return this.gameObject;}
	}
	
	public Vector3 Pos
	{
		get {return pos;}
	}
	
	private void OnDrawGizmos()
	{
		for (int i = 0; i < links.Length; i++)
		{
			Gizmos.color = new Color (color.r, color.g, color.b);
			Gizmos.DrawLine(gameObject.transform.position, links[i].transform.position);
			
			Gizmos.DrawSphere(transform.position, 0.5F);
		}
	}
}
