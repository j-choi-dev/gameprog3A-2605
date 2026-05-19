using SampleApp.Model;
using System;
using UnityEngine;

namespace SampleApp.View
{
    public abstract class ObservableFloatInput : MonoBehaviour
    {
        public abstract float Value { get; set; }
        public abstract bool Interactable { get; set; }

        public abstract IObservable<float> OnValueChanged { get; }
        public abstract void SetValueWithoutNotify( float value );

        public abstract void SetIndeterminateValue();

        private FloatValueRange _range = null;

        protected float ApplyRange( float value )
        {
            return _range != null ? _range.Apply( value ) : value;
        }
    }
}
