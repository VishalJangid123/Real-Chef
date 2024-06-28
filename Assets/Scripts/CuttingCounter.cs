using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter, IKitchenObjectParent, IHasProgress
{
    [SerializeField] private CuttingRecipeSO[] cuttingKitchenObjectSO;
    private int currentCutProgress;

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;



    public event EventHandler OnCut;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // has no object on counter
            if (player.HasKitchenObject())
            {
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectsSO()))
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    currentCutProgress = 0;
                    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipefromInput(GetKitchenObject().GetKitchenObjectsSO());

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressedNormalized = (float)currentCutProgress / cuttingRecipeSO.cuttingProgressMax
                    }) ;

                }
            }
        }
        else
        {
            // has object on counter
            if (player.HasKitchenObject())
            {
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    if (plateKitchenObject.TryAddIngredients(GetKitchenObject().GetKitchenObjectsSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                }
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectsSO()))
        {
            currentCutProgress++;
            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipefromInput(GetKitchenObject().GetKitchenObjectsSO());

            OnCut?.Invoke(this, EventArgs.Empty);

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressedNormalized = (float)currentCutProgress / cuttingRecipeSO.cuttingProgressMax
            });

            if (currentCutProgress >= cuttingRecipeSO.cuttingProgressMax)
            {
                KitchenObjectsSO output = GetOutputFromInputKitchenObjectSO(GetKitchenObject().GetKitchenObjectsSO());
                // has object now cut it
                GetKitchenObject().DestroySelf();
                KitchenObject.SpawnKitchenObject(output, this);
            }

        }
    }

    private bool HasRecipeWithInput(KitchenObjectsSO kitchenObjectsSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingKitchenObjectSO)
        {
            if (cuttingRecipeSO.input == kitchenObjectsSO)
            {
                return true;
            }
        }
        return false;
    }


    private KitchenObjectsSO GetOutputFromInputKitchenObjectSO(KitchenObjectsSO kitchenObjectsSO)
    {
        foreach(CuttingRecipeSO cuttingRecipeSO in cuttingKitchenObjectSO)
        {
            if(cuttingRecipeSO.input == kitchenObjectsSO)
            {
                return cuttingRecipeSO.output;
            }
        }
        return null;
    }

    private CuttingRecipeSO GetCuttingRecipefromInput(KitchenObjectsSO kitchenObjectsSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingKitchenObjectSO)
        {
            if (cuttingRecipeSO.input == kitchenObjectsSO)
            {
                return cuttingRecipeSO;
            }
        }
        return null;
    }
}
