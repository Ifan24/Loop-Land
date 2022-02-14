
public class PathColorController : Grids
{
    protected override void Start() {
        base.Start();
    }
    
    public override void CardHoverIndicator() {
        rd.material.color = warningColor;
    }
}
