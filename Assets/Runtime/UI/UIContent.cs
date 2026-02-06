using UnityEngine.UIElements;

namespace Runtime.UI
{
    public class UIContent
    {
        public VisualElement GameplayContent { get; }

        public UIContent(UIDocument gameplayDocument)
        {
            GameplayContent = gameplayDocument.rootVisualElement.Q<VisualElement>("content");
        }
    }
}