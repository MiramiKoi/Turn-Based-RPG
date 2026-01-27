using UnityEngine;

namespace Runtime.Units
{
    public class UnitView : MonoBehaviour
    {
        [SerializeField]
        private Transform _transform;
        public Transform Transform => _transform;
    }
}