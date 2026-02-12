using System.Collections.Generic;
using Runtime.Common;
using Runtime.Stats;

namespace Runtime.Units.Stats
{
    public class UnitStatsPresenter : IPresenter
    {
        private readonly UnitModel _model;
        private readonly List<StatPresenter> _statPresenters = new();

        public UnitStatsPresenter(UnitModel model)
        {
            _model = model;
        }

        public void Enable()
        {
            foreach (var stat in _model.Stats)
            {
                var statPresenter = new StatPresenter(stat);
                statPresenter.Enable();
                _statPresenters.Add(statPresenter);
            }
        }

        public void Disable()
        {
            foreach (var statPresenter in _statPresenters)
            {
                statPresenter.Disable();
            }

            _statPresenters.Clear();
        }
    }

}