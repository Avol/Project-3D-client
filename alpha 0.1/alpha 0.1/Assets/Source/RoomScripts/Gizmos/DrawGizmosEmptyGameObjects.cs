using UnityEngine;
using System.Collections;

public class DrawGizmosEmptyGameObjects : MonoBehaviour {


    public float heightY = 1.0f;
    public float widthX = 1.0f;
    public float lengthZ = 1.0f;

    public Color color;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
		
    }

    void OnDrawGizmos() {
    	if (color.a < 10) color.a = 255;
        if (this.transform.childCount == 0) {
            Gizmos.color = new Color(color.r, color.g, color.b, color.a);
            Gizmos.DrawCube(transform.position, new Vector3(widthX, heightY, lengthZ));
            gameObject.transform.localScale = new Vector3(widthX, heightY, lengthZ);
        }

    }
}
