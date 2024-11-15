using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG
{
    internal class GameManager
    {
        // componentes "herdados" do Game
        static Camera camera = Game.camera;
        static Transform playerTransform = Game.playerTransform;
        static List<Obstacle> obstacles = Game.obstacles;

        // componentes proprios do GameManager
        static float obstacleSpawnTimer = 0f; // reseta sempre que um obstaculo spawna
        static float obstacleSpawnRate = 3f; // frequencia de spawn, maior frequencia gera mais obstaculos

        static float obstacleSpawnInterval;

        public static void Start()
        {
            // transforma frequencia em intervalo (é mais intuivo assim)
            obstacleSpawnInterval = 1 / obstacleSpawnRate; 

            ResetToStartPosition();
        }

        public static void Update(float delta)
        {
            // player
            playerTransform.rotation.Y += delta * 9f;

            // obstaculos
            obstacleSpawnTimer += delta;
            if (obstacleSpawnTimer >= obstacleSpawnInterval)
            {
                SpawnObstacle();
            }

            foreach (var obstacle in obstacles)
            {
                obstacle.transform.position.Z += 4f * delta;

                // 0.5 é o raio do collider do jogador
                if (CheckCollision(playerTransform, obstacle.transform, 0.5f))
                {
                    Console.WriteLine("obstacle transform é igual ao player transform");
                }               
            }
        }
        
        private static bool CheckCollision(Transform collider1, Transform collider2, float offset)
        {
            // processing
            // ship.getX() >= powerup.getX() - 35 &&
            // ship.getX() <= powerup.getX() + 35 &&
            // ship.getY() >= powerup.getY() - 35 &&
            // ship.getY() <= powerup.getY() + 35)

            return collider1.position.X >= collider2.position.X - offset &&
                collider1.position.X <= collider2.position.X + offset &&
                collider1.position.Z >= collider2.position.Z - offset &&
                collider1.position.Z <= collider2.position.Z + offset;
        }

        private static void ResetToStartPosition()
        {
            // camera
            camera.position.Z = 8f;
            camera.position.Y = 3.5f;
            camera.rotation.X = 320f;

            // player
            playerTransform.position.Z = 5.5f;
            Game.transform2.position.Y = 1f;
        }

        private static void SpawnObstacle()
        {
            // reseta o timer
            obstacleSpawnTimer = 0f;

            // cria novo obstaculo e adiciona a lista
            obstacles.Add(new Obstacle());
        }
    }
}
