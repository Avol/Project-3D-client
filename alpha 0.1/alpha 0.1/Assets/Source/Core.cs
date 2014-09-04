using UnityEngine;
using System.Collections;

public class Core : MonoBehaviour {
    public Interaction interaction;
    public ProfileManager profileManager;

	void Awake () {
        this.interaction = new Interaction(this);
        this.profileManager = new ProfileManager(this);
	}
}