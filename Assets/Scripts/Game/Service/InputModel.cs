using UnityEngine;

namespace Game.Service
{
    public class InputModel
    {
        private readonly Vector3 _acceleration;
        private readonly Vector3 _angularVelocity;

        public InputModel(Vector3 acceleration, Vector3 angularVelocity)
        {
            _acceleration = acceleration;
            _angularVelocity = angularVelocity;
        }

        public Vector3 Acceleration => _acceleration;

        public Vector3 AngularVelocity => _angularVelocity;

        public override string ToString()
        {
            return "_acceleration = " + _acceleration.ToString()
                                      + " _angularVelocity = " + _angularVelocity.ToString();
        }
    }
}