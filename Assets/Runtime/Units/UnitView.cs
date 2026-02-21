using Runtime.StatusEffects.Collection;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

namespace Runtime.Units
{
    public class UnitView : MonoBehaviour
    {
        public Transform Transform { get; private set; }

        [field: SerializeField] public SpriteRenderer SpriteRenderer { get; private set; }
        
        [field: SerializeField] public Transform TransformRenderer { get; private set; }

        [field: SerializeField] public Animator Animator { get; private set; }

        [field: SerializeField] public StatusEffectCollectionView StatusEffectCollectionView { get; private set; }

        [field: SerializeField] public Light2D Light { get; private set; }

        [field: SerializeField] public UIDocument UIDocument { get; private set; }

        private void Awake()
        {
            Transform = transform;
        }
    }
}