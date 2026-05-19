using System;
using UniRx;
using UnityEngine;


namespace SampleCharacterSDK.Domain
{
    public class Character : MonoBehaviour, ICharacter
    {
        [SerializeField] private GameObject _character;

        public Transform Transform => _character.transform;

        public string ID { get; private set;  }

        private Subject<Vector3> _onPositionChanged = new Subject<Vector3>();
        public IObservable<Vector3> OnPositionChanged => throw new NotImplementedException();

        public void SetID(string id)
            => ID = id;

        public void SetPosition(Vector3 vector)
        {
            Transform.localPosition = vector;
            _onPositionChanged.OnNext(Transform.localPosition);
        }

        public void SetPositionX(float val)
        {
            Transform.localPosition = new Vector3(val, Transform.localPosition.y, Transform.localPosition.z);
        _onPositionChanged.OnNext(Transform.localPosition);
        }

        public void SetPositionY(float val)
        {
            Transform.localPosition = new Vector3( Transform.localPosition.x, val, Transform.localPosition.z);
            _onPositionChanged.OnNext(Transform.localPosition);
        }

        public void SetPositionZ(float val)
        {
            Transform.localPosition = new Vector3(Transform.localPosition.x, Transform.localPosition.y, val);
            _onPositionChanged.OnNext(Transform.localPosition);
        }
    }
}
