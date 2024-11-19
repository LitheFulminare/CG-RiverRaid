using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG
{
    internal class Collider // ia ser usado pra simplificar os argumentos da função CheckCollision, mas foi descartada por causa de tempo e complexidade
    {
        private OpenTK.Mathematics.Vector3 _position;
        private float _size;

        public Collider(OpenTK.Mathematics.Vector3 position, float size)
        {
            _position = position;
            _size = size;
        }
    }
}
