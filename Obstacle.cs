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

        public bool isDestroyed = false;
        public Transform transform;
        
        public Obstacle()
        {
            transform = new Transform();
            //_collider = new Collider(transform.position, Game.obstacleSize);
            OnSpawn();
        }

        public void OnSpawn()
        {
            Random random = new Random();           
            transform.position.X = random.Next(-5, 5); // posição aleatoria da esquerda pra direita
            transform.position.Z = -13f; // faz spawnar no fundo, fora da tela
            transform.position.Y += 1f;
        }
    }
}
