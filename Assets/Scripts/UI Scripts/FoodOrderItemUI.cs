using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FoodOrderItemUI : MonoBehaviour
{
    [SerializeField] private TMP_Text recipeName;
    [SerializeField] private TMP_Text numberOfOrders;
    [SerializeField] private TMP_Text price;

    public void SetFoodOrderItem(RecipeSo recipeSo)
    {
        recipeName.text = recipeSo.recipeName;
        price.text = recipeSo.price.ToString();
    }
}
