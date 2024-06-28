using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCounterVisuals : MonoBehaviour
{
    [SerializeField] private PlateCounter plateCounter;
    [SerializeField] private Transform plateVisualPrefab;
    [SerializeField] private Transform counterTopPoint;
    List<GameObject> plateVisualGameObjects;

    private void Awake()
    {
        plateVisualGameObjects = new List<GameObject>();
    }

    private void Start()
    {
        plateCounter.OnPlateSpawned += PlateCounter_OnPlateSpawned;
        plateCounter.OnPlateRemoved += PlateCounter_OnPlateRemoved;
    }

    private void PlateCounter_OnPlateRemoved(object sender, System.EventArgs e)
    {
        GameObject plate = plateVisualGameObjects[plateVisualGameObjects.Count - 1];
        plateVisualGameObjects.Remove(plate);
        Destroy(plate);
    }

    private void PlateCounter_OnPlateSpawned(object sender, System.EventArgs e)
    {
        Transform plateVisualTransform =  Instantiate(plateVisualPrefab, counterTopPoint);

        float plateOffset = 0.1f;
        plateVisualTransform.localPosition = new Vector3(0, plateOffset * plateVisualGameObjects.Count, 0);
        plateVisualGameObjects.Add(plateVisualTransform.gameObject);
    }
}
