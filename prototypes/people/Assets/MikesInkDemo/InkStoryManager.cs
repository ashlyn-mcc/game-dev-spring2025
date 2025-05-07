using UnityEngine;
using System.Collections;
using Ink.Runtime;
using UnityEngine.UI;
using TMPro;

public class InkStoryManager : MonoBehaviour
{
    [SerializeField] private TextAsset inkJSONAsset;
    [SerializeField] private GameObject choicePrefab;
    [SerializeField] private GameObject choicesUI;
    [SerializeField] private GameObject textBoxUI;
    [SerializeField] private TMP_Text textBox;

    // We can use this to know whether a knot is currently running.
    // I use it to control whether I can launch TalkToCharacter or not.
    public bool knotActive = false;

    private Story inkStory;

    // We are going to use this to make the coroutine wait until a choice button is clicked.
    private bool choiceMade = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        // Initialize the Ink story
        inkStory = new Story(inkJSONAsset.text);

        // Launch the intro knot right away
        StartCoroutine(LaunchKnot("IntroductoryScene"));
    }

    public void TalkToCharacter(string knotName)
    {
        // Reset the story to replay it
        inkStory = new Story(inkJSONAsset.text);

        // Pull variables from TaskManager and set them inside the Ink story
        TaskManager tasks = FindObjectOfType<TaskManager>();

        // Ensure that the variables from TaskManager are properly updated in Ink
        inkStory.variablesState["movement"] = tasks.movement;
        inkStory.variablesState["hitBullseyes"] = tasks.hitBullseyes;
        inkStory.variablesState["givenTrophy"] = tasks.givenTrophy;  

        // Start the knot specified
        StartCoroutine(LaunchKnot(knotName));
    }


    IEnumerator LaunchKnot(string knotName)
    {
        knotActive = true;

        // Set ink to use the knotName that was provided
        inkStory.ChoosePathString(knotName);

        // Show the UI panel
        textBoxUI.SetActive(true);

        // Main story loop — either narrative lines or choices
        while (inkStory.canContinue || inkStory.currentChoices.Count > 0)
        {
            // Display story text as long as there are no choices
            while (inkStory.canContinue)
            {
                string line = inkStory.Continue().Trim();
                textBox.text = line;

                // Wait for spacebar to advance
                while (!Input.GetKeyDown(KeyCode.Space))
                    yield return null;

                yield return null; 
            }

            while (inkStory.currentChoices.Count > 0)
            {
                choicesUI.SetActive(true);
                RemoveChoiceButtons(); 

                // Display current choices
                for (int i = 0; i < inkStory.currentChoices.Count; i++)
                {
                    GameObject buttonObj = Instantiate(choicePrefab, choicesUI.transform);
                    TMP_Text choiceText = buttonObj.GetComponentInChildren<TMP_Text>();
                    choiceText.text = $"{(char)('A' + i)}: {inkStory.currentChoices[i].text}";
                }

                // Wait for key press to choose
                bool validChoicePressed = false;
                while (!validChoicePressed)
                {
                    if (Input.GetKeyDown(KeyCode.A) && inkStory.currentChoices.Count > 0)
                    {
                        inkStory.ChooseChoiceIndex(0);
                        validChoicePressed = true;
                    }
                    else if (Input.GetKeyDown(KeyCode.B) && inkStory.currentChoices.Count > 1)
                    {
                        inkStory.ChooseChoiceIndex(1);
                        validChoicePressed = true;
                    }
                    yield return null;
                }

                RemoveChoiceButtons();
                choicesUI.SetActive(false);
            }

        }

        // Turn off the UI now that the knot is over
        textBoxUI.SetActive(false);
        knotActive = false;

        UpdateTaskManagerVariables(); 

    }

    // Destroys all current choice buttons before the next set appears
    private void RemoveChoiceButtons()
    {
        foreach (Transform child in choicesUI.transform)
            Destroy(child.gameObject);
    }

    private void UpdateTaskManagerVariables()
    {
        TaskManager tasks = FindObjectOfType<TaskManager>();

        tasks.givenTrophy = (bool)inkStory.variablesState["givenTrophy"];
        tasks.hitBullseyes = (bool)inkStory.variablesState["hitBullseyes"];
        tasks.movement = inkStory.variablesState["movement"].ToString();
    }


}
