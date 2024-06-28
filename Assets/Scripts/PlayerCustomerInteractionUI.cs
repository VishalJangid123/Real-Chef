using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCustomerInteractionUI : MonoBehaviour
{
    [SerializeField] private Text CustomerQuestionText;
    [SerializeField] private Button acceptBtn;
    [SerializeField] private Button denyBtn;

    

    private void Start()
    {
        acceptBtn.onClick.AddListener(delegate { });

    }

    public void SetPlayerCustomerInteractionUI()
    {
        
    }

}
