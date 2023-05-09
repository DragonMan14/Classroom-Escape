using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    GameObject inputPanelObject;
    TMP_InputField inputField;
    TextMeshProUGUI placeholder;
    public bool inputIsActive => inputPanelObject.activeInHierarchy;
    public bool confirmationClicked { get; private set; }

    public TMP_InputValidator date;

    public string Input { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        inputPanelObject = GameObject.FindGameObjectWithTag("InputPanel");
        GameObject inputFieldObject = GameObject.FindGameObjectWithTag("InputField");
        inputField = inputFieldObject.GetComponent<TMP_InputField>();
        placeholder = inputFieldObject.GetComponentsInChildren<TextMeshProUGUI>()[0];
        
        inputPanelObject.SetActive(false);
    }

    private void Update()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.J))
        {
            DisplayInput();
        }
        if (UnityEngine.Input.GetKeyDown(KeyCode.H))
        {
            SetPlaceholder("Hello!");
        }
        if (UnityEngine.Input.GetKeyDown(KeyCode.K))
        {
            SetValidation("Hi");
        }
        if (UnityEngine.Input.GetKeyDown(KeyCode.L))
        {
            SetValidation("date");
        }
    }

    public void DisplayInput()
    {
        inputPanelObject.SetActive(true);
    }

    public void HideInput()
    {
        inputPanelObject.SetActive(false);
        inputField.text = string.Empty;
    }

    public void SetValidation(string validator)
    {
        switch(validator)
        {
            case "date":
                inputField.inputValidator = date; 
                break;
            default:
                Debug.LogError("Invalid input validator");
                break;
        }
    }

    public bool CheckInput(string desiredInput)
    {
        return Input.Equals("04/01/2023");
    }

    public void SetPlaceholder(string text)
    {
        placeholder.text = text;
    }

    // ReadStringInput is not to be called
    // This is only used as a listener to save the input from the input field to the Input variable
    public void ReadStringInput(string input)
    {
        this.Input = input;
        print($"The input is {this.Input}");
    }

    public void ReadStringInput(TMP_InputField input)
    {
        this.Input = input.text;
        print($"The input is {this.Input}");
    }

    // Not to be called
    // Only used as a listener 
    public void OnConfirmationClick()
    {
        confirmationClicked = true;
    }

    public void ResetConfirmationClick()
    {
        confirmationClicked = false;
    }

    public IEnumerator WaitForInput()
    {
        while (inputIsActive && !confirmationClicked)
        {
            yield return null;
        }
    }
}
