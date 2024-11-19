using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG
{
    internal class Shot
    {
        private float _velocity = 5f;
        Transform _transform;

        public float Velocity => _velocity;
        public Transform Transform => _transform;
        
        public Shot()
        {
            _transform = new Transform();
        }

        public void UpdateMovement(float delta)
        {
            _transform.position.Z += _velocity * delta;
        }
    }
}
