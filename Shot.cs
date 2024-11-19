using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CG
{
    internal class Shot
    {
        private float _velocity = 20f;

        private float lifespanTimer = 0f;
        private float lifespan = 2f; // tempo que o tiro dura antes de ser destruido

        private bool _exceededLifespan = false;

        Transform _transform;

        public float Velocity => _velocity;
        public bool ExceededLifespan => _exceededLifespan;
        public Transform Transform => _transform;

        // msm coisa do CheckCollision no GameManger,
        // sem especificar OpenTK.Mathematics ele não sabe qual Vector3 é
        public Shot(OpenTK.Mathematics.Vector3 playerPostision)
        {
            _transform = new Transform();
            _transform.position = playerPostision;
        }

        public void Update(float delta)
        {
            _transform.position.Z -= _velocity * delta;

            lifespanTimer += delta;
            _exceededLifespan = lifespanTimer >= lifespan;
        }
    }
}
