using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TableButtonUI : MonoBehaviour
{
    [SerializeField] private Button tableButton;
    [SerializeField] private TMP_Text tableNumber;
    [SerializeField] private TMP_Text tableStatus;



    public void SetTableButtonUI(RestaurantTable table, Customer customer = null)
    {
        tableNumber.text = table.tableNumber.ToString();
        if (table.isAvailable == true)
        {
            // table is empty
            tableButton.image.sprite = GameAssets.instance.buttonGreen;
            tableStatus.text = "Available";
            tableButton.onClick.RemoveAllListeners();

            if (customer != null)
            {
                tableButton.onClick.AddListener(delegate () { print("Table is available clicked");
                if (table.CanSeatParty(customer.partySize))
                    {
                        table.AssignCustomer(customer);
                    }
                    else
                    {
                        // show error that table have less party size
                        UIManager.instance.ShowPopupError("The table you selected have less party size than customer requeseted.");
                    }


                });
            }
        }
        else
        {
            if(table.seatedCustomer != null)
            {
                // customer on the table
                tableButton.image.sprite = GameAssets.instance.buttonRed;
                tableStatus.text = "Occupied";


            }
            else
            {
                // customer left - table needs to be clean
                tableButton.image.sprite = GameAssets.instance.buttonGray;
                tableStatus.text = "Need to be clean";


            }
        }
    }
    
}
