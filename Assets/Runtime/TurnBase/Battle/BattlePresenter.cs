using System.Linq;
using Runtime.Agents;
using Runtime.Common;
using Runtime.Core;

namespace Runtime.TurnBase.Battle
{
    public class BattlePresenter : IPresenter
    {
        private readonly BattleModel _model;
        private readonly World _world;

        public BattlePresenter(BattleModel model, World world)
        {
            _model = model;
            _world = world;
        }

        public void Enable()
        {
            _world.TurnBaseModel.OnBattleTick += OnBattleTick;
        }

        public void Disable()
        {
            _world.TurnBaseModel.OnBattleTick -= OnBattleTick;
        }
        
        private void OnBattleTick()
        {
            var playerFraction = _world.PlayerModel.Value.Description.Fraction;
            foreach (var agent in _world.UnitCollection.Models.Values.OfType<AgentModel>())
            {
                var agentEnemyFraction = agent.Description.EnemyFractions;
                if (!agent.IsDead && agent.State.Visible.Value && agentEnemyFraction.Contains(playerFraction))
                {
                    _model.EnterToBattle(agent);
                }
                else
                {
                    _model.LeaveBattle(agent);
                }
            }
        }
    }
}