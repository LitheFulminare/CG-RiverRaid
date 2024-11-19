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
        private static float _speedCap = 3 * _originalSpeed;
        private static float _speed = _originalSpeed;

        public bool isDestroyed = false;
        public Transform transform;
        
        public Obstacle()
        {
            transform = new Transform();
            //_collider = new Collider(transform.position, Game.obstacleSize);
            OnSpawn();
        }

        public void Update(float delta)
        {
            transform.position.Z += _speed * delta;
        }

        public void OnSpawn()
        {
            Random random = new Random();     
            
            // posição aleatoria no eixo Y
            float randomYPosition = (float)random.NextDouble();
            randomYPosition = Single.Lerp(-0.5f, 0.5f, randomYPosition);

            transform.position.X = random.Next(-5, 5); // posição aleatoria da esquerda pra direita
            transform.position.Z = -18f; // faz spawnar no fundo, fora da tela
            transform.position.Y += randomYPosition;
        }

        // aumenta a velocidade quando um obstaculo é destruido
        // tem um cap (agora é 3x a original)
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
