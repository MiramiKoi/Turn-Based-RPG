using UnityEngine;

namespace Runtime.Units
{
    public class UnitView : MonoBehaviour
    {
        [SerializeField] private Transform _transform;
        public Transform Transform => _transform;

        [SerializeField] private SpriteRenderer _spriteRenderer;
        public SpriteRenderer SpriteRenderer => _spriteRenderer;

        [SerializeField] private Animator _animator;
        public Animator Animator => _animator;
    }
}