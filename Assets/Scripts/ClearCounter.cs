using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter, IKitchenObjectParent
{
    [SerializeField] private KitchenObjectsSO kitchenObjectsSO;


    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // has no object on counter
            if (player.HasKitchenObject())
            {
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
        }
        else
        {
            // has object on counter
            if (player.HasKitchenObject())
            {
                if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    if (plateKitchenObject.TryAddIngredients(GetKitchenObject().GetKitchenObjectsSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                }
                else
                {
                    // player not holding plate but something else
                    if(GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject1))
                    {
                        if (plateKitchenObject1.TryAddIngredients(player.GetKitchenObject().GetKitchenObjectsSO()))
                        { 
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    
}
