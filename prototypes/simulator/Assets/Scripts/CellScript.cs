using UnityEngine;
using System.Collections.Generic;

// Represents a single cell in the simulation grid
// Handles the cell's state and visual representation
public class CellScript : MonoBehaviour
{
    // References to visual components
    [SerializeField] GameObject selectionPlane;
    [SerializeField] GameObject heightCube;
    private Material heightCubeMaterial;

    Color defaultColor;

    // Cell state with property to update visuals when changed
    public CellState State = new CellState();


    void Start()
    {
        // Cache the material for performance and initialize visuals
        heightCubeMaterial = heightCube.GetComponentInChildren<Renderer>().material;
        defaultColor = heightCubeMaterial.color;
        UpdateVisuals();
    }

    void Update()
    {
        
    }

    void ResetCellState() {
        State.height = 0;
        UpdateVisuals();
    }   

    public void Hover() {

        Debug.Log("Hovering");
        selectionPlane.SetActive(true);

        // Update the selection plane's position to match the state's height
        float height = transform.position.y + State.height + 0.2f;
        selectionPlane.transform.position = new Vector3(selectionPlane.transform.position.x, height, selectionPlane.transform.position.z);
    }

    public void Unhover() {
        selectionPlane.SetActive(false);
    }

   public void Clicked() {
        Debug.Log("Cell clicked, updating state.");

        float cost = 0f;

        // Determine the cost based on the selection
        if (GridManager.Instance.GetLastSelection() == GridManager.SelectionType.Soil) {
            cost = GridManager.Instance.GetSoilCost(GridManager.Instance.GetSelectedSoilType());
        }
        else if (GridManager.Instance.GetLastSelection() == GridManager.SelectionType.Item) {
            cost = GridManager.Instance.GetItemCost(GridManager.Instance.GetSelectedItem());
        }

        // Check if the player has enough money
        if (GridManager.Instance.playerMoney >= cost) {

            // Subtract the cost and place the thing
            GridManager.Instance.playerMoney -= cost;
            GridManager.Instance.UpdateMoneyUI();

            // Update the cell state based on the selection
            if (GridManager.Instance.GetLastSelection() == GridManager.SelectionType.Soil) {
                State.soilType = GridManager.Instance.GetSelectedSoilType();
            } else if (GridManager.Instance.GetLastSelection() == GridManager.SelectionType.Item) {
                State.itemType = GridManager.Instance.GetSelectedItem();
            }
            
            UpdateVisuals();
        } 
    }

    // public void RightClicked() {
        
    // }   

    // Calculates the next state of this cell for the simulation
    public CellState GenerateNextSimulationStep()
    {
        // Create a copy of the current state to modify
        CellState nextState = this.State.Clone();

        // This is just an example
        //ApplyMountainSmoothing(nextState);

        return nextState;
    }

    // void ApplyMountainSmoothing(CellState cellState) {
    //     // Get all neighboring cells (excluding the current cell)
    //     List<CellScript> neighbors = GridManager.Instance.GetNeighbors(this, true);
        
    //     // Calculate the average height of all neighboring cells
    //     float totalHeight = 0;
    //     foreach (CellScript neighbor in neighbors) {
    //         totalHeight += neighbor.State.height;
    //     }
        
    //     // Set the next height to be the average of all neighbors
    //     // This creates a smoothing/diffusion effect across the grid
    //     cellState.height = totalHeight / neighbors.Count;
    // }

    // Updates the visual representation of the cell based on its state
    public void UpdateVisuals() {

   
    if (heightCube != null) {
        heightCube.transform.localScale = new Vector3(1, State.height, 1);
    }

      // Change the color based on the soil type
    switch (State.soilType) {
        case SoilType.Compost:
            heightCubeMaterial.color = new Color(0.243f, 0.122f, 0.114f); 
            Debug.Log("Changed color");
            break;
        case SoilType.Topsoil:
            heightCubeMaterial.color = new Color(0.6f, 0.3f, 0.1f);  
            break;
        case SoilType.Sand:
            heightCubeMaterial.color = new Color(0.824f, 0.706f, 0.549f); 
            break;
        default:
            heightCubeMaterial.color = defaultColor;  
            break;
    }

    foreach (Transform child in transform) {
        child.gameObject.SetActive(false);
    }

    transform.Find("HeightCube").gameObject.SetActive(true);

    switch (State.itemType) {
        case ItemType.Flower:
            transform.Find("Flower").gameObject.SetActive(true);
            break;
        case ItemType.Cactus:
            transform.Find("Cactus").gameObject.SetActive(true);
            break;
        case ItemType.Bush:
            transform.Find("Bush").gameObject.SetActive(true);
            break;
        case ItemType.Path:
            transform.Find("Path").gameObject.SetActive(true);
            break;
        case ItemType.Well:
            transform.Find("Well").gameObject.SetActive(true);
            break;
        case ItemType.Hose:
            transform.Find("Hose").gameObject.SetActive(true);
            break;
        default:
            break;
    }
}



}
