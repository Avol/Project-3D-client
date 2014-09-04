using UnityEngine;
using System.Collections;

public class TokenScript : ClickableObjectScript {

	private StateMachine script1;
	
	void Start ()
	{
		script1 = GameObject.Find("Player").GetComponent<StateMachine>();
		name = "token";
	}

	override public void OnMouseDown ()
	{
		script1.EndPoint = this.gameObject.transform.FindChild("Trigger").transform;
		script1.SetStateToMove();
		script1.Target = this.gameObject;
	}
}
