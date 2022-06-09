// A longer example of Vector3.Lerp usage.
// Drop this script under an object in your scene, and specify 2 other objects in the "startMarker"/"endMarker" variables in the script inspector window.
// At play time, the script will move the object along a path between the position of those two markers.

using UnityEngine;
using System.Collections;
using HurricaneVR.Framework.Core;

public class ExampleScript : MonoBehaviour
{
    // Transforms to act as start and end markers for the journey.
    public Transform startMarker;
    public Transform endMarker;

    // Movement speed in units per second.
    public float speed = 1.0F;

    // Time when the movement started.
    private float startTime;

    // Total distance between the markers.
    private float journeyLength;

    void Start()
    {
        // Keep a note of the time the movement started.
        startTime = Time.time;

        // Calculate the journey length.
        //  journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
    }

    // Move to the target end position.
    void Update()
    {
        // Distance moved equals elapsed time times speed..
        // float distCovered = (Time.time - startTime) * speed;

        // Fraction of journey completed equals current distance divided by total distance.
        // float fractionOfJourney = distCovered / journeyLength;

        // Set our position as a fraction of the distance between the markers.
        //transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fractionOfJourney);
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("on trigger enter" + other.gameObject.transform.position);
        HVRGrabbable grabbable = other.gameObject.GetComponent<HVRGrabbable>();
        if (grabbable != null)
        {
            Debug.Log("is being held " + grabbable.IsBeingHeld);
            // it is not held a entering the zone, so snap to drop zone
            if (!grabbable.IsBeingHeld)
            {
                StartCoroutine(SnapToZone(other.gameObject));
            }
        }
        else
        {
            Debug.Log(other.gameObject.name);
        }
        // Transform transform = other.gameObject.transform;
        //  transform.position = new Vector3(0, 0, 0);
        //  
    }

    IEnumerator SnapToZone(GameObject otherGameObject)
    {
        Rigidbody rb = otherGameObject.GetComponent<Rigidbody>();
        Destroy(rb);
        Vector3 startingPos = otherGameObject.transform.position;
        Quaternion startingRot = otherGameObject.transform.rotation;
        for (float f = 0.0f; f <= 1.0f; f += 0.01f)
        {
            otherGameObject.transform.position = Vector3.Lerp(startingPos, endMarker.position, f);
            otherGameObject.transform.rotation = Quaternion.Lerp(startingRot, endMarker.rotation, f);

            Debug.Log("other object moving" + otherGameObject.transform.position);
            // yield return new WaitForSeconds(.01f);
            yield return null;
        }
    }
}