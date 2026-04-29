using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class AVP_InputField : MonoBehaviour, ISelectHandler, IPointerClickHandler
{
    public TMP_InputField inputField;

    private TouchScreenKeyboard keyboard;
    private bool keyboardOpened = false;

    private void Awake()
    {
        if (inputField == null)
            inputField = GetComponent<TMP_InputField>();
    }

    public void OnSelect(BaseEventData eventData)
    {
        OpenKeyboard();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OpenKeyboard();
    }

    private void OpenKeyboard()
    {
        if (inputField == null)
        {
            Debug.LogError("[AVP_InputField] inputField is null.");
            return;
        }

        if (keyboardOpened)
            return;

        keyboard = TouchScreenKeyboard.Open(
            inputField.text,
            TouchScreenKeyboardType.Default,
            false,
            inputField.lineType == TMP_InputField.LineType.MultiLineNewline,
            inputField.contentType == TMP_InputField.ContentType.Password
        );

        keyboardOpened = true;
    }

    private void Update()
    {
        if (!keyboardOpened)
            return;

        if (keyboard == null)
        {
            keyboardOpened = false;
            return;
        }

        try
        {
            if (!keyboard.active)
            {
                keyboardOpened = false;
                keyboard = null;
                return;
            }

            inputField.text = keyboard.text;
            inputField.caretPosition = inputField.text.Length;

            if (keyboard.status == TouchScreenKeyboard.Status.Done ||
                keyboard.status == TouchScreenKeyboard.Status.Canceled ||
                keyboard.status == TouchScreenKeyboard.Status.LostFocus)
            {
                keyboardOpened = false;
                keyboard = null;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("[AVP_InputField] Keyboard became invalid: " + e.Message);
            keyboardOpened = false;
            keyboard = null;
        }
    }
}