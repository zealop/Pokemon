public class Move
{
    public MoveBase Base { get; set; }
    public int PP { get; set; }

    public bool Disabled { get; set; }
    public string Name { get => Base.Name; }
    public Move(MoveBase pBase)
    {
        Base = pBase;
        PP = Base.PP;
    }
}
