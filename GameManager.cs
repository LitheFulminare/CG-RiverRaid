using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG
{
    internal static class GameManager
    {
        static Camera camera = Game.camera;
        static Transform playerTransform = Game.playerTransform;

        public static void Start()
        {
            ResetToStartPosition();
        }

        public static void Update(float delta)
        {

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
    }
}
