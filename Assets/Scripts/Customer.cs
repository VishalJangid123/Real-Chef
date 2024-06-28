using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Customer : BaseInteract
{
    public string nameOfCustomer;
    public int partySize;
    public bool isSeated;
    public RestaurantTable assignedTable;


    private NavMeshAgent agent;


    [SerializeField] private Transform chatBubbleTransform;
    [SerializeField] private CustomerAnimationHandler customerAnimationHandler;

    public enum State
    {
        WaitingInQueue,
        IsSeated,
        FoodOrdered,
        FoodDelivered,
        Billing,
        Leave
    }

    public State state = State.WaitingInQueue;

    public GameObject customerPartyObject;
    public List<RecipeSo> orderedRecipes;

    private void Awake()
    {
        orderedRecipes = new List<RecipeSo>();
        orderedRecipes.Add(GameAssets.instance.recipeSoList[0]);
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }


    public override void Interact(Player player)
    {
        print("Interact called ");
        //player.GetComponent<PlayerCustomerInteractionUI>().SetPlayerCustomerInteractionUI();
        
        StartCoroutine(Wait(() => RequestTable(player)));
    }

    public void RequestTable(Player player)
    {
        print("Request Table by " + nameOfCustomer);
        if (assignedTable == null)
        {
            player.Say("Checking table for you...");
            
            print("Checking");
            // open the UI and let user choose the table to assign to the customer
            // show Assign button only in this use case
            KitchenManager.instance.ShowTableMenu(true, this);

            return;

            RestaurantTable table = player.FindAvailableTable(partySize);
            if (table != null)
            {
                assignedTable = table;
                player.SeatCustomer(this, table);
                state = State.IsSeated;
                player.Say("Here is your table ");
            }
            else
            {
                player.DeclineRequest(this);
                player.Say("Sorry we dont have table for you");

            }
        }
    }

    public void LeaveRestaurant()
    {
        if (assignedTable != null)
        {
            assignedTable.RemoveCustomer();
            assignedTable = null;
        }
        Destroy(gameObject);
    }

    IEnumerator Wait(Action onComplete)
    {
        print("Starrt");
        yield return new WaitForSeconds(5);
        print("End");
        onComplete();
    }


    // MOVEMENT
    internal void MoveCustomerToChair(Vector3 position)
    {
        agent.SetDestination(position);

    }

}
