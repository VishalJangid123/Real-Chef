using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DeliveryManager : Singleton<DeliveryManager>
{
    protected override void InternalInit()
    {
    }

    protected override void InternalOnDestroy()
    {
    }

    [Serializable]
    public struct Table_OrderedList
    {
        public RestaurantTable table;
        public List<RecipeSo> orderedList;
    }

    List<Table_OrderedList> waitingOrderList;

    private void Awake()
    {
        waitingOrderList = new List<Table_OrderedList>();

        // Initialize all tables before only
        foreach(RestaurantTable table in GameAssets.instance.tables)
        {
            Table_OrderedList item = new Table_OrderedList { table = table, orderedList = new List<RecipeSo>() };
            waitingOrderList.Add(item);
        }

    }

    private void Start()
    {
        KitchenManager.instance.OnOrderAcceptedByPlayer += Instance_OnOrderAcceptedByPlayer;
    }

    private void Instance_OnOrderAcceptedByPlayer(object sender, KitchenManager.OnOrderAcceptedByPlayerEventArgs e)
    {
        AddNewOrderToWaitingRecipeList(e.table);
    }

    public void AddNewOrderToWaitingRecipeList(RestaurantTable table)
    {
        print("Recipe from table number " + table.tableNumber + " added to the kithen waiting list");
        foreach (Table_OrderedList order in waitingOrderList)
        {
            if(order.table == table)
            {
                // table already exist add menu directly
                foreach (RecipeSo recipe in table.seatedCustomer.orderedRecipes)
                {
                    order.orderedList.Add(recipe);
                }
            }
        }
        
    }




}
