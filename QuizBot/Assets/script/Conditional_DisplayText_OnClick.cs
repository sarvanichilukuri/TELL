//This script displays a parameter element when the object is clicked based on the game state.
//Used to display input validation text.
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Conditional_DisplayText_OnClick : MonoBehaviour
{
    public Button clickedButton; //Button clicked
    public TextMeshProUGUI displayText; //Text that might be displayed
    public Validation_Evaluator checker; //Used to check game state

    void Start()
    {
        //Create listener for the button in question
        clickedButton.onClick.AddListener(TaskOnClick);
        
    }

    void TaskOnClick()
    {
        //Make text opaque based on condition
        if(!checker.EvaluatorValidation()) //if input is invalid
            displayText.alpha = 255;
    }
}