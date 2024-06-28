using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngredientsAddedEventArgs> OnIngredientsAdded;
    public class OnIngredientsAddedEventArgs : EventArgs
    {
        public KitchenObjectsSO kitchenObjectsSO;
    }
    public List<KitchenObjectsSO> kitchenObjectsSOs;
    [SerializeField] List<KitchenObjectsSO> validIngredientsSOs;

    private void Awake()
    {
        kitchenObjectsSOs = new List<KitchenObjectsSO>();
    }

    public bool TryAddIngredients(KitchenObjectsSO kitchenObjectsSO)
    {
        if (!validIngredientsSOs.Contains(kitchenObjectsSO))
        {
            return false;
        }

        if (kitchenObjectsSOs.Contains(kitchenObjectsSO))
        {
            return false;
        }
        else
        {
            kitchenObjectsSOs.Add(kitchenObjectsSO);
            OnIngredientsAdded?.Invoke(this, new OnIngredientsAddedEventArgs
            {
                kitchenObjectsSO = kitchenObjectsSO
            });

            return true;
        }
    }
}
