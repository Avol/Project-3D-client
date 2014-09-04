using UnityEngine;
using System.Collections;

public class StateMachine : MonoBehaviour 
{

	private State EXPLORE;
	private State PLAY;
	private State INTERACT;
	private State MOVE;
	private State currentState;
	
	private Transform startPoint;
	private Transform endPoint;
	private Transform camera;
	private GameObject target;
	
	private float range;
	
	private NodeScript prevNode;
	private NodeScript startNode;
	private NodeScript targetNode;
	
	
	public enum States
	{
		EXPLORE,
		PLAY,
		INTERACT,
		MOVE
	}

	void Awake () 
	{
		EXPLORE = new ExploreMode();
		PLAY = new PlayMode();
		INTERACT = new InteractMode();
		MOVE = new MoveMode();
		
		startPoint = GameObject.Find("StartPosition").transform;
		camera = this.gameObject.transform.FindChild("Main Camera").transform;
		
		range = 0.05F;
		
		startNode = GameObject.Find("Node1").GetComponent<NodeScript>();
		
	}
	
	void Start ()
	{
		currentState = EXPLORE;
		targetNode = startNode;
	}
	
	void Update () 
	{
		StateManagement();
		Debug.Log(currentState);
	}
	
	void StateManagement ()
	{
		if (currentState == EXPLORE)
		{
			camera.rotation = Quaternion.Slerp(camera.rotation, Quaternion.LookRotation(targetNode.transform.position - camera.position), Time.deltaTime);
		}
		
		if (currentState == MOVE)
		{
			prevNode = targetNode;
		
			this.gameObject.transform.position = Vector3.Lerp(this.gameObject.transform.position, endPoint.position, Time.deltaTime);
			camera.rotation = Quaternion.Slerp(camera.rotation, Quaternion.LookRotation(target.transform.position - camera.position), Time.deltaTime);
			
			float distance = Vector3.Distance(this.gameObject.transform.position, endPoint.position);
			if (distance < range)
			{
				if (target.name == "game")
				{
					currentState = PLAY;
				}
				
				if (target.name == "token")
				{
					currentState = INTERACT;
				}
				
				if(target == targetNode.ThisObject)
				{
					currentState = EXPLORE;
				}
			}
		}
		
		if (currentState == PLAY)
		{
			
		}
		
		if (currentState == INTERACT)
		{
			
		}
	}
	
	void OnGUI ()
	{
		if (currentState == EXPLORE)
		{
			if (GUI.Button(new Rect(10, Screen.height/2 - 25, 100, 50), "Left"))
			{
				targetNode = targetNode.Links[1];
			}
			
			if (GUI.Button(new Rect(Screen.width - 110, Screen.height/2 - 25, 100, 50), "Right"))
			{
				targetNode = targetNode.Links[0];
			}
		}
		
		if (currentState == PLAY)
		{
			if (GUI.Button(new Rect(10, Screen.height/2 - 25, 100, 50), "Back"))
			{				
				endPoint = startPoint;
				target = prevNode.ThisObject;
				currentState = MOVE;
			}
							
			if (GUI.Button(new Rect(Screen.width - 110, Screen.height/2 - 25, 100, 50), "Play Game"))				
			{
				print ("Playing the game");
			}
		}
		
		if (currentState == INTERACT)
		{
			if (GUI.Button(new Rect(10, Screen.height/2 - 25, 100, 50), "Back"))
			{				
				endPoint = startPoint;
				target = prevNode.ThisObject;
				currentState = MOVE;
			}
			
			if (GUI.Button(new Rect(Screen.width - 110, Screen.height/2 - 25, 100, 50), "Interact"))				
			{
				print ("Interact");
			}
		}
	}
	
	public void SetStateToMove()
	{
		currentState = MOVE;
	}
	
	public Transform EndPoint
	{
		get {return endPoint;}
		set {endPoint = value;}
	}
	
	public GameObject Target
	{
		get {return target;}
		set {target = value;}
	}
}
