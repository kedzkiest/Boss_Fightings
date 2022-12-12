using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class bodyController : MonoBehaviour
{
    [Space(10)]
    [Header("GameObject Assignment")]
    [Space(10)]

    [SerializeField] private GameObject[] legTargets;
    [SerializeField] private GameObject[] legCubes;
    [SerializeField] GameObject spider;


    [Space(10)]
    [Header("Values for leg Movement")]
    [Space(10)]

    // maximum distance it can go without moving the leg
    [SerializeField] private float moveDistance = 2.5f;
    // determine how many frames it takes for the leg to move
    [SerializeField] private int legMovementSmoothness = 5;
    // determine how many frames it takes for the body to rotate
    [SerializeField] private int bodySmoothness = 5;
    [SerializeField] private int velocitySmoothness = 3;
    [SerializeField] private float overStepMultiplier = 1.3f;
    // make this code waits a couple of frames while moving the other leg
    [SerializeField] private int waitTimeBetweenSteps = 2;
    [SerializeField] float spiderJitterCutOff = 0.1f;
    [SerializeField] float stepHeight = 0.5f;


    [Space(10)]
    [Header("GameObject Assignment")]
    [Space(10)]

    [SerializeField] private bool enableBodyRotation = false;
    [SerializeField] private bool enableMovementRotation = false;
    [SerializeField] private bool rigidBodyController;



    private bool currentLeg = true;

    private Vector3[] legPositions;
    private Vector3[] legOriginalPositions;

    private Vector3 velocity;
    private Vector3 lastSpiderPosition;
    private Vector3 lastVelocity;

    private Vector3 lastBodyUp;

    private List<int> oppsiteLegIndex = new List<int>();
    private List<int> nextIndexToMove = new List<int>();
    private List<int> IndexMoving = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        lastBodyUp = transform.up;

        legPositions = new Vector3[legTargets.Length];
        legOriginalPositions = new Vector3[legTargets.Length];

        for (int i = 0; i < legTargets.Length; i++)
        {
            Vector3 position = legTargets[i].transform.position;
            legPositions[i] = position;
            legOriginalPositions[i] = position;

            if (currentLeg) { oppsiteLegIndex.Add(i + 1); currentLeg = false; }
            else if(!currentLeg) { oppsiteLegIndex.Add(i - 1); currentLeg = true; }
        }

        lastSpiderPosition = spider.transform.position;

        rotateBody();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        velocity = spider.transform.position - lastSpiderPosition;
        velocity = velocity + velocitySmoothness * lastVelocity;
        velocity = velocity / (velocitySmoothness + 1);

        moveLegs();
        rotateBody();

        lastSpiderPosition = spider.transform.position;
        lastVelocity = velocity;
    }

    void moveLegs()
    {
        if (!enableMovementRotation) return;

        for(int i = 0; i < legTargets.Length; i++)
        {
            if (Vector3.Distance(legTargets[i].transform.position, legCubes[i].transform.position) >= moveDistance)
            {
                if (!nextIndexToMove.Contains(i) && !IndexMoving.Contains(i)) nextIndexToMove.Add(i);
            }
            else if (!IndexMoving.Contains(i))
            {
                legTargets[i].transform.position = legOriginalPositions[i];
            }
        }

        if (nextIndexToMove.Count == 0 || IndexMoving.Count != 0) return;

        Vector3 targetPosition = legCubes[nextIndexToMove[0]].transform.position;
        targetPosition += Mathf.Clamp(velocity.magnitude * overStepMultiplier, 0, 2) * 
            (legCubes[nextIndexToMove[0]].transform.position - legTargets[nextIndexToMove[0]].transform.position) + 
            velocity * overStepMultiplier;

        StartCoroutine(step(nextIndexToMove[0], targetPosition, false));
    }

    IEnumerator step(int index, Vector3 moveTo, bool isOpposite)
    {
        if (!isOpposite) moveOppositeLeg(oppsiteLegIndex[index]);

        if (nextIndexToMove.Contains(index)) nextIndexToMove.Remove(index);
        if(!IndexMoving.Contains(index)) IndexMoving.Add(index);

        Vector3 startingPosition = legOriginalPositions[index];

        for(int i = 0; i <= legMovementSmoothness; i++)
        {
            legTargets[index].transform.position = Vector3.Lerp(startingPosition, 
                moveTo + new Vector3(0, Mathf.Sign(i / (legMovementSmoothness + spiderJitterCutOff) * Mathf.PI) * stepHeight, 0), 
                i / legMovementSmoothness);

            yield return new WaitForFixedUpdate();
        }

        legOriginalPositions[index] = moveTo;

        for(int i = 1; i <= waitTimeBetweenSteps; i++)
        {
            yield return new WaitForFixedUpdate();
        }

        if (IndexMoving.Contains(index)) IndexMoving.Remove(index);
    }

    void moveOppositeLeg(int index)
    {
        Vector3 targetPosition = legCubes[index].transform.position;
        targetPosition += Mathf.Clamp(velocity.magnitude * overStepMultiplier, 0, 2) *
            (legCubes[index].transform.position - legTargets[index].transform.position) +
            velocity * overStepMultiplier;

        StartCoroutine(step(index,  targetPosition, true));
    }

    void rotateBody()
    {
        if (!enableBodyRotation) return;

        Vector3 v1 = legTargets[0].transform.position - legTargets[1].transform.position;
        Vector3 v2 = legTargets[2].transform.position - legTargets[3].transform.position;
        Vector3 normal = Vector3.Cross(v1, v2).normalized;
        Vector3 up = Vector3.Lerp(lastBodyUp, normal, 1f / bodySmoothness);
        transform.up = up;
        if (!rigidBodyController) transform.rotation = Quaternion.LookRotation(transform.parent.forward, up);
        lastBodyUp = up;
    }
}
