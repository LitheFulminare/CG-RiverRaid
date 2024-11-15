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
        static Player player = Game.player;
        static List<Obstacle> obstacles = Game.obstacles;

        // componentes proprios do GameManager
        static float obstacleSpawnTimer = 0f; // reseta sempre que um obstaculo spawna
        static float obstacleSpawnRate = 3f; // frequencia de spawn, maior frequencia gera mais obstaculos

        static float obstacleSpawnInterval;

        public static void Start()
        {
            // transforma frequencia em intervalo (é mais intuivo assim)
            obstacleSpawnInterval = 1 / obstacleSpawnRate; 

            ResetMap();
        }

        public static void Update(float delta)
        {
            // player
            player.Rotate(9f, delta);
            player.Update(delta);

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
                if (CheckCollision(player.Transform, obstacle.transform, 0.5f))
                {
                    Console.WriteLine("Colisão entre obstáculo e player");
                }               
            }
        }
        
        private static bool CheckCollision(Transform collider1, Transform collider2, float offset)
        {
            /*
            diretamente do processing
             ship.getX() >= powerup.getX() - 35 &&
             ship.getX() <= powerup.getX() + 35 &&
             ship.getY() >= powerup.getY() - 35 &&
             ship.getY() <= powerup.getY() + 35)
            */

            return collider1.position.X >= collider2.position.X - offset &&
                collider1.position.X <= collider2.position.X + offset &&
                collider1.position.Z >= collider2.position.Z - offset &&
                collider1.position.Z <= collider2.position.Z + offset;
        }

        private static void ResetMap()
        {
            // camera
            camera.position.Z = 8f;
            camera.position.Y = 3.5f;
            camera.rotation.X = 320f;

            // player
            player.ResetPostion();
            Game.transform2.position.Y = 1f;

            // map
            obstacles.Clear();
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
