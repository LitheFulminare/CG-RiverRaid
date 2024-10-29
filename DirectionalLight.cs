using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG
{
    internal class DirectionalLight : Transform
    {
        public Vector3 Direction => Forward;
        public Vector3 color = Vector3.One;
    }
}
