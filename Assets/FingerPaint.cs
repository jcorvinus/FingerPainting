using UnityEngine;
using System.Collections.Generic;
using Leap;

public class FingerPaint : MonoBehaviour
{
    public HandController LeapHandController;
    List<Vector3> linePoints;
    LineRenderer lineRenderer;
    FingerModel finger;

    private bool fingerdetect;
    private float newPointDelta = 0.02f;
    private Vector3 fingerTipPos;

    // Use this for initialization
    void Awake()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        lineRenderer.SetVertexCount(0);
        lineRenderer.SetWidth(0.1f, 0.1f);
        lineRenderer.SetColors(Color.green, Color.green);
        lineRenderer.useWorldSpace = true;
        fingerdetect = false;
        linePoints = new List<Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
        HandModel[] allGraphicHands = LeapHandController.GetAllGraphicsHands();

        if (allGraphicHands.Length <= 0) return;
        HandModel handModel = allGraphicHands[0];

        finger = handModel.fingers[(int)Finger.FingerType.TYPE_INDEX];

        fingerTipPos = finger.GetTipPosition();

        fingerdetect = (Input.GetKey(KeyCode.Space));

        if (fingerdetect)
        {
            // Using named temp variables like this helps me think more clearly about the code
            Vector3 previousPoint = (linePoints.Count > 0) ? linePoints[linePoints.Count - 1] : new Vector3(-1000, -1000, -1000); // If you've never seen this before, it's called a ternary expression.
                                                                                                        // It's just an if/else collapsed into a single line of code. 
                                                                                                        // Also, the crazy out of bounds initial value here ensures the starting point will always draw.

            if (Vector3.Distance(fingerTipPos, previousPoint) > newPointDelta)
            {
                linePoints.Add(fingerTipPos);
                lineRenderer.SetVertexCount(linePoints.Count);
                lineRenderer.SetPosition(linePoints.Count - 1, (Vector3)linePoints[linePoints.Count - 1]);
                Debug.Log(string.Format("Added point at: {0}!", fingerTipPos));
            }
        }
    }
}