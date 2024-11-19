using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CG
{
    internal class Player
    {
        // life e fuel agora não são servem pra nada, n deu tempo de implemtar
        private int _life = 3;
        private int _fuel = 100;

        private float _speed = 3f;
        private float _rotationSpeed = 400f;

        private float _deathTime = 2.5f; // tempo que é esperado antes de resetar
        private float _respawnTimer = 0f; // timer q começa a contar quando o player morre

        private bool _isDead = false;

        private Transform _transform;

        public Transform Transform => _transform;
        public bool IsDead => _isDead;

        public Player()
        {
            _transform = new Transform();
        }

        public void Update(float delta)
        {
            // respawn
            if (_isDead)
            {
                _respawnTimer += delta;

                if (_respawnTimer < _deathTime) return;

                Respawn();
            }

            //if (KeyboardState.IsKeyDown(Keys.Right))
            //{
            //    _transform.position.X += _speed * delta;
            //}
        }

        // o ideal seria substituir isso pra toda a logica de movimento do player estar contida dentro do player (ou na sua propria classe)
        public void Move(int direction, float delta)
        {
            _transform.position.X += _speed * delta * direction;
            _transform.rotation.Y -= delta * _rotationSpeed * direction;
        }

        public void ResetPostion()
        {
            _transform.position.Z = 5.5f;
            _transform.position.X = 0f;
        }

        public void TakeDamage()
        {
            if (_isDead) return;

            _life -= 1;
            _isDead = true;
        }
        
        public Shot Shoot()
        {
            return new Shot(_transform.position);
        }

        private void Respawn()
        {
            ResetPostion();
            _isDead = false;
            _respawnTimer = 0f;
            GameManager.ResetMap();
        }
    }
}
