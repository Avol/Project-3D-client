using UnityEngine;
using System.Collections;

/// <summary>
/// Clickable object script is attached to all interactive Objects.
/// </summary>

public class ClickableObjectScript : MonoBehaviour 
{
	public Material normal;
	public Material iluminated;
	
	private string name;
	private float play;
	private ParticleEmitter particle;
	
	
	void Awake () 
	{
		particle = this.gameObject.transform.FindChild("SparkleParticles").GetComponent<ParticleEmitter>();
	}
	
	void Start ()
	{
		particle.emit = false;
	}
	
	void Update () 
	{
		play = Mathf.PingPong(Time.time, 3);
		if (play > 2.9)
		{
			particle.emit = true	;		
		}
		else
		{
			particle.emit = false;
		}

	}
	
	void OnMouseOver () 
	{	
		this.gameObject.renderer.material = iluminated;
	}
	
	void OnMouseExit ()
	{
		this.gameObject.renderer.material = normal;
	}
	
	virtual public void OnMouseDown ()
	{
		// To be overridden
	}
}