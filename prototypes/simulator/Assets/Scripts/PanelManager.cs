using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    public GameObject[] panels;
    public Button[] buttons;
    public Camera mainCamera; // Assign your main camera here in the Inspector

    private int activePanelIndex = -1;
    private Vector3 originalCameraPosition;
    private bool cameraMoved = false;

    void Start()
    {
        // Store original camera position
        if (mainCamera != null)
        {
            originalCameraPosition = mainCamera.transform.position;
        }

        // Attach button listeners
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            buttons[i].onClick.AddListener(() => TogglePanel(index));
        }
    }

    void TogglePanel(int panelIndex)
    {
        if (activePanelIndex == panelIndex)
        {
            // Deactivate all panels
            foreach (GameObject panel in panels)
            {
                panel.SetActive(false);
            }
            activePanelIndex = -1;

            // Move camera back
            ResetCameraPosition();
        }
        else
        {
            // Deactivate all panels
            foreach (GameObject panel in panels)
            {
                panel.SetActive(false);
            }

            // Activate selected panel
            if (panelIndex >= 0 && panelIndex < panels.Length)
            {
                panels[panelIndex].SetActive(true);
                activePanelIndex = panelIndex;

                // Move camera left
                MoveCameraLeft();
            }
        }
    }

    void MoveCameraLeft()
{
    if (mainCamera != null && !cameraMoved)
    {
        mainCamera.transform.position = originalCameraPosition + new Vector3(2f, 0f, -3.5f); // +2 on x
        cameraMoved = true;
    }
}


    void ResetCameraPosition()
    {
        if (mainCamera != null && cameraMoved)
        {
            mainCamera.transform.position = originalCameraPosition;
            cameraMoved = false;
        }
    }
}
