using System;
using UnityEngine;

namespace SampleCharacterSDK.Domain
{
    public interface ICharacter
    {
        string ID { get; }
        Transform Transform { get; }

        IObservable<Vector3> OnPositionChanged { get; }

        void SetID(string id);
        void SetPositionX(float val);
        void SetPositionY(float val);
        void SetPositionZ(float val);
        void SetPosition(Vector3 vector);

    }
}
