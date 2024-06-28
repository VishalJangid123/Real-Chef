using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatBubble : MonoBehaviour
{
    [SerializeField] SpriteRenderer background;
    [SerializeField] TextMeshPro textMeshPro;
    public float typingSpeed = 0.05f;

    public static void Create(Transform parent, Vector3 localposition, string text, bool destroy = true)
    {
        GameObject chat =  Instantiate(GameAssets.instance.chatBubblePF, parent);
        chat.transform.localPosition = localposition;
        chat.GetComponent<ChatBubble>().Setup(text);

        Destroy(chat, 4f);
    }
    public void Setup(string text)
    {
        textMeshPro.SetText(text);
        textMeshPro.ForceMeshUpdate();
        Vector2 textSize = textMeshPro.GetRenderedValues();
        Vector2 padding = new Vector2(0.5f, 0.5f);

        background.size = textSize + padding;
        WriteText(text);
    }

    private Coroutine typingCoroutine;

    public void WriteText(string text)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeText(text));
    }

    private IEnumerator TypeText(string text)
    {
        textMeshPro.text = "";
        foreach (char letter in text.ToCharArray())
        {
            textMeshPro.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
