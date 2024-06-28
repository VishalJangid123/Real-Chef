using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TableState
{
    Occupied,
    Available,
    NeedToBeClean
}

public class RestaurantTable : BaseCounter
{
    public int tableNumber;
    public int seatingCapacity;
    public bool isAvailable = true;
    public Customer seatedCustomer; // current customer on the table

    [SerializeField] private List<Transform> chairs;
    [SerializeField] private List<Transform> plates;

    public void AssignCustomer(Customer customer)
    {
        isAvailable = false;
        seatedCustomer = customer;

        // assign chair to all the customer party
        //customer.transform.position = chairs[0].position;

        customer.MoveCustomerToChair(chairs[0].position);

        //for(int party = 0; party <= customer.partySize-1; party++)
        //{
        //    GameObject partyGO = Instantiate(customer.customerPartyObject, customer.transform);
        //    partyGO.transform.position = chairs[party + 1].transform.position;
        //}
        customer.transform.rotation = transform.rotation;
    }

    public void RemoveCustomer()
    {
        isAvailable = true;
        seatedCustomer.isSeated = false;
        seatedCustomer.assignedTable = null;
        seatedCustomer = null;
    }

    public int SeatingCapacity
    {
        get { return seatingCapacity; }
    }

    public bool IsAvailable
    {
        get { return isAvailable; }
    }

    public bool CanSeatParty(int partySize)
    {
        return isAvailable && seatingCapacity >= partySize;
    }

    public override void Interact(Player player)
    {
        if (isAvailable)
        {
            //table is empty
        }
        else
        {
            // customer is sitting on the table
            if(seatedCustomer.state == Customer.State.IsSeated)
            {
                // ask for order
                KitchenManager.instance.ShowFoodOrderedMenu(this);
            }
            else if(seatedCustomer.state == Customer.State.FoodOrdered)
            {
                // customer ordered food already
            }
            else if(seatedCustomer.state == Customer.State.FoodDelivered)
            {
                // food was delieveed
            }

        }
    }
}
