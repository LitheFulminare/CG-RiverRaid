﻿using OpenTK.Windowing.GraphicsLibraryFramework;
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
        private float _deathTime = 1f; // tempo que é esperado antes de resetar
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

        // o ideal seria substituir isso pra toda a logica de movimento do player estar contida dentro do player
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
            if (_isDead) return;

            _life -= 1;
            _isDead = true;
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