using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class KitchenManager : Singleton<KitchenManager>
{
    protected override void InternalInit()
    {
    }

    protected override void InternalOnDestroy()
    {
    }

    public event EventHandler<OnOrderAcceptedByPlayerEventArgs> OnOrderAcceptedByPlayer;
    public class OnOrderAcceptedByPlayerEventArgs : EventArgs
    {
        public RestaurantTable table;
    }

    public List<RecipeSo> currentListOfOrderInKitchen;


    [SerializeField] private GameObject tableMenuPanel;
    [SerializeField] private Transform tableMenuScrollPanel;
    [SerializeField] private GameObject tableButton;

    [Header("Food Order")]
    [SerializeField] private GameObject foodOrderPanel;
    [SerializeField] private Transform foodOrderScrollPanel;
    [SerializeField] private Button acceptFoodOrderButton;
    [SerializeField] private Button rejectFoodOrderButton;
    [SerializeField] private GameObject foodOrderItemPF;

    private bool showTableMenu = false;

    private void Start()
    {
        tableMenuPanel.SetActive(false);
        foodOrderPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowTableMenu(showTableMenu);
            showTableMenu = !showTableMenu;

        }
    }

    public void ShowTableMenu(bool show, Customer customer = null)
    {
        DestroyChidrenInTransform(tableMenuScrollPanel.gameObject);

        tableMenuPanel.SetActive(show);

        if (show)
        {
            foreach(RestaurantTable table in GameAssets.instance.tables)
            {

                GameObject btn = Instantiate(tableButton, tableMenuScrollPanel);
                if(customer == null)
                    btn.GetComponent<TableButtonUI>().SetTableButtonUI(table);
                else
                    btn.GetComponent<TableButtonUI>().SetTableButtonUI(table, customer);

            }

        }
    }

    public void ShowFoodOrderedMenu(RestaurantTable table)
    {
        DestroyChidrenInTransform(foodOrderScrollPanel.gameObject);
        foodOrderPanel.SetActive(true);
        foreach(RecipeSo r in table.seatedCustomer.orderedRecipes)
        {
            GameObject item = Instantiate(foodOrderItemPF, foodOrderScrollPanel);
            item.GetComponent<FoodOrderItemUI>().SetFoodOrderItem(r);
        }

        acceptFoodOrderButton.onClick.RemoveAllListeners();
        rejectFoodOrderButton.onClick.RemoveAllListeners();

        acceptFoodOrderButton.onClick.AddListener(delegate() {
            OnOrderAcceptedByPlayer?.Invoke(this, new OnOrderAcceptedByPlayerEventArgs { table = table }); 
        }
        );
    }

    void DestroyChidrenInTransform(GameObject go)
    {
        foreach(Transform child in go.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
