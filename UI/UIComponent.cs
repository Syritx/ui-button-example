using OpenTK.Mathematics;

namespace _3d.UI {
    public abstract class UIComponent {
        
        public virtual void Render() {}
        public virtual void ChangeDefaultDimensions(Vector2 dimension) {}
    }
}