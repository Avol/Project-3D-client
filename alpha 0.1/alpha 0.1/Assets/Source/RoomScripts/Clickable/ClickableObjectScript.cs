using UnityEngine;
using System.Collections;

/// <summary>
/// Clickable object script is attached to all interactive Objects.
/// </summary>

public class ClickableObjectScript : MonoBehaviour 
{
	public Material normal;
	public Material iluminated;//correct name
	
	private Interaction interaction;
	
	private string name;
	private float play;
	private ParticleEmitter particle;
	
	void Awake () 
	{
		particle = this.gameObject.transform.FindChild("SparkleParticles").GetComponent<ParticleEmitter>();
		particle.emit = false;
	}
	
	void Update () 
	{
        if (interaction==null)
        {
            interaction = GameObject.Find("Player").transform.FindChild("Main Camera").GetComponent<Core>().interaction;
            if (interaction != null)
            {
                interaction.addInteractionObject(this.gameObject, delegate()
                {
                    OnMouseDown();
                }, delegate()
                {
                    gameObject.renderer.material = iluminated;
                }, delegate()
                {
                    gameObject.renderer.material = normal;
                });
            }
        }

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

    /*void OnMouseOver () 
    {	
        this.gameObject.renderer.material = iluminated;
    }
	
    void OnMouseExit ()
    {
        this.gameObject.renderer.material = normal;
    }*/

    virtual public void OnMouseDown ()
	{
		// To be overridden
	}
}