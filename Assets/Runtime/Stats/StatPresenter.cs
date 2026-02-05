using Runtime.Common;
using UnityEngine;

namespace Runtime.Stats
{
    public class StatPresenter : IPresenter
    {
        private StatModel _model;
        
        public StatPresenter(StatModel model)
        {
            _model = model;
        }
        
        public void Enable()
        {
            _model.ValueChanged += OnValueChanged;
        }
        
        public void Disable()
        {
            _model.ValueChanged -= OnValueChanged;
        }
        
        private void OnValueChanged(float value)
        {
            Debug.Log($"Id: {_model.Description.Id} Value: {value}");
        }
    }
}
