using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;
    [SerializeField] private KitchenObjectsSO kitchenObjectsSO;

    float spawnPlateTimer;
    float spawnPlateTimerMax = 4f;
    int spawnPlateAmount;
    int spawnPlateMax = 4;

    private void Update()
    {
        spawnPlateTimer += Time.deltaTime;
        if(spawnPlateTimer > spawnPlateTimerMax)
        {
            spawnPlateTimer = 0f;
            if(spawnPlateAmount < spawnPlateMax)
            {
                spawnPlateAmount++;
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            if(spawnPlateAmount > 0)
            {
                spawnPlateAmount--;
                KitchenObject.SpawnKitchenObject(kitchenObjectsSO, player);
                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }

        }
    }
}
