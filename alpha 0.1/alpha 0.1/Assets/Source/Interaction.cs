using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// @author Donatas Kanapickas
/// </summary>
public class Interaction {
    private Core core;
    private InteractionManager manager;

    public Vector3 leftHandScreenPos { get; private set; }
    public Vector3 rightHandScreenPos { get; private set; }

    public bool isLeftHandGripped { get; private set; }
    public bool isLeftHandReleased { get; private set; }
    public bool isRightHandGripped { get; private set; }
    public bool isRightHandReleased { get; private set; }

    private GUITexture leftHandGUITexture;
    private GUITexture rightHandGUITexture;

    private IList<InteractionObject> interactionObjects;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="core"></param>
    public Interaction(Core core)
    {
        this.core = core;

        this.isLeftHandGripped = false;
        this.isLeftHandReleased = false;
        this.isRightHandGripped = false;
        this.isRightHandReleased = false;

        this.manager = core.GetComponent<InteractionManager>();

        this.leftHandGUITexture = GameObject.Find("LeftHand").GetComponent<GUITexture>();
        this.rightHandGUITexture = GameObject.Find("RightHand").GetComponent<GUITexture>();

        this.interactionObjects = new List<InteractionObject>();

        this.core.StartCoroutine(this.update());
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    IEnumerator update()
    {
        while (true)
        {
            this.extractHandData();
            this.calculateCollisionEvent();
            yield return new WaitForSeconds(0.01f);
        }
    }

    /// <summary>
    /// Extracts hand position, and grip data.
    /// </summary>
    private void extractHandData()
    {
        if (manager != null && manager.IsInteractionInited())
        {
            // extract hand coordinates.
            this.leftHandScreenPos = manager.GetLeftHandScreenPos();
            this.rightHandScreenPos = manager.GetRightHandScreenPos();

            // update hand texture coordinates
            this.leftHandGUITexture.transform.position = this.leftHandScreenPos;
            this.rightHandGUITexture.transform.position = this.rightHandScreenPos;

            // check for left hand grip
            if (this.manager.GetLeftHandEvent() == InteractionWrapper.InteractionHandEventType.Grip)
            {
                this.isLeftHandGripped = true;
                this.isLeftHandReleased = false;
                this.leftHandGUITexture.texture = (Texture)Resources.Load("LeftHandGrip");
            }
            else if (this.manager.GetLeftHandEvent() == InteractionWrapper.InteractionHandEventType.Release)
            {
                this.isLeftHandGripped = false;
                this.isLeftHandReleased = true;
                this.leftHandGUITexture.texture = (Texture)Resources.Load("LeftHandRelease");
            }
            //check for right hand grip
            if (this.manager.GetRightHandEvent() == InteractionWrapper.InteractionHandEventType.Grip)
            {
                this.isRightHandGripped = true;
                this.isRightHandReleased = false;
                this.rightHandGUITexture.texture = (Texture)Resources.Load("RightHandGrip");
            }
            else if (this.manager.GetRightHandEvent() == InteractionWrapper.InteractionHandEventType.Release)
            {
                this.isRightHandGripped = false;
                this.isRightHandReleased = true;
                this.rightHandGUITexture.texture = (Texture)Resources.Load("RightHandRelease");
            }
        }
    }

    /// <summary>
    /// Calculates the hand & object collision.
    /// Executes handles if gripped.
    /// </summary>
    private void calculateCollisionEvent()
    {
        foreach(InteractionObject interactionObject in this.interactionObjects) {
            // extract hand texture rectangles.
            Rect leftHandRect = this.leftHandGUITexture.GetScreenRect();
            Rect rightHandRect = this.rightHandGUITexture.GetScreenRect();
            // check rect overlaps & hand grip.
            // execute handler if met the conditions.
            bool interacted = false;
            // MAKE SURE OBJECT IS VISIBLE ON SCREEN.
            if (interactionObject.gObject.renderer.isVisible)
            {
                // CHECK KINECT HANDS
                if (leftHandRect.Overlaps(this.objectToScreenRect(interactionObject.gObject)))
                {
                    interacted = true;
                    if (this.isLeftHandGripped)
                    {
                        interactionObject.clickHandler();
                    }
                    else if (interactionObject.mouseOverHandler != null)
                        interactionObject.mouseOverHandler();
                }
                if (rightHandRect.Overlaps(this.objectToScreenRect(interactionObject.gObject)))
                {
                    interacted = true;
                    if (this.isRightHandGripped)
                        interactionObject.clickHandler();
                    else if (interactionObject.mouseOverHandler != null)
                        interactionObject.mouseOverHandler();
                }

                // CHECK CURSOR.
                if (this.objectToScreenRect(interactionObject.gObject).Contains(Input.mousePosition))
                {
                    interacted = true;
                    if (Input.GetMouseButtonDown(0))
                        interactionObject.clickHandler();
                    else if (interactionObject.mouseOverHandler != null)
                        interactionObject.mouseOverHandler();

                }
            }
            if (!interacted && interactionObject.mouseOutHandler != null)
                interactionObject.mouseOutHandler();
        }
    }

    private void calculateCollisionEventRay() {

    }

    private void calculateDrag()
    {

    }

    /// <summary>
    /// Calculate 3d object to screen rect.
    /// </summary>
    /// <param name="go"></param>
    /// <returns></returns>
    public Rect objectToScreenRect(GameObject go)
    {
        Vector3 cen = go.collider.bounds.center;
        Vector3 ext = go.collider.bounds.extents;
        Vector2[] extentPoints = new Vector2[8]
        {
            this.core.camera.WorldToScreenPoint(new Vector3(cen.x-ext.x, cen.y-ext.y, cen.z-ext.z)),
            this.core.camera.WorldToScreenPoint(new Vector3(cen.x+ext.x, cen.y-ext.y, cen.z-ext.z)),
            this.core.camera.WorldToScreenPoint(new Vector3(cen.x-ext.x, cen.y-ext.y, cen.z+ext.z)),
            this.core.camera.WorldToScreenPoint(new Vector3(cen.x+ext.x, cen.y-ext.y, cen.z+ext.z)),
 
            this.core.camera.WorldToScreenPoint(new Vector3(cen.x-ext.x, cen.y+ext.y, cen.z-ext.z)),
            this.core.camera.WorldToScreenPoint(new Vector3(cen.x+ext.x, cen.y+ext.y, cen.z-ext.z)),
            this.core.camera.WorldToScreenPoint(new Vector3(cen.x-ext.x, cen.y+ext.y, cen.z+ext.z)),
            this.core.camera.WorldToScreenPoint(new Vector3(cen.x+ext.x, cen.y+ext.y, cen.z+ext.z))
        };

        Vector2 min = extentPoints[0];
        Vector2 max = extentPoints[0];

        foreach (Vector2 v in extentPoints)
        {
            min = Vector2.Min(min, v);
            max = Vector2.Max(max, v);
        }

        if (max.x - min.x > 1000 || max.y - min.y > 1000) new Rect(0, 0, 0, 0);
        return new Rect(min.x, min.y, max.x - min.x, max.y - min.y);
    }

    /// <summary>
    /// Add interaction object with hands collision.
    /// </summary>
    /// <param name="rect"></param> a recatangle needed to calculate the hand and object collision.
    /// <param name="handler">a function executed, on hand interaction with object.</param>
    public void addInteractionObject(GameObject gObject, Action handler, Action mouseOverHandler=null, Action mouseOutHandler=null)
    {
        this.interactionObjects.Add(new InteractionObject(gObject, handler, mouseOverHandler, mouseOutHandler));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="interactionObject"></param>
    public void removeInteractionObject(InteractionObject interactionObject)
    {
        this.interactionObjects.Remove(interactionObject);
    }

    /// <summary>
    /// 
    /// </summary>
    public void clearInteractionEvents()
    {
        this.interactionObjects.Clear();
    }
}

/// <summary>
/// Object to be interacted with hands.
/// </summary>
public class InteractionObject
{
    public GameObject gObject;
    public Action clickHandler;
    public Action mouseOverHandler;
    public Action mouseOutHandler;

    public InteractionObject(GameObject gObject, Action clickHandler, Action mouseOverHandler, Action mouseOutHandler)
    {
        this.gObject = gObject;
        this.clickHandler = clickHandler;
        this.mouseOverHandler = mouseOverHandler;
        this.mouseOutHandler = mouseOutHandler;
    }
}
