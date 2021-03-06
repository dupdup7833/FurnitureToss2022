// A longer example of Vector3.Lerp usage.
// Drop this script under an object in your scene, and specify 2 other objects in the "startMarker"/"endMarker" variables in the script inspector window.
// At play time, the script will move the object along a path between the position of those two markers.

using UnityEngine;
using System.Collections;
using HurricaneVR.Framework.Core;
using HurricaneVR.Framework.Core.Grabbers;

public class FT_DropZone : MonoBehaviour
{
    // Transforms to act as start and end markers for the journey.
    public Transform dropZone;
    public GameObject guideGamePiece;
    private Mesh guideGamePieceMesh;
    // Movement speed in units per second.
    public float speed = 1.0F;

    // Time when the movement started.
    private float startTime;

    // Total distance between the markers.
    private float journeyLength;

    private bool objectPlaced = false;




    void Start()
    {
        // Keep a note of the time the movement started.
        startTime = Time.time;
        guideGamePiece.SetActive(false);
        guideGamePieceMesh = guideGamePiece.GetComponent<MeshFilter>().sharedMesh;
        /* foreach (Transform child in transform)
         {
             dropZone = child;
         }
         */




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

    private void releaseIt(HVRGrabberBase basestuff, HVRGrabbable grabble)
    {
        StartCoroutine(SnapToZone(grabble.gameObject));

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("on trigger enter" + other.gameObject.transform.position);
        if (!objectPlaced && other.tag == "FT_GamePiece" && guideGamePieceMesh == other.GetComponent<MeshFilter>().sharedMesh)
        {
            HVRGrabbable grabbable = other.gameObject.GetComponent<HVRGrabbable>();

            if (grabbable != null)
            {

                Debug.Log("is being held " + grabbable.IsBeingHeld);
                // it is not held a entering the zone, so snap to drop zone
                if (!grabbable.IsBeingHeld)
                {
                    Destroy(grabbable);
                    StartCoroutine(SnapToZone(other.gameObject));

                }
                else
                {
                    grabbable.Released.AddListener(releaseIt);
                    guideGamePiece.SetActive(true);
                }
            }
            else
            {

                Debug.Log(other.gameObject.name);
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (!objectPlaced && other.tag == "FT_GamePiece")
        {
            HVRGrabbable grabbable = other.gameObject.GetComponent<HVRGrabbable>();

            if (grabbable != null)
            {
                if (grabbable.IsBeingHeld)
                {
                    grabbable.Released.RemoveListener(releaseIt);
                }
            }
            guideGamePiece.SetActive(false);
        }
    }

    IEnumerator SnapToZone(GameObject otherGameObject)
    {
        Debug.Log("Snap to Zone");
        objectPlaced = true;
        Rigidbody rb = otherGameObject.GetComponent<Rigidbody>();
        Destroy(rb);
        HVRGrabbable grabbable = otherGameObject.GetComponent<HVRGrabbable>();
        Destroy(grabbable);

        Vector3 startingPos = otherGameObject.transform.position;
        Quaternion startingRot = otherGameObject.transform.rotation;
        Vector3 startingScale = otherGameObject.transform.localScale;
        for (float f = 0.0f; f <= 1.0f; f += 0.02f)
        {
            otherGameObject.transform.position = Vector3.Lerp(startingPos, dropZone.position, f);
            otherGameObject.transform.localScale = Vector3.Lerp(startingScale, guideGamePiece.transform.localScale, f);
            otherGameObject.transform.rotation = Quaternion.Lerp(startingRot, dropZone.rotation, f);

            Debug.Log("other object moving" + otherGameObject.transform.position);
            // yield return new WaitForSeconds(.01f);
            yield return null;
        }
        guideGamePiece.SetActive(false);
    }
}