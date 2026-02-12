using UniRx;

namespace Runtime.UI.Blocker
{
    public class UIBlockerModel
    {
        public BoolReactiveProperty IsPointerOverUI { get; } = new();
    }
}