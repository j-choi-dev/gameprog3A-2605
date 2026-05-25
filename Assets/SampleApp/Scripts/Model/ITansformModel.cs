using System;
using UnityEngine;

namespace SampleApp.Model
{
    public interface ITansformModel
    {
        IObservable<Vector3> OnPositionChanged { get; }
    }
}
