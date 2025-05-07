using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.UI;

// Manages the grid of cells for the simulation.
// Handles grid creation, cell updates, and provides utility functions for cell access.
public class GridManager : MonoBehaviour
{

    // Amount of money the player has to spend
    public float playerMoney = 450;  

    // Display text of player's money
    [SerializeField] 
    TextMeshProUGUI moneyText;  

    // Types a cell can be
    public enum SelectionType { None, Soil, Item }

    // The type of the last button selected (either a soil type or an item)
    private SelectionType lastSelection = SelectionType.None;

    // The last type of soil button clicked
    private SoilType selectedSoilType = SoilType.Default;

    // The last type of item button clicked
    private ItemType selectedItem = ItemType.None;

    // Singleton instance for easy access from other scripts
    public static GridManager Instance { get; private set; }

    // Reference to the prefab used to create each cell
    [SerializeField]
    GameObject cellPrefab;

    // UI Sliders that show a cell's values when hovering
    public Slider densitySlider;  
    public Slider substrateSlider; 
    public Slider hydrationSlider;

    // Grid dimensions
    [SerializeField]
    int gridW = 10;
    [SerializeField]
    int gridH = 5;

    // Materials for visual feedback when hovering over cells
    [SerializeField]
    Material hoverMaterial;
    [SerializeField]
    Material defaultMaterial;

    // Path finding variables
    Coroutine pathfindingCoroutine;
    float pathfindingSpeed = 0.05f;

    // Cell size parameters
    float cellWidth = 1;
    float cellHeight = 1;
    float spacing = 0.0f;

    // Maximum height value for cells (used for normalization)
    public float maxHeight = 5;

    // Timing control for simulation updates
    float nextSimulationStepTimer = 0;
    float nextSimulationStepRate = 0.5f;

    // The 2D array that stores all cell references
    public CellScript[,] grid;

    public CellScript startCell;
    public CellScript endCell;
    
    // Tracks which cell the mouse is currently hovering over
    public CellScript currentHoverCell;

    // Variables for showing the number of days on the UI
    public int currentDay = 0;
    [SerializeField] 
    TextMeshProUGUI dayText;

    // UI elements for the pop up panel with cell metrics
    [SerializeField] GameObject metricsPanel; 
    [SerializeField] TextMeshProUGUI densityText; 
    [SerializeField] TextMeshProUGUI hydrationText; 
    [SerializeField] TextMeshProUGUI substrateText;


    // How much each item costs to place
    public enum ItemCost {
    Compost = 15,
    Sand = 10,
    Topsoil = 5,
    Path = 25,
    Well = 100,
    Hose = 40,
    Flower = 35,
    Cactus = 75,
    Bush = 55
    }

    // Time progression variables
    float nextDayTimer = 0;
    float dayDuration = 20f;

    // Based on what UI buttons are clicked, sets which types of objects will be placed next.

    public void SetSoilToCompost()
    {
    selectedSoilType = SoilType.Compost;
    lastSelection = SelectionType.Soil;
    }

    public void SetSoilToTopsoil() {
        selectedSoilType = SoilType.Topsoil;
        lastSelection = SelectionType.Soil;
    }

    public void SetSoilToSand() {
        selectedSoilType = SoilType.Sand;
        lastSelection = SelectionType.Soil;
    }

    public void SetItemToFlower() {
        selectedItem = ItemType.Flower;
        lastSelection = SelectionType.Item;
    }

    public void SetItemToCactus() {
        selectedItem = ItemType.Cactus;
        lastSelection = SelectionType.Item;
    }

    public void SetItemToBush() {
        selectedItem = ItemType.Bush;
        lastSelection = SelectionType.Item;
    }

    public void SetItemToPath() {
        selectedItem = ItemType.Path;
        lastSelection = SelectionType.Item;
    }

    public void SetItemToWell() {
        selectedItem = ItemType.Well;
        lastSelection = SelectionType.Item;
    }

    public void SetItemToHose() {
        selectedItem = ItemType.Hose;
        lastSelection = SelectionType.Item;
    }

    public SelectionType GetLastSelection() {
        return lastSelection;
    }

    public ItemType GetSelectedItem() {
        return selectedItem;
    }

    // Initializes the singleton instance
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // Cleans up the singleton reference when destroyed
    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    void Start()
    {
        GenerateGrid();
        UpdateMoneyUI();
        UpdateDayUI();
    }


    // Called every frame
    void Update()
    {
        // Handle time progression for each day
        nextDayTimer -= Time.deltaTime;
        if (nextDayTimer <= 0)
        {
            // Make next day updates
            SimulationStep();
            nextDayTimer = dayDuration; 

            // Update the UI
            currentDay++;
            dayText.text = "Day " + currentDay;
        }


        // Handle mouse hover detection
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, float.MaxValue, LayerMask.GetMask("cell"))) {

            // Get the cell that was hit
            CellScript cs = hit.collider.gameObject.GetComponentInParent<CellScript>();
            Vector2Int gridPosition = new Vector2Int(cs.State.x, cs.State.y);
            
            // Reset previous hover cell's material if we've moved to a new cell
            if (currentHoverCell != null && currentHoverCell != grid[gridPosition.x, gridPosition.y]) {
                currentHoverCell.gameObject.GetComponentInChildren<Renderer>().material = defaultMaterial;
                currentHoverCell.Unhover();
            }

            // Update current hover cell and change its material
            currentHoverCell = grid[gridPosition.x, gridPosition.y];
            // Debug.Log("currentCell = "+ currentHoverCell);
            currentHoverCell.Hover();

            // Check if the hovered cell has a plant (flower, cactus, or bush)
            if (currentHoverCell.State.itemType == ItemType.Flower || currentHoverCell.State.itemType == ItemType.Cactus || currentHoverCell.State.itemType == ItemType.Bush)
            {
                // Show the panel
                metricsPanel.SetActive(true);

                // Update the position of the panel to be a little above and to the right of the mouse position
                Vector3 mousePos = Input.mousePosition;
                mousePos.x += 175f; 
                mousePos.y -= 50f; 
                metricsPanel.transform.position = mousePos;

                // Update panel text to display the metrics for the current hovered plant
                UpdateMetricsPanel(currentHoverCell);
            }
            else
            {
                // Hide the panel if the hover is not over a plant cell
                metricsPanel.SetActive(false);
            }


            if (Input.GetMouseButtonDown(0)) {
                
                currentHoverCell.Clicked();
            
            }
        }
    }

    void UpdateMetricsPanel(CellScript cell)
    {
        // Get the metrics for the hovered plant
        float density = GetDensityForCell(cell);
        float hydration = GetHydrationForCell(cell);
        float substrate = GetSubstrateForCell(cell);

        // Update UI with metrics
        densityText.text = "Density: " + density.ToString("F2");
        hydrationText.text = "Hydration: " + hydration.ToString("F2");
        substrateText.text = "Substrate: " + substrate;
    }

   
    float GetDensityForCell(CellScript cell)
    {

    // Get cell neighbors
    List<CellScript> neighbors = GetNeighbors(cell);

    // Count how many neighbors have an item on them
    int occupiedNeighbors = 0;
    foreach (var neighbor in neighbors)
    {
        if (neighbor.State.itemType != ItemType.None) 
        {
            occupiedNeighbors++;
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //I used AI to help me come up with these math functions below. I prompted it with "I want a mathmatical function 
    // that will gradually decrease with a variable up to 5, and sharply decrease if the variable is between 6 and 8"

    float density = 0f;

    if (occupiedNeighbors <= 5)
    {
       
        density = Mathf.Lerp(1f, 0f, Mathf.Pow(occupiedNeighbors / 5f, 2));  // Smooth decrease for 0-5 neighbors (using a quadratic function)
    }
    else
    {
        // Sharp drop after 6 neighbors
        density = Mathf.Max(0f, 1f - Mathf.Pow(occupiedNeighbors - 5f, 2) * 0.25f); // Sharp decrease after 5 neighbors, scaled down
    }
    
    density = Mathf.Clamp(density, 0f, 1f); // Return the final density value (clamped to ensure it stays within the 0 to 1 range)

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    // Set the slider's value based on the calculated density
    densitySlider.value = density;

    return density;

    }


    float GetHydrationForCell(CellScript cell)
    {

        // Get the cell neighbors
        List<CellScript> neighbors = GetNeighbors(cell);

        bool hasHoseNeighbor = false;
        CellScript hoseNeighbor = null;

        // Check if a plant cell has any hoses for neighbors
        foreach (var neighbor in neighbors)
        {
            if (neighbor.State.itemType == ItemType.Hose)
            {
                hasHoseNeighbor = true;
                hoseNeighbor = neighbor;
                break; 
            }
        }

        // If there are no hoses, plant is getting no water, hydration is 0
        if (!hasHoseNeighbor){
            return 0f;
        }

        // A* path finding to trace a path back through hoses to the nearest well
        CellScript nearestWell = FindNearestWell(hoseNeighbor);

        // If there isn't a well, then plant is getting no water, hydration is 0
        if (nearestWell == null){
             return 0f;
        }

        // A* path finding to trace a path back through hoses to the nearest well
        List<CellScript> path = FindPath(hoseNeighbor, nearestWell);

        // If there isn't a path, then plant is getting no water, hydration is 0
        if (path == null || path.Count == 0) {
            return 0f; 
        }

        // I kinda just randomly picked this value. It's the most number of hoses you can have to distribute water on one path.
        float maxPathLength = 10; 
        float pathCost = path.Count; 

        // Calculate the hydration based on length of path to well (Further from well = less water distributed)
        float hydration = Mathf.Clamp(1f - (pathCost / maxPathLength), 0f, 1f);
        hydrationSlider.value = hydration;

        return hydration;
    }

    float GetSubstrateForCell(CellScript cell)
    {
        float compatibility = 0f;

        // Flower 
        if (cell.State.itemType == ItemType.Flower)
        {
            switch (cell.State.soilType)
            {
                case SoilType.Topsoil:
                    compatibility = 1.0f;
                    break;
                case SoilType.Compost:
                    compatibility = 0.75f;
                    break;
                case SoilType.Default:
                    compatibility = 0.5f;
                    break;
                case SoilType.Sand:
                    compatibility = 0.25f;
                    break;
            }
        }

        // Bush
        else if (cell.State.itemType == ItemType.Bush)
        {
            switch (cell.State.soilType)
            {
                case SoilType.Compost:
                    compatibility = 1.0f;
                    break;
                case SoilType.Topsoil:
                    compatibility = 0.75f;
                    break;
                case SoilType.Default:
                    compatibility = 0.5f;
                    break;
                case SoilType.Sand:
                    compatibility = 0.25f;
                    break;
            }
        }

        // Cactus
        else if (cell.State.itemType == ItemType.Cactus)
        {
            switch (cell.State.soilType)
            {
                case SoilType.Sand:
                    compatibility = 1.0f;
                    break;
                case SoilType.Default:
                    compatibility = 0.75f;
                    break;
                case SoilType.Topsoil:
                    compatibility = 0.5f;
                    break;
                case SoilType.Compost:
                    compatibility = 0.25f;
                    break;
            }
        }

        // Update the UI
        substrateSlider.value = compatibility;
        substrateText.text = compatibility.ToString();

        return compatibility;
    }

    CellScript FindNearestWell(CellScript start)
    {
        // Going to store all the well scripts in a list
        List<CellScript> wells = new List<CellScript>();

        // Go through all the cells to find the wells
        foreach (var cell in grid)
        {
            if (cell.State.itemType == ItemType.Well)
            {
                wells.Add(cell);
            }
        }

        // If there aren't any wells in the garden return null
        if (wells.Count == 0){
            return null; 
        }

        // Initialize variables for finding the closest well
        CellScript closestWell = null;
        float shortestPath = float.MaxValue;


        // Goes through each well in the garden and calls Find Path (A* Path finding)
        // to determine what the closest well is
        foreach (var well in wells)
        {
            List<CellScript> path = FindPath(start, well);

            if (path != null && path.Count < shortestPath)
            {
                shortestPath = path.Count;
                closestWell = well;
            }
        }

        return closestWell;
    }

    List<CellScript> FindPath(CellScript start, CellScript goal)
    {

        // A* Variables
        List<CellScript> openSet = new List<CellScript> { start };
        Dictionary<CellScript, CellScript> cameFrom = new Dictionary<CellScript, CellScript>();
        Dictionary<CellScript, float> gScore = new Dictionary<CellScript, float>();
        Dictionary<CellScript, float> fScore = new Dictionary<CellScript, float>();

        // Give every cell an initial gScore and fScore
        foreach (var cell in grid)
        {
            gScore[cell] = float.MaxValue;
            fScore[cell] = float.MaxValue;
        }

        gScore[start] = 0;
        fScore[start] = Heuristic(start, goal);

        // As long as there are unexplored cells, keep searching
        while (openSet.Count > 0)
        {

            // Find the cell with the lowest fscore
            CellScript current = GetLowestFScoreCell(openSet, fScore);

            // If the lowest one is the goal well then stop and return the path
            if (current == goal) {
                return ReconstructPath(cameFrom, current);
            }

            // Once the cell has been explored get rid of it from the open set
            openSet.Remove(current);

            // Go through all the neighbors of the current cell
            foreach (CellScript neighbor in GetNeighbors(current))
            {
                // Only consider a cell as a viable path if it has a hose on it
                if (neighbor.State.itemType != ItemType.Hose && neighbor != goal){
                    continue; 
                }

                // Add 1 to the cost
                float tentativeGScore = gScore[current] + 1; 

                // Update the path only if the new path is shorter than the previous path
                if (tentativeGScore < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + Heuristic(neighbor, goal);

                    // If the neihgbor was not already being considered as an option for exploration add it
                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }

        // If there isn't a path, just return nothing.
        return null;
    }


    public SoilType GetSelectedSoilType() 
    {
        // Get what kind of soil type is currently selected
        Debug.Log("selected"+selectedSoilType);
        return selectedSoilType;
    }

    public float GetSoilCost(SoilType soilType) 
    {
        // Get how much each kind of soil costs
        switch (soilType) {
            case SoilType.Compost: 
                return (float)ItemCost.Compost;
            case SoilType.Sand: 
                return (float)ItemCost.Sand;
            case SoilType.Topsoil: 
                return (float)ItemCost.Topsoil;
            default: 
                return 0f;
        }
    }

    public float GetItemCost(ItemType itemType) {

        // Get how much each item costs to build/ plant
        switch (itemType) {
            case ItemType.Flower: 
                return (float)ItemCost.Flower;
            case ItemType.Cactus: 
                return (float)ItemCost.Cactus;
            case ItemType.Bush: 
                return (float)ItemCost.Bush;
            case ItemType.Path: 
                return (float)ItemCost.Path;
            case ItemType.Well: 
                return (float)ItemCost.Well;
            case ItemType.Hose: 
                return (float)ItemCost.Hose;
            default: 
                return 0f;
        }
    }

    private List<CellScript> ReconstructPath(Dictionary<CellScript, CellScript> cameFrom, CellScript current) {

        // Rreconstruct the path from start to goal
        List<CellScript> path = new List<CellScript>();
        path.Add(current);
        
        while (cameFrom.ContainsKey(current)) {
            current = cameFrom[current];
            path.Add(current);
        }
        
        // Reverse to get path from start to goal
        path.Reverse(); 
        return path;
    }

    private float CalculateCost(CellScript a, CellScript b) {
        return 1;
    }

    private float Heuristic(CellScript a, CellScript b) {
        return Mathf.Abs(a.State.x - b.State.x) + Mathf.Abs(a.State.y - b.State.y);
    }

    private CellScript GetLowestFScoreCell(List<CellScript> openSet, Dictionary<CellScript, float> fScore) {
        CellScript lowest = openSet[0];
        foreach (var cell in openSet) {
            if (fScore[cell] < fScore[lowest]) {
                lowest = cell;
            }
        }
        return lowest;
    }

    public List<CellScript> GetNeighbors(CellScript cell, bool includeDiagonals = false) {
        List<CellScript> neighbors = new List<CellScript>();
        for (int x = -1; x <= 1; x++) {
            for (int y = -1; y <= 1; y++) {
                if (Mathf.Abs(x) == Mathf.Abs(y) && !includeDiagonals){
                     continue;
                }
                CellState neighborState = GridManager.Instance.GetCellStateByIndex(cell.State.x + x, cell.State.y + y);
                if (neighborState != null) {
                    neighbors.Add(GridManager.Instance.grid[neighborState.x, neighborState.y]);
                }
            }
        }
        return neighbors;
    }

    // Advances the simulation by one step
    void SimulationStep() 
    {
        // Update the day UI text
        UpdateDayUI();

        float totalAddedMoney = 0f;

        // Prepare the next state array
        CellState[,] nextState = new CellState[gridW, gridH];

        for (int x = 0; x < gridW; x++) {
            for (int y = 0; y < gridH; y++) {
                CellScript cell = grid[x, y];

                // Generate the next simulation step for the cell
                nextState[x, y] = cell.GenerateNextSimulationStep();

                // If the cell contains a plant, calculate metrics
                if (IsPlant(cell.State.itemType)) {
                    float density = GridManager.Instance.GetDensityForCell(cell);
                    float hydration = GridManager.Instance.GetHydrationForCell(cell);
                    float substrate = GridManager.Instance.GetSubstrateForCell(cell);

                    // Compute overall plant health (0-1)
                    float avgValue = (substrate + hydration + density) / 3f;

                    // Convert to money gain ($0-$20 per plant)
                    float moneyForCell = avgValue * 20f;
                    totalAddedMoney += moneyForCell;
                }
            }
        }

        // Apply the new states to the grid
        for (int x = 0; x < gridW; x++) {
            for (int y = 0; y < gridH; y++) {
                grid[x, y].State = nextState[x, y];
                grid[x, y].UpdateVisuals();
            }
        }

        // Round and add money to player's total
        playerMoney += Mathf.RoundToInt(totalAddedMoney);
        UpdateMoneyUI();
    }

    // Check if an itemType is a plant
    bool IsPlant(ItemType itemType) 
    {
        return itemType == ItemType.Flower || itemType == ItemType.Cactus || itemType == ItemType.Bush;
    }

    public CellState GetCellStateByIndexWithWrap(int x, int y) {
        // Wrap coordinates to stay within grid bounds
        x = (x + gridW) % gridW;
        y = (y + gridH) % gridH;
        return grid[x,y].State;
    }

    public CellState GetCellStateByIndex(int x, int y) {
        if (x < gridW && x >= 0 && y < gridH && y >= 0) {
            return grid[x,y].State;
        }
        return null;
    }

    // Converts a world position to grid indices
    Vector2Int WorldPointToGridIndices(Vector3 worldPoint) {
        Vector2Int gridPosition = new Vector2Int();
        gridPosition.x = Mathf.FloorToInt(worldPoint.x / (cellWidth + spacing));
        gridPosition.y = Mathf.FloorToInt(worldPoint.z / (cellHeight + spacing));
        return gridPosition;
    }

    // Creates the grid of cells
    public void GenerateGrid()
    {
        // Clear any existing cells
        for (int i = transform.childCount - 1; i >= 0; i--) {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }

        // Initialize the grid array
        grid = new CellScript[gridW, gridH];

        // Create each cell in the grid
        for (int x = 0; x < gridW; x++) {
            for (int y = 0; y < gridH; y++) {
                // Calculate position based on cell size and spacing
                Vector3 pos = new Vector3(((cellWidth + spacing) * x) - 5, 0, ((cellHeight + spacing) * y) - 5);

                // Instantiate the cell and get its script component
                GameObject cell = Instantiate(cellPrefab, pos, Quaternion.identity);
                CellScript cs = cell.GetComponent<CellScript>();

                cs.State.soilType = SoilType.Default;

                // Set initial height and position in the grid
                cs.State.height = 0.25f;
                cs.State.x = x;
                cs.State.y = y;

                // Set cell size and parent
                cell.transform.localScale = new Vector3(cellWidth, 1, cellHeight);
                cell.transform.SetParent(transform);

                // Store reference in the grid array
                grid[x, y] = cs;
            }
        }
    }

    public void UpdateMoneyUI() {
        moneyText.text = "$" + playerMoney.ToString("F2");
    }

    void UpdateDayUI() {
        dayText.text = "Day " + currentDay;
    }
}
