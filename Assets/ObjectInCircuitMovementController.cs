using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInCircuitMovementController : MonoBehaviour
{

    public bool move;
    private Vector2[] targets;
    private float moveSpeed;
    private float timeToWait;
    private bool showedTargets;
    private int placeInArray;

    // Update is called once per frame
    void Update()
    {
        if (move)
            if (move)
            {
                transform.position = Vector2.MoveTowards(transform.position, targets[placeInArray], moveSpeed * Time.deltaTime);
            }
    }

    public void InitializeCyclicMovements(Vector2[] incomingTargets, float _moveSpeed, float _timeToWait, int arrayNumber)
    {
        string name = gameObject.name;
        moveSpeed = _moveSpeed;
        timeToWait = _timeToWait;
        placeInArray = arrayNumber;
        targets = incomingTargets;
        move = true;
        StartCoroutine(StartMoving());
    }

    protected virtual IEnumerator StartMoving()
    {
        while (true)
        {
            if (transform.position.x == targets[placeInArray].x && transform.position.y == targets[placeInArray].y)
            {
                placeInArray++;
                if (placeInArray == targets.Length)
                {
                    placeInArray = 0;
                }
            }
            yield return new WaitForSeconds(timeToWait);
        }

    }

    protected void OnCollisionEnter2D(Collision2D other)
    {
        if (GameObjectIsPlayer(other.gameObject))
        {
            other.transform.parent = transform;
        }
    }

    protected void OnCollisionExit2D(Collision2D other)
    {
        if (GameObjectIsPlayer(other.gameObject))
        {
            other.transform.parent = null;
        }
    }

    protected bool GameObjectIsPlayer(GameObject other)
    {
        return other.GetComponent<PlayerController>();
    }

}

