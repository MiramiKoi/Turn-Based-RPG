using System;
using Runtime.Stats;
using UnityEngine;

namespace Runtime.Units.Combat
{
    public class UnitCombatModel
    {
        private readonly StatModelCollection _stats;
        private readonly UnitStateModel _state;

        public event Action OnAttacked;
        public event Action OnDamaged;

        public UnitCombatModel(StatModelCollection stats, UnitStateModel state)
        {
            _stats = stats;
            _state = state;
        }

        public float GetDamage()
        {
            OnAttacked?.Invoke();
            return _stats["attack_damage"].Value;
        }

        public bool CanAttack(Vector2Int target)
        {
            var current = _state.Position.Value;

            return Math.Abs(current.x - target.x) <= _stats["attack_range"].Value &&
                   Math.Abs(current.y - target.y) <= _stats["attack_range"].Value;
        }

        public void TakeDamage(float damage)
        {
            OnDamaged?.Invoke();
            _stats["health"].ChangeValue(-damage);
        }
    }
}