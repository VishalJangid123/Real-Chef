using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlateCompleteVisual : MonoBehaviour
{
    [SerializeField] private PlateKitchenObject plateKitchenObject;

    [Serializable]
    public struct KitchenObjectSO_GameObject
    {
        public KitchenObjectsSO kitchenObjectSO;
        public GameObject gameObject;
    }

    [SerializeField] List<KitchenObjectSO_GameObject> kitchenObjectSO_GameObjects;


    private void Start()
    {
        plateKitchenObject.OnIngredientsAdded += PlateKitchenObject_OnIngredientsAdded1;

        foreach (KitchenObjectSO_GameObject k_go in kitchenObjectSO_GameObjects)
        {
            k_go.gameObject.SetActive(false );
        }
    }

    private void PlateKitchenObject_OnIngredientsAdded1(object sender, PlateKitchenObject.OnIngredientsAddedEventArgs e)
    {
        foreach (KitchenObjectSO_GameObject k_go in kitchenObjectSO_GameObjects)
        {
            if (e.kitchenObjectsSO == k_go.kitchenObjectSO)
            {
                k_go.gameObject.SetActive(true);
            }
        }
    }

    

}
