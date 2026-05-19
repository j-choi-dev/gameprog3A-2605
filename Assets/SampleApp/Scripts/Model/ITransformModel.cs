using System;
using UnityEngine;

namespace SampleApp.Model
{
    public interface ITransformModel
    {
        IObservable<Vector3> OnPositionChanged { get; }
    }
}