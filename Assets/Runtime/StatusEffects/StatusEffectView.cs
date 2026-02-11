using UnityEngine;

namespace Runtime.StatusEffects
{
    public class StatusEffectView : MonoBehaviour
    {
        public Transform Transform { get; private set; }
        public GameObject GameObject { get; private set; }

        public ParticleSystem OnApplyParticle => _onApplyParticle;
        public ParticleSystem OnTickParticle => _onTickParticle;
        public ParticleSystem OnRemoveParticle => _onRemoveParticle;

        [SerializeField] private ParticleSystem _onApplyParticle;
        [SerializeField] private ParticleSystem _onTickParticle;
        [SerializeField] private ParticleSystem _onRemoveParticle;

        private void Awake()
        {
            Transform = transform;
            GameObject = gameObject;
        }
    }
}