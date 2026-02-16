using System;
using Runtime.Equipment;
using Runtime.Stats;
using UnityEngine;

namespace Runtime.Units.Combat
{
    public class UnitCombatModel
    {
        private readonly StatModelCollection _stats;
        private readonly EquipmentModel _equipment;
        private readonly UnitStateModel _state;

        public event Action OnAttacked;
        public event Action OnDamaged;

        public UnitCombatModel(StatModelCollection stats, EquipmentModel equipment, UnitStateModel state)
        {
            _stats = stats;
            _equipment = equipment;
            _state = state;
        }

        public float GetDamage()
        {
            OnAttacked?.Invoke();

            var damage = _stats["attack_damage"].Value;
            if (_equipment.TryGetStats("weapon", out var weaponStats))
            {
                damage = weaponStats["damage"].MaxValue;
            }

            return damage;
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

            if (_equipment.TryGetStats("armour", out var armourStats))
            {
                damage *= 1 - armourStats["protection"].MaxValue / 100f;
            }

            _stats["health"].ChangeValue(-damage);
        }
    }
}