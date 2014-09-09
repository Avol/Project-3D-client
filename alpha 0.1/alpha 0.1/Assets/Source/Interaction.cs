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

    private const int selectedTimerLimit = 15;

    public Vector3 leftHandScreenPos { get; private set; }
    public Vector3 rightHandScreenPos { get; private set; }

    private GUITexture leftHandObject;
    private GUITexture rightHandObject;

    public GameObject selectedLeftHand;
    public GameObject selectedRightHand;
    public int selectedLeftTimer;
    public int selectedRightTimer;

    private IList<InteractionObject> interactionObjects;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="core"></param>
    public Interaction(Core core)
    {
        this.core = core;

        this.leftHandObject = GameObject.Find("LeftHand").GetComponent<GUITexture>();
        this.rightHandObject = GameObject.Find("RightHand").GetComponent<GUITexture>();

        this.manager = core.GetComponent<InteractionManager>();
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
            if (this.interactionObjects.Count > 0)
            {
                this.calculateSelectedEvent();
            }
            yield return new WaitForSeconds(0.05f);
        }
    }

    /// <summary>
    /// Extracts hand position, and grip data.
    /// </summary>
    private void extractHandData()
    {
        if (manager != null && manager.IsInteractionInited())
        {
            this.leftHandObject.transform.position = manager.GetLeftHandScreenPos();
            this.rightHandObject.transform.position = manager.GetRightHandScreenPos();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void calculateSelectedEvent()
    {
        if (this.interactionObjects != null)
            for (int i = 0; i < this.interactionObjects.Count; i++)
            {
                InteractionObject interactionObject = this.interactionObjects[i];
                if (interactionObject != null)
                {
                    // extract hand texture rectangles.
                    Rect leftHandRect = this.leftHandObject.GetScreenRect();
                    Rect rightHandRect = this.rightHandObject.GetScreenRect();

                    bool interacted = false;
                    if (leftHandRect.Overlaps(this.objectToScreenRect(interactionObject.gObject)))
                    {
                        interacted = true;
                        if (interactionObject.mouseOverHandler != null)
                            interactionObject.mouseOverHandler();
                        if (this.selectedLeftHand == interactionObject.gObject)
                            this.selectedLeftTimer++;
                        else this.selectedLeftTimer = 0;
                        if (this.selectedLeftTimer >= selectedTimerLimit)
                            interactionObject.mouseDownHandler("left");
                        this.selectedLeftHand = interactionObject.gObject;
                    }

                    if (rightHandRect.Overlaps(this.objectToScreenRect(interactionObject.gObject)))
                    {
                        interacted = true;
                        if (interactionObject.mouseOverHandler != null)
                            interactionObject.mouseOverHandler();
                        if (this.selectedRightHand == interactionObject.gObject)
                            this.selectedRightTimer++;
                        else this.selectedRightTimer = 0;
                        if (this.selectedRightTimer >= selectedTimerLimit)
                            interactionObject.mouseDownHandler("right");
                        this.selectedRightHand = interactionObject.gObject;
                    }
                    else this.selectedRightHand = null;

                    if (this.objectToScreenRect(interactionObject.gObject).Contains(Input.mousePosition))
                    {
                        interacted = true;
                        if (Input.GetMouseButton(0))
                            interactionObject.mouseDownHandler("cursor");
                        if (interactionObject.mouseOverHandler != null)
                            interactionObject.mouseOverHandler();
                    }

                    if (interacted == false)
                        if (interactionObject.mouseOutHandler != null)
                            interactionObject.mouseOutHandler();
                }
            }
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
    public InteractionObject addInteractionObject(GameObject gObject, Action<string> mouseDownHandler, Action<string> mouseUpHandler=null, Action mouseOverHandler=null, Action mouseOutHandler=null)
    {
        InteractionObject iObject = new InteractionObject(gObject, mouseDownHandler, mouseUpHandler, mouseOverHandler, mouseOutHandler);
        this.interactionObjects.Add(iObject);
        return iObject;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="interactionObject"></param>
    public void removeInteractionObject(GameObject gameObject)
    {
        foreach (InteractionObject iObject in this.interactionObjects)
        {
            if (iObject.gObject == gameObject)
            {
                this.interactionObjects.Remove(iObject);
                break;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void clearInteractionEvents()
    {
        this.interactionObjects = new List<InteractionObject>();
    }
}

/// <summary>
/// Object to be interacted with hands.
/// </summary>
public class InteractionObject
{
    public GameObject gObject;
    public Action<string> mouseDownHandler;
    public Action<string> mouseUpHandler;
    public Action mouseOverHandler;
    public Action mouseOutHandler;

    public InteractionObject(GameObject gObject, Action<string> mouseDownHandler, Action<string> mouseUpHandler, Action mouseOverHandler, Action mouseOutHandler)
    {
        this.gObject = gObject;
        this.mouseDownHandler = mouseDownHandler;
        this.mouseOverHandler = mouseOverHandler;
        this.mouseOutHandler = mouseOutHandler;
        this.mouseUpHandler = mouseUpHandler;
    }
}
