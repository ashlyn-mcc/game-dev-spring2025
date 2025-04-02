public enum SoilType {
    Default,
    Compost,
    Topsoil,
    Sand
}

public enum ItemType {
    None,
    Flower,
    Cactus,
    Bush,
    Path,
    Well,
    Hose
}

public class CellState {
    public int x;
    public int y;
    public float height;
    public SoilType soilType = SoilType.Default;
    public ItemType itemType = ItemType.None;
    public string pathStateVisuals = "default";

    public CellState Clone() {
        return new CellState {
            x = this.x,
            y = this.y,
            height = this.height,
            soilType = this.soilType,
            itemType = this.itemType,
            pathStateVisuals = this.pathStateVisuals
        };
    }
}
