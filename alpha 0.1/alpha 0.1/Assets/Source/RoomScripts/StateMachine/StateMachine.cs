using UnityEngine;
using System.Collections;

/// <summary>
/// State machine.cs is attached to all the mementos in the levels/scene
/// </summary>

public class StateMachine : MonoBehaviour 
{
	private Interaction interaction;

	private State ZOOMED;
	private State MOVEIN;
	private State MOVEOUT;
	private State PLACED;
	private State currentState;
	
	private Vector3 placedPosition;
	private Vector3 targetPosition;
	private Vector3 camera;
	private float range;
	
	private bool playSound;
	private bool mementoUsed;
	
	public Material normal;
	public Material illuminated;
	public AudioClip[] audioClip;
	public int investigateTime;
	
	public bool toggleZoom;
	public bool toggleRotate;
	public bool toggleAnimate;
	public bool togglePlaySound;
	
	private static bool usingMemento;
	
	void Awake ()
	{
		
		ZOOMED = new ZoomedMode();
		MOVEIN = new MoveInMode();
		MOVEOUT = new MoveOutMode();
		PLACED = new PlacedMode();
		
		currentState = PLACED;
		
		placedPosition = this.gameObject.transform.position;
		camera = GameObject.Find("Player").transform.FindChild("Player Camera").transform.position;
		range = 0.003F;
		
		playSound = false;
		mementoUsed = false;
		usingMemento = false;
		
	}
	
	void Update ()
	{
		Debug.Log(this.gameObject.name + usingMemento);
//		Debug.Log("levelMemento: " + MementoScript.UsedLevelMementos);
//		Debug.Log("totalMemento: " + MementoScript.UsedTotalMementos);

		InteractionManagement();
		StateManagement();
	}
	
	private void StateManagement ()
	{
		if (currentState == PLACED)
		{

		}
		
		if (currentState == MOVEIN)
		{
		//	Will only be executed when object are allowed to zoom, else playsound/animation inmediately
			if (toggleZoom)
			{
				MoveInGameObject();
			}
			else
			{
				currentState = ZOOMED;
			}
		}
	
		if (currentState == ZOOMED)
		{
		//	Will only be executed when objects are allowed to rotate, else there is a time to investigate the object before it returnes
			if (toggleRotate)
			{
				this.gameObject.transform.Rotate(new Vector3(0, 0.5F, 0));
			}
			
			if (togglePlaySound)
			{
				if (!playSound)
				{
					PlaySound(Random.Range(0, audioClip.Length));	
				}
				
			}
			
			if (toggleAnimate)
			{
				Debug.Log("ANIMATION");
			}
			
			if (!toggleAnimate && !togglePlaySound)
			{
				StartCoroutine(Investigate());
			}
		}
		
		if (currentState == MOVEOUT)
		{
			
			if (toggleZoom)
			{
				MoveOutGameObject();
			}
		}
	}
	
	private void InteractionManagement ()
	{
		if (interaction == null)
		{
			interaction = GameObject.Find("Player").transform.FindChild("Player Camera").GetComponent<Core>().interaction;
			
			if (interaction != null)
			{
				interaction.addInteractionObject(this.gameObject, delegate() 
				{
					MouseHandler()	;
				}, 
					delegate() 
				{
					MouseOverHandler();
				}, 
					delegate() 
				{
					MouseOutHandler();
				});
			}
		}
	}
	
	private void MouseHandler ()
	{
		if (!usingMemento)
		{
			if (!mementoUsed)
			{
				MementoScript.UsedLevelMementos++;
				MementoScript.UsedTotalMementos++;
			}
			mementoUsed = true;
			
			currentState = MOVEIN;
			usingMemento = true;
		}
	}
	
	private void MouseOverHandler ()
	{
//		Debug.Log("MouseOver");
		if (currentState == PLACED)
		{	
			renderer.material = illuminated;
		}
	}
	
	private void MouseOutHandler ()
	{
//		Debug.Log("mouseOut");
		if (currentState == PLACED)
		{
			renderer.material = normal;
		}
	}
	
	private void MoveInGameObject ()
	{
		targetPosition = GameObject.Find("Player").transform.FindChild("ZoomedPosition").transform.position;
		this.gameObject.transform.position = Vector3.Lerp(this.gameObject.transform.position, targetPosition, Time.deltaTime);
		
		float distance = Vector3.Distance(this.gameObject.transform.position, targetPosition);
		if(distance < range)
		{
			currentState = ZOOMED;
			if (audioClip.Length != 0)
			{
				playSound = false;
			}
		}
	}
	
	private void MoveOutGameObject ()
	{	
		this.gameObject.transform.position = Vector3.Lerp(this.gameObject.transform.position, placedPosition, Time.deltaTime);
		this.gameObject.transform.rotation = Quaternion.Slerp(this.gameObject.transform.rotation, Quaternion.LookRotation(camera - this.gameObject.transform.position), Time.deltaTime);
		
		float distance = Vector3.Distance(this.gameObject.transform.position, placedPosition);
		if(distance < range)
		{
			usingMemento = false;
			if (!usingMemento)
			{
				currentState = PLACED;
			}
		}
	}
	
	private void PlaySound(int clip)
	{
		audio.clip = audioClip[clip];
		audio.Play();
		StartCoroutine (DelayedSwitchState(audio.clip.length));
		playSound = true;
	}
	
	private IEnumerator Investigate ()
	{
		yield return new WaitForSeconds (investigateTime);
		currentState = MOVEOUT;
		yield return new WaitForSeconds (1);
		StopCoroutine("Investigate");
	}
	
	private IEnumerator DelayedSwitchState(float time)
	{
		yield return new WaitForSeconds (time);
		currentState = MOVEOUT;
		yield return new WaitForSeconds (1);
		StopCoroutine("DelayedSwitchState");
		
	}
}
