using UnityEngine;
using TMPro;
using System;
using UniRx;

namespace SampleApp.View
{
    public class ObservableFloatInputTMPro : ObservableFloatInput
    {
        [SerializeField] private TMP_InputField m_Input = null;
        [SerializeField] private int _numDigits = 4;

        public override bool Interactable { get => m_Input.interactable; set => m_Input.interactable = value; }

        public override IObservable<float> OnValueChanged => m_Input.onEndEdit.AsObservable()
            .Select( _ => ApplyRange( SafeParse() ) );

        public override float Value
        {
            set
            {
                m_Input.text = ApplyRange( value ).ToString( $"F{_numDigits}" );
                m_Input.onEndEdit.Invoke( m_Input.text );
            }
            get
            {
                return SafeParse();
            }
        }

        private float SafeParse()
        {
            if( float.TryParse( m_Input.text, out float result ) )
            {
                return result;
            }
            return 0;
        }

        public override void SetValueWithoutNotify( float value )
        {
            if( m_Input.isFocused == false )
            {
                m_Input.SetTextWithoutNotify( ApplyRange( value ).ToString() );
            }
        }

        public override void SetIndeterminateValue()
        {
            m_Input.SetTextWithoutNotify( "---" );
        }
    }
}
