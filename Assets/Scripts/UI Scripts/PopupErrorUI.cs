using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PopupErrorUI : MonoBehaviour
{
    [SerializeField] private TMP_Text infoText;
    [SerializeField] private Button closeButton;

    public void SetPopupErrorUI(string info)
    {
        infoText.text = info;
        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(delegate { this.gameObject.SetActive(false);  });
    }


}
