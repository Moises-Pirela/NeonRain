using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TruckAI : MonoBehaviour
{
    public enum TruckState  
    {
        WAITING,
        MOVING,
        SELECTING
    }

    public TruckState state;
    
    public Transform[] positions;
    
    public Door gateToOpen;

    public GameObject platform;

    private int currentPositionIndex;
    private NavMeshAgent agent;
    private bool waiting;

    private float waitTimer;
    private float waitTime = 60f;

    private List<Transform> passengers = new List<Transform>();

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        state = TruckState.SELECTING;

        gateToOpen.onOpen += OnGateOpen;
    }

    private void Update()
    {
        switch (state)
        {
            case TruckState.WAITING:
                Waiting();
                break;
            case TruckState.MOVING:
                CheckDistance();
                break;
            case TruckState.SELECTING:
                SelectNewPosition();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        //MovePassengers(agent.velocity);
        
    }

    private void MovePassengers(Vector3 velocity)
    {
        if (passengers.Count == 0) return;
        
        float directionX = Mathf.Sign(velocity.x);
        float directionZ = Mathf.Sign(velocity.z);

        foreach (var passenger in passengers)
        {
            float pushX = directionZ == 1 ? velocity.x : 0;
            float pushY = velocity.z * directionZ;
            
            passenger.Translate(new Vector3(pushX, 0 , pushY));
        }
    }

    private void CheckDistance()
    {
        if (Vector3.Distance(transform.position, positions[currentPositionIndex].position) <= 10f)
        {   
            if (currentPositionIndex == 5)
                state = TruckState.WAITING;
            else 
            {
                if (currentPositionIndex != positions.Length - 1)
                    SelectNewPosition();
                else
                {
                    state = TruckState.WAITING;
                }
            }
        }
    }

    private void Waiting()
    {
        if (currentPositionIndex == 5)
        {
            TryOpengate();
        }
        else
        {
            waitTimer += Time.deltaTime;

            if (waitTimer >= waitTime)
            {
                currentPositionIndex = 0; 
                MoveTo(positions[currentPositionIndex].position);
                waitTimer = 0f;
            }
        }
    }

    private void TryOpengate()
    {
        gateToOpen.Interact();
    }

    private void OnGateOpen()
    {
        SelectNewPosition();
    }

    private void MoveTo(Vector3 position)
    {
        agent.SetDestination(position);
        state = TruckState.MOVING;
    }

    private void SelectNewPosition()
    {
        currentPositionIndex++;
        MoveTo(positions[currentPositionIndex].position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent = platform.transform;
            //other.GetComponent<CharacterController>().enabled = false;

            // if (!passengers.Contains(other.transform))
            //     passengers.Add(other.transform);
        }
    }

    // private void OnTriggerExit(Collider other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         other.transform.parent = null;
    //         passengers.Remove(other.transform);
    //     }
    // }
}
