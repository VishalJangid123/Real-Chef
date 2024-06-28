using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectsSO kitchenObjectsSO;


    private IKitchenObjectParent kitchenObjectParent;

    public KitchenObjectsSO GetKitchenObjectsSO()
    {
        return kitchenObjectsSO;
    }

    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return kitchenObjectParent;
    }

    public void SetKitchenObjectParent(IKitchenObjectParent kop)
    {
        if(this.kitchenObjectParent != null)
        {
            kitchenObjectParent.ClearKitchenObject(); // current parant is clear
        }

        kitchenObjectParent = kop;
        if (kop.HasKitchenObject())
        {
            Debug.LogError("Counter already have a kitchen object");
        }

        kop.SetKitchenObject(this);




        transform.parent = kop.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }


    public void DestroySelf()
    {
        kitchenObjectParent.ClearKitchenObject();
        Destroy(gameObject);
    }

    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
    {
        if(this is PlateKitchenObject)
        {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }
        else
        {
            plateKitchenObject = null;
            return false;
        }
    }

    public static KitchenObject SpawnKitchenObject(KitchenObjectsSO kitchenObjectsSO, IKitchenObjectParent kitchenObjectParent)
    {
        Transform kitchenobjectTransform = Instantiate(kitchenObjectsSO.prefab);
        KitchenObject ko = kitchenobjectTransform.GetComponent<KitchenObject>();
        ko.SetKitchenObjectParent(kitchenObjectParent);
        return ko;
    }
}
