using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    protected override void InternalInit()
    {
    }

    protected override void InternalOnDestroy()
    {
    }

    [Header("Popup Error")]
    [SerializeField] private GameObject popupErrorPanel;


    public void ShowPopupError(string text)
    {
        popupErrorPanel.SetActive(true);
        popupErrorPanel.GetComponent<PopupErrorUI>().SetPopupErrorUI(text);
    }

}
