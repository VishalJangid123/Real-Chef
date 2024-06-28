using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ConversationManager : MonoBehaviour
{
    public static string[] conversationOptions = { "Hello, how can I help you?", "What would you like to order?" };
    public GameObject conversationPanel;
    public Text conversationText;
    public Button[] optionButtons;

    private int selectedOptionIndex;

    public void StartConversation(Player player, Customer customer)
    {
        // show the conversation panel
        conversationPanel.SetActive(true);

        // display conversation options to the player
        conversationText.text = customer.name + ": " + conversationOptions[0];
        for (int i = 0; i < optionButtons.Length; i++)
        {
            optionButtons[i].gameObject.SetActive(i < conversationOptions.Length - 1);
            if (i < conversationOptions.Length - 1)
            {
                optionButtons[i].GetComponentInChildren<Text>().text = conversationOptions[i + 1];
            }
        }

        // wait for the player to select an option
        StartCoroutine(WaitForOptionSelection());
    }

    private IEnumerator WaitForOptionSelection()
    {
        // disable option buttons until an option is selected
        foreach (Button button in optionButtons)
        {
            button.interactable = false;
        }

        // wait for an option to be selected
        while (selectedOptionIndex == -1)
        {
            yield return null;
        }

        // return the index of the selected option
        int optionIndex = selectedOptionIndex;
        selectedOptionIndex = -1;
        conversationPanel.SetActive(false);
        yield return optionIndex;
    }

    public void SelectOption(int optionIndex)
    {
        selectedOptionIndex = optionIndex;
    }
}
