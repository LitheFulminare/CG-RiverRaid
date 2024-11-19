using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG
{
    internal class Obstacle
    {
        //private Collider _collider;
        private static float _originalSpeed = 4f;
        private static float _speedCap = 5 * _originalSpeed;
        private static float _speed = _originalSpeed;

        private Transform _transform;

        private bool _isDestroyed = false;
        
        public Transform Transform => _transform;
        public bool IsDestroyed => _isDestroyed;
        
        public Obstacle()
        {
            _transform = new Transform();
            //_collider = new Collider(_transform.position, Game.obstacleSize);
            OnSpawn();
        }

        public void Update(float delta)
        {
            _transform.position.Z += _speed * delta;
        }

        public void OnSpawn()
        {
            Random random = new Random();
            
            // posição aleatoria no eixo Y
            float randomYPosition = (float)random.NextDouble();
            randomYPosition = Single.Lerp(-0.5f, 0.5f, randomYPosition);

            _transform.position.X = random.Next(-5, 5); // posição aleatoria da esquerda pra direita
            _transform.position.Z = -18f; // faz spawnar no fundo, fora da tela
            _transform.position.Y += randomYPosition;
        }

        // aumenta a velocidade quando um obstaculo é destruido, mas tem um cap
        public static void IncreaseSpeed()
        {
            float increasedSpeed = _speed * 1.05f;

            if (increasedSpeed > _speedCap)
            {
                _speed = _speedCap;
                return;
            }

            _speed = increasedSpeed;
        }

        public static void ResetSpeed()
        {
            _speed = _originalSpeed;
        }
    }
}
