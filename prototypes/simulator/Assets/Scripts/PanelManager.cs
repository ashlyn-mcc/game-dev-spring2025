using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    public GameObject[] panels;      // Assign your panels here (size = 3)
    public Button[] buttons;         // Assign your 3 buttons here

    void Start()
    {
        // Attach button click listeners
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i; // Capture the index for the lambda
            buttons[i].onClick.AddListener(() => ShowPanel(index));
        }
    }

    void ShowPanel(int panelIndex)
    {
        // Disable all panels
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }

        // Enable the selected panel
        if (panelIndex >= 0 && panelIndex < panels.Length)
        {
            panels[panelIndex].SetActive(true);
        }
    }
}
