using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter, IKitchenObjectParent, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }

    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned
    }

    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    FryingRecipeSO fryingRecipeSO;
    BurningRecipeSO burningRecipeSO;

    private float fryingTimer;
    private float burningTimer;
    private State state;

    private void Start()
    {
        state = State.Idle;
    }

    private void Update()
    {
        if (HasKitchenObject())
        {
            switch (state)
            {
                case State.Idle:
                    break;
                case State.Frying:
                    fryingTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    { progressedNormalized = fryingTimer / fryingRecipeSO.fryingTimeMax });

                    if (fryingTimer > fryingRecipeSO.fryingTimeMax)
                    {
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);

                        state = State.Fried;

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });

                        burningTimer = 0f;
                        burningRecipeSO = GetBurningRecipefromInput(GetKitchenObject().GetKitchenObjectsSO());
                    }

                    break;
                case State.Fried:
                    burningTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    { progressedNormalized = burningTimer / burningRecipeSO.burningTimeMax });

                    if (burningTimer > burningRecipeSO.burningTimeMax)
                    {
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);
                        state = State.Burned;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        { progressedNormalized = 0f });

                    }

                    break;
                case State.Burned:
                    break;
            }



            
        }
    }


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
                    fryingRecipeSO = GetFryingRecipefromInput(GetKitchenObject().GetKitchenObjectsSO());
                    state = State.Frying;
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });

                    fryingTimer = 0f;

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
                        state = State.Idle;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        { progressedNormalized = 0f });
                    }
                }
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);
                state = State.Idle;
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                { progressedNormalized = 0f }); 
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectsSO kitchenObjectsSO)
    {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            if (fryingRecipeSO.input == kitchenObjectsSO)
            {
                return true;
            }
        }
        return false;
    }


    private KitchenObjectsSO GetOutputFromInputKitchenObjectSO(KitchenObjectsSO kitchenObjectsSO)
    {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            if (fryingRecipeSO.input == kitchenObjectsSO)
            {
                return fryingRecipeSO.output;
            }
        }
        return null;
    }

    private FryingRecipeSO GetFryingRecipefromInput(KitchenObjectsSO kitchenObjectsSO)
    {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            if (fryingRecipeSO.input == kitchenObjectsSO)
            {
                return fryingRecipeSO;
            }
        }
        return null;
    }

    private BurningRecipeSO GetBurningRecipefromInput(KitchenObjectsSO kitchenObjectsSO)
    {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
        {
            if (burningRecipeSO.input == kitchenObjectsSO)
            {
                return burningRecipeSO;
            }
        }
        return null;
    }
}
