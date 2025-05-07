using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    public GameObject[] panels;
    public Button[] buttons;
    public Camera mainCamera; 

    private int activePanelIndex = -1;
    private Vector3 originalCameraPosition;
    private bool cameraMoved = false;

    void Start()
    {
        
        
        originalCameraPosition = mainCamera.transform.position;

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
            foreach (GameObject panel in panels)
            {
                panel.SetActive(false);
            }
            activePanelIndex = -1;

            ResetCameraPosition();
        }
        else
        {
            foreach (GameObject panel in panels)
            {
                panel.SetActive(false);
            }

            if (panelIndex >= 0 && panelIndex < panels.Length)
            {
                panels[panelIndex].SetActive(true);
                activePanelIndex = panelIndex;

                MoveCameraLeft();
            }
        }
    }

    void MoveCameraLeft()
{
    if (mainCamera != null && !cameraMoved)
    {
        mainCamera.transform.position = originalCameraPosition + new Vector3(2f, 0f, -3.5f); 
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
