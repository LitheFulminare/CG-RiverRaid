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
            transform.position.Z = -13f; // faz spawnar no fundo, fora da tela
            transform.position.Y += randomYPosition;
        }
        public static void IncreaseSpeed()
        {
            _speed *= 1.05f;
        }

        public static void ResetSpeed()
        {
            _speed = _originalSpeed;
        }
    }
}
