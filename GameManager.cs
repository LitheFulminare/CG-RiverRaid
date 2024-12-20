﻿using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CG
{
    internal static class GameManager
    {
        // componentes "herdados" do Game
        // serve pra não ficar escrevendo 'Game.' toda hora
        static Camera camera = Game.camera;
        static Player player = Game.player;

        static List<Obstacle> obstacles = Game.obstacles;
        static List<Shot> projectiles = Game.projectiles;

        static float obstacleSize = Game.obstacleSize;
        static float projectileSize = Game.projectileSize;

        // componentes proprios do GameManager
        static float obstacleSpawnTimer = 0f; // reseta sempre que um obstaculo spawna
        static float obstacleSpawnRate = 2f; // frequencia de spawn, maior frequencia gera mais obstaculos
        static float obstacleSpawnInterval = 1 / obstacleSpawnRate;

        public static void Start()
        {
            Game.bottomWaterTransform.position.Y = -2f;

            ResetMap();
        }

        public static void Update(float delta)
        {
            // ------ player ------
            player.Update(delta);
        
            // ------ obstaculos e projeteis ------
            obstacleSpawnTimer += delta;
            if (obstacleSpawnTimer >= obstacleSpawnInterval)
            {
                SpawnObstacle();
            }

            // aparentemente mudar os elementos da lista no meio do foreach da problema
            // o 'ToList()' resolve isso
            foreach (var obstacle in obstacles.ToList())
            {
                obstacle.Update(delta);

                foreach (var projectile in projectiles.ToList())
                {
                    if (projectile.ExceededLifespan)
                    {
                        projectiles.Remove(projectile);
                        break;
                    }
                  
                    projectile.Update(delta);

                    if (CheckCollision(projectile.Transform.position, obstacle.Transform.position, obstacleSize, projectileSize))
                    {
                        Obstacle.IncreaseSpeed();
                        obstacleSpawnInterval /= 1.01f;

                        obstacles.Remove(obstacle);
                        projectiles.Remove(projectile);
                        break;
                    }
                }

                // 0.5 é o raio do collider do jogador
                if (CheckCollision(player.Transform.position, obstacle.Transform.position, obstacleSize / 2, 0.5f))
                {
                    player.TakeDamage();
                }               
            }
        }

        // precisa do OpenTK.Mathematics senão ele reclama de ambiguidade do Vector3
        // o ideal seria refatorar como colisão é processada, isso daqui tá muito longo
        private static bool CheckCollision(OpenTK.Mathematics.Vector3 collider1position, OpenTK.Mathematics.Vector3 collider2position, float collider1Size, float collider2Size)
        {
            /*
            diretamente do processing
             ship.getX() >= powerup.getX() - 35 &&
             ship.getX() <= powerup.getX() + 35 &&
             ship.getY() >= powerup.getY() - 35 &&
             ship.getY() <= powerup.getY() + 35)
            */

            float totalOffset = collider1Size + collider2Size;

            return
                Math.Abs(collider1position.X - collider2position.X) <= totalOffset &&
                Math.Abs(collider1position.Z - collider2position.Z) <= totalOffset;
        }

        public static void ResetMap()
        {
            // camera
            camera.position.Z = 8f;
            camera.position.Y = 3.5f;
            camera.rotation.X = 320f;

            // player
            player.ResetPostion();

            // map
            obstacles.Clear();
            obstacleSpawnInterval = 1 / obstacleSpawnRate;
            Obstacle.ResetSpeed();
        }

        private static void SpawnObstacle()
        {
            // reseta o timer
            obstacleSpawnTimer = 0f;

            // cria um novo obstaculo e adiciona na fila
            obstacles.Add(new Obstacle());

            if (obstacles.Count > 15)
            {
                obstacles.RemoveAt(0);
            }
        }
    }
}
