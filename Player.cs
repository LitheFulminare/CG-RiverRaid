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
        private int _life = 3;
        private int _fuel = 100;

        private float _speed = 3f;
        private float _iFrames = 1f;
        private float _iFramesTimer = 0f;

        private bool isInvincible = false;

        private Transform _transform;

        public Transform Transform => _transform;

        public Player()
        {
            _transform = new Transform();
        }

        public void Update(float delta)
        {
            if (isInvincible)
            {
                _iFramesTimer += delta;

                if (_iFramesTimer < _iFrames) return;

                Console.WriteLine("Invincibilidade acabou");
                _iFramesTimer = 0;
                isInvincible = false;
            }

            //if (KeyboardState.IsKeyDown(Keys.Right))
            //{
            //    _transform.position.X += _speed * delta;
            //}
        }

        public void Move(int direction, float delta)
        {
            _transform.position.X += _speed * delta * direction;
        }

        public void ResetPostion()
        {
            _transform.position.Z = 5.5f;
            _transform.position.X = 0f;
        }

        public void Rotate(float speed, float delta)
        {
            _transform.rotation.Y += delta * speed;
        }

        public void TakeDamage()
        {
            if (isInvincible) return;

            Console.WriteLine("Player tomou dano");
            _life -= 1;
            isInvincible = true;
        }
    }
}
