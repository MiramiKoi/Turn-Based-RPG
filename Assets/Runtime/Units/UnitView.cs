using Runtime.StatusEffects.Collection;
using UnityEngine;

namespace Runtime.Units
{
    public class UnitView : MonoBehaviour
    {
        public Transform Transform { get; private set; }

        [field: SerializeField] public SpriteRenderer SpriteRenderer { get; private set; }

        [field: SerializeField] public Animator Animator { get; private set; }

        [field: SerializeField] public StatusEffectCollectionView StatusEffectCollectionView { get; private set; }

        private void Awake()
        {
            Transform = transform;
        }
    }
}