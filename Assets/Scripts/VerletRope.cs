using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class VerletRope : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField, Tooltip("The Number of points along the rope ignoring a start and end point. (ropeResolution+2)")]
    private int ropeResolution = 5;
    private int m_Resolution => ropeResolution + 2;

    [SerializeField, Tooltip("Distance between points of the rope.")]
    private float ropeSegmentLength = 0.25f;

    [SerializeField]
    private int m_SubStepCount = 50;

    private List<RopeSegment> ropeSegments = new List<RopeSegment>();

    [Header("Anchors")]
    [SerializeField]
    private Transform m_RopeAnchorStart;
    [SerializeField]
    private Transform m_RopeAnchorEnd;
                                
    [Header("Components")]
    [SerializeField]
    private LineRenderer m_LineRenderer;
    [SerializeField]
    private EdgeCollider2D m_EdgeCollider;
    
    private void Start()
    {
        if (!m_LineRenderer)
            m_LineRenderer = GetComponent<LineRenderer>();
        if (!m_EdgeCollider)
            m_EdgeCollider = GetComponent<EdgeCollider2D>();

        Setup();
    }

    private void LateUpdate()
    {
        DrawRope();
    }

    private void FixedUpdate()
    {
        Simulate();
    }

    public void Setup()
    {
        Vector3 ropeStartPoint = m_RopeAnchorStart.position;

        for (int i = 0; i < m_Resolution; i++)
        {
            this.ropeSegments.Add(new RopeSegment(ropeStartPoint));
            ropeStartPoint.y -= ropeSegmentLength;
        }
    }

    void Simulate()
    {
        Vector2 forceGravity = new Vector2(0f, -1f);

        for (int i = 0; i < m_Resolution; i++)
        {
            RopeSegment firstSegment = ropeSegments[i];
            Vector2 velocity = firstSegment.currentPos - firstSegment.previousPos;

            firstSegment.previousPos = firstSegment.currentPos;
            firstSegment.currentPos += velocity;
            firstSegment.currentPos += forceGravity * Time.deltaTime;

            ropeSegments[i] = firstSegment;
        }

        // CONSTRAINTS

        for (int i = 0; i < m_SubStepCount; i++)
        {
            ApplyConstraint();
        }
    }

    private void ApplyConstraint()
    {
        RopeSegment ropeStart = ropeSegments[0];
        ropeStart.currentPos = m_RopeAnchorStart.position;
        ropeSegments[0] = ropeStart;

        for (int i = 0; i < m_Resolution - 1; i++)
        {
            RopeSegment firstSegment = ropeSegments[i];
            RopeSegment secondSegment = ropeSegments[i + 1];

            float dist = (firstSegment.currentPos - secondSegment.currentPos).magnitude;
            float error = dist - ropeSegmentLength;

            Vector2 changeDir = (firstSegment.currentPos - secondSegment.currentPos).normalized;
            Vector2 changeAmount = changeDir * error;

            if(i != 0)
            {
                firstSegment.currentPos -= changeAmount * 0.5f;
                ropeSegments[i] = firstSegment;

                secondSegment.currentPos += changeAmount * 0.5f;
                ropeSegments[i + 1] = secondSegment;
            }
            else
            {
                secondSegment.currentPos += changeAmount;
                ropeSegments[i + 1] = secondSegment;
            }
        }

        RopeSegment ropeEnd = ropeSegments[ropeSegments.Count - 1];
        ropeEnd.currentPos = m_RopeAnchorEnd.position;
        ropeSegments[ropeSegments.Count - 1] = ropeEnd;
    }

    /*
    private void SplitRope()
    {
        VerletRope firstSeg = Instantiate(m_SubRopePrefab, transform.position, transform.rotation);
        VerletRope secondSeg = Instantiate(m_SubRopePrefab, transform.position, transform.rotation);

        firstSeg.Setup();
        secondSeg.Setup();

        Destroy(this.gameObject);
    }*/

    private void DrawRope()
    {
        if (ropeSegments.Count <= 1)
            return;

        Vector3[] ropePositions = new Vector3[m_Resolution];
        List<Vector2> edgeColliderPoints = new List<Vector2>();

        for (int i = 0; i < m_Resolution; i++)
        {
            ropePositions[i] = ropeSegments[i].currentPos;
            edgeColliderPoints.Add((Vector3) ropePositions[i]);
        }

        m_LineRenderer.positionCount = ropePositions.Length;
        m_LineRenderer.SetPositions(ropePositions);
        m_EdgeCollider.SetPoints(edgeColliderPoints);
    }

    public struct RopeSegment
    {
        public Vector2 currentPos;
        public Vector2 previousPos;

        public RopeSegment(Vector2 pos)
        {
            this.currentPos = pos;
            this.previousPos = pos;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("CUT!");
        //Vector2 closestPoint = m_EdgeCollider.ClosestPoint(collision.transform.position);
        //print(closestPoint);
    }
}
