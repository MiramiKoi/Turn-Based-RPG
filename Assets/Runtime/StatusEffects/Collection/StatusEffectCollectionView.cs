using UnityEngine;

namespace Runtime.StatusEffects.Collection
{
    public class StatusEffectCollectionView : MonoBehaviour
    {
        public Transform Transform { get; private set; }

        private void Awake()
        {
            Transform = transform;
        }
    }
}