using SampleApp.Presenter;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SampleApp.View
{
    public class SampleAppUIView : MonoBehaviour, ISampleAppUIView
    {
        [SerializeField] private ObservableDropdown _dropDown;
        [SerializeField] private ObservableVector3Input _vector3Input;

        public IObservable<int> OnSelectIndexChange => _dropDown.OnValueChanged;

        public IObservable<string> OnSelectNameChange => _dropDown.OnTextChanged;

        private void Start()
        {
            _vector3Input.Interactable = false;
        }

        public void SetOptions( IReadOnlyList<string> options )
        {
            _dropDown.Options.Clear();
            _dropDown.SetOptions( options );
        }

        public void SetTransformInteractable( bool isInteractable )
            => _vector3Input.Interactable = isInteractable;

        public void SetVector3( Vector3 vector )
            => _vector3Input.SetValueWithoutNotify( vector );
    }
}
