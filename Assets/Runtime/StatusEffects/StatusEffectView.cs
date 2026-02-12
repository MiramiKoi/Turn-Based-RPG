using UnityEngine;

namespace Runtime.StatusEffects
{
    public class StatusEffectView : MonoBehaviour
    {
        public Transform Transform { get; private set; }
        public GameObject GameObject { get; private set; }

        [field: SerializeField] public ParticleSystem OnApplyParticle { get; private set; }
        [field: SerializeField] public ParticleSystem OnTickParticle { get; private set; }
        [field: SerializeField] public ParticleSystem OnRemoveParticle { get; private set; }

        private void Awake()
        {
            Transform = transform;
            GameObject = gameObject;
        }
    }
}