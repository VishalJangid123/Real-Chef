using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateIconUI : MonoBehaviour
{
    [SerializeField] PlateKitchenObject plateKitchenObject;
    [SerializeField] Transform iconTemplate;

    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        plateKitchenObject.OnIngredientsAdded += PlateKitchenObject_OnIngredientsAdded;
    }

    private void PlateKitchenObject_OnIngredientsAdded(object sender, PlateKitchenObject.OnIngredientsAddedEventArgs e)
    {
        UpdateVisual(); 
    }

    private void UpdateVisual()
    {
        foreach(Transform child in transform)
        {
            if (child == iconTemplate) continue;
            Destroy(child.gameObject);
        }
        foreach(KitchenObjectsSO ko in plateKitchenObject.kitchenObjectsSOs)
        {
            Transform iconTemplateTransform = Instantiate(iconTemplate, transform);
            iconTemplateTransform.gameObject.SetActive(true);
            iconTemplateTransform.GetComponent<PlateIconSingleUI>().SetKitchObjectSO(ko);

        }
    }
}
