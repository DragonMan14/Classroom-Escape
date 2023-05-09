using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InputTester : Interactable, IInteractable
{
    InputManager inputManager;
    bool inputMatches;

    Animator door;
    
    // Start is called before the first frame update
    void Start()
    {
        inputManager = InputManager.Instance;
        door = this.transform.parent.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("Started");
            StartCoroutine(InputDetection("04/01/2023"));
        }
    }

    public void Interact()
    {
        StartCoroutine(InteractCoroutine("04/01/2023"));
    }

    public IEnumerator InteractCoroutine(string desiredInput)
    {
        yield return InputDetection(desiredInput);
        if (inputMatches)
        {
            door.SetBool("isOpen", true);
        }
    }

    private IEnumerator InputDetection(string desiredInput)
    {
        inputManager.DisplayInput();
        while (inputManager.inputIsActive)
        {
            yield return inputManager.WaitForInput();
            if (inputManager.confirmationClicked)
            {
                inputManager.ResetConfirmationClick();
                inputMatches = inputManager.CheckInput(desiredInput);
            } 
            else
            {
                yield break;
            }

            if (inputMatches)
            {
                inputManager.HideInput();
                break;
            }
        }
    }
}
