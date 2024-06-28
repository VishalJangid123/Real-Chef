using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private BaseCounter basecounter;
    [SerializeField] private GameObject[] visualGameObject;

    private void Start()
    {
        Player.Instance.OnSelectedCounterChanged += Instance_OnSelectedCounterChanged1;
    }

    private void Instance_OnSelectedCounterChanged1(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if(e.selectedCounter == basecounter)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    void Show()
    {
        foreach(GameObject go in visualGameObject)
        {
            go.SetActive(true);
        }
    }

    void Hide()
    {
        foreach (GameObject go in visualGameObject)
        {
            go.SetActive(false);
        }
    }

}
