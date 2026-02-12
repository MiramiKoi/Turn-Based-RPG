using UnityEngine;

namespace Runtime.Units.Collection
{
    public class UnitModelCollectionView : MonoBehaviour
    {
        public Transform Transform { get; private set; }

        private void Awake()
        {
            Transform = transform;
        }
    }
}