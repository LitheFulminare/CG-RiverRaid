using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG
{
    internal class Obstacle
    {
        public bool isDestroyed = false;
        public Transform transform;
        
        public Obstacle()
        {
            transform = new Transform();
            OnSpawn();
        }

        public void OnSpawn()
        {
            Random random = new Random();           
            transform.position.X = random.Next(-5, 5);
            transform.position.Z = -13f;
        }
    }
}
