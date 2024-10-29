using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace CG
{
    // Classe de malha, que envia os dados de vértices e índices para a placa de
    //vídeo, permitindo o seu desenho.
    internal class Mesh
    {
        private int _vbo;
        private int _ebo;
        private int _vao;
        private int _elementCount;

        public Mesh(float[] vertices, uint[] indices)
        {
            _vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * vertices.Length, vertices, BufferUsageHint.StaticDraw);

            _vao = GL.GenVertexArray();
            GL.BindVertexArray(_vao);
            
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, sizeof(float) * 8, 0);
            
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, sizeof(float) * 8, sizeof(float) * 3);
            
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, sizeof(float) * 8, sizeof(float) * 6);

            _ebo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(uint) * indices.Length, indices, BufferUsageHint.StaticDraw);

            _elementCount = indices.Length;
        }

        public void Draw()
        {
            GL.BindVertexArray(_vao);
            GL.DrawElements(BeginMode.Triangles, _elementCount, DrawElementsType.UnsignedInt, 0);
        }

        public static Mesh CreatePlane(float size)
        {
            float halfSize = size / 2f;
            float[] vertices =
            [
                //posição                  //cor              //uv
                -halfSize, 0f,  halfSize,  1.0f, 0.0f, 0.0f,  0.0f, 0.0f,//0
                 halfSize, 0f,  halfSize,  0.0f, 1.0f, 0.0f,  1.0f, 0.0f,//1
                 halfSize, 0f, -halfSize,  0.0f, 0.0f, 1.0f,  1.0f, 1.0f,//2
                -halfSize, 0f, -halfSize,  1.0f, 1.0f, 0.0f,  0.0f, 1.0f,//3
            ];
            uint[] indices =
            [
                0, 1, 2,
                0, 2, 3,
            ];
            return new Mesh(vertices, indices);
        }

        public static Mesh CreatePost()
        {
            float[] vertices =
            {
                //posição       //normal     //uv
                -1f, -1f,  0f,  0f, 0f, 1f,  0f, 0f,
                 1f, -1f,  0f,  0f, 0f, 1f,  1f, 0f,
                 1f,  1f,  0f,  0f, 0f, 1f,  1f, 1f,
                -1f,  1f,  0f,  0f, 0f, 1f,  0f, 1f,
            };

            uint[] indices =
            {
                0, 1, 2,
                0, 2, 3,
            };

            return new Mesh(vertices, indices);
        }

        public static Mesh CreateCube(float size)
        {
            float halfSize = size / 2f;
            float[] vertices =
            [
                //posição                         //normal           //uv
                -halfSize, -halfSize,  halfSize,  0.0f, 0.0f, 1.0f,  0.0f, 0.0f,//0
                 halfSize, -halfSize,  halfSize,  0.0f, 0.0f, 1.0f,  1.0f, 0.0f,//1
                 halfSize,  halfSize,  halfSize,  0.0f, 0.0f, 1.0f,  1.0f, 1.0f,//2
                -halfSize,  halfSize,  halfSize,  0.0f, 0.0f, 1.0f,  0.0f, 1.0f,//3

                 halfSize, -halfSize, -halfSize,  0.0f, 0.0f, -1.0f,  0.0f, 0.0f,//4
                -halfSize, -halfSize, -halfSize,  0.0f, 0.0f, -1.0f,  1.0f, 0.0f,//5
                -halfSize,  halfSize, -halfSize,  0.0f, 0.0f, -1.0f,  1.0f, 1.0f,//6
                 halfSize,  halfSize, -halfSize,  0.0f, 0.0f, -1.0f,  0.0f, 1.0f,//7

                 halfSize, -halfSize,  halfSize,  1.0f, 0.0f, 0.0f,  0.0f, 0.0f,//8
                 halfSize, -halfSize, -halfSize,  1.0f, 0.0f, 0.0f,  1.0f, 0.0f,//9
                 halfSize,  halfSize, -halfSize,  1.0f, 0.0f, 0.0f,  1.0f, 1.0f,//10
                 halfSize,  halfSize,  halfSize,  1.0f, 0.0f, 0.0f,  0.0f, 1.0f,//11

                -halfSize, -halfSize, -halfSize,  -1.0f, 0.0f, 0.0f,  0.0f, 0.0f,//12
                -halfSize, -halfSize,  halfSize,  -1.0f, 0.0f, 0.0f,  1.0f, 0.0f,//13
                -halfSize,  halfSize,  halfSize,  -1.0f, 0.0f, 0.0f,  1.0f, 1.0f,//14
                -halfSize,  halfSize, -halfSize,  -1.0f, 0.0f, 0.0f,  0.0f, 1.0f,//15

                -halfSize,  halfSize,  halfSize,  0.0f, 1.0f, 0.0f,  0.0f, 0.0f,//16
                 halfSize,  halfSize,  halfSize,  0.0f, 1.0f, 0.0f,  1.0f, 0.0f,//17
                 halfSize,  halfSize, -halfSize,  0.0f, 1.0f, 0.0f,  1.0f, 1.0f,//18
                -halfSize,  halfSize, -halfSize,  0.0f, 1.0f, 0.0f,  0.0f, 1.0f,//19

                -halfSize, -halfSize, -halfSize,  0.0f, -1.0f, 0.0f,  0.0f, 0.0f,//20
                 halfSize, -halfSize, -halfSize,  0.0f, -1.0f, 0.0f,  1.0f, 0.0f,//21
                 halfSize, -halfSize,  halfSize,  0.0f, -1.0f, 0.0f,  1.0f, 1.0f,//22
                -halfSize, -halfSize,  halfSize,  0.0f, -1.0f, 0.0f,  0.0f, 1.0f,//23
            ];
            uint[] indices =
            [
                0, 1, 2,
                0, 2, 3,

                4, 5, 6,
                4, 6, 7,

                8, 9, 10,
                8, 10, 11,

                12, 13, 14,
                12, 14, 15,

                16, 17, 18,
                16, 18, 19,

                20, 21, 22,
                20, 22, 23,
            ];
            return new Mesh(vertices, indices);
        }

        public static Mesh CreateCylinder(float radius = 0.5f, float height = 1f, uint subdivisions = 16)
        {
            List<float> vertices = new List<float>();

            float halfHeight = height / 2;
            for(uint i = 0; i <= subdivisions; i++)
            {
                float value = (float)i / subdivisions;
                float angle = value * MathF.Tau;

                float cos = MathF.Cos(angle);
                float sin = -MathF.Sin(angle);

                float x = cos * radius;
                float z = sin * radius;

                // 0 - Vértice do topo(lateral)
                vertices.AddRange(new float[] { x, halfHeight, z });// Posição
                vertices.AddRange(new float[] { cos, 0f, sin });// Cor/Normal
                vertices.AddRange(new float[] { value, 1f });// UV

                // 1 - Vértice de baixo(lateral)
                vertices.AddRange(new float[] { x, -halfHeight, z });// Posição
                vertices.AddRange(new float[] { cos, 0f, sin });// Cor/Normal
                vertices.AddRange(new float[] { value, 0f });// UV

                // 2 - Vértice do topo(cap superior)
                vertices.AddRange(new float[] { x, halfHeight, z });// Posição
                vertices.AddRange(new float[] { 0f, 1f, 0f });// Cor/Normal
                vertices.AddRange(new float[] { (cos + 1f) * 0.5f, -(sin + 1f) * 0.5f });// UV

                // 3 - Vértice de baixo(cap inferior)
                vertices.AddRange(new float[] { x, -halfHeight, z });// Posição
                vertices.AddRange(new float[] { 0f, -1f, 0f });// Cor/Normal
                vertices.AddRange(new float[] { (cos + 1f) * 0.5f, (sin + 1f) * 0.5f });// UV
            }

            List<uint> indices = new List<uint>();
            // Faces laterais
            for (uint i = 0; i < subdivisions; i++)
            {
                uint i0 = i * 4;
                uint i1 = i * 4 + 1;
                uint i2 = (i + 1) * 4;
                uint i3 = (i + 1) * 4 + 1;

                indices.AddRange(new uint[] { i0, i1, i3 });
                indices.AddRange(new uint[] { i0, i3, i2 });
            }

            // Faces do topo
            for (uint i = 0; i < subdivisions; i++)
            {
                uint i0 = 0 + 2;
                uint i1 = (i + 1) * 4 + 2;
                uint i2 = (i + 2) * 4 + 2;

                indices.AddRange(new uint[] { i0, i1, i2 });
            }

            // Faces de baixo
            for (uint i = 0; i < subdivisions; i++)
            {
                uint i0 = 0 + 3;
                uint i1 = (i + 1) * 4 + 3;
                uint i2 = (i + 2) * 4 + 3;

                indices.AddRange(new uint[] { i0, i2, i1 });
            }

            return new Mesh(vertices.ToArray(), indices.ToArray());
        }

        public static Mesh CreateSphere(float radius = 0.5f, uint segments = 16, uint rings = 8)
        {
            List<float> vertices = new List<float>();

            // Vértices dos anéis da esfera
            for (int i = 0; i <= rings; i++)
            {
                float valueV = (float)i / rings;
                float cosV = MathF.Cos(valueV * MathF.PI);
                float sinV = MathF.Sin(valueV * MathF.PI);

                for (int j = 0; j <= segments; j++)
                {
                    float valueH = (float)j / segments;
                    float cosH = MathF.Cos(valueH * MathF.Tau) * sinV;
                    float sinH = -MathF.Sin(valueH * MathF.Tau) * sinV;

                    Vector3 normal = new Vector3(cosH, cosV, sinH);
                    Vector3 position = normal * radius;

                    vertices.AddRange(new float[] { position.X, position.Y, position.Z });// Posição
                    vertices.AddRange(new float[] { normal.X, normal.Y, normal.Z });// Cor/Normal
                    vertices.AddRange(new float[] { valueH, 1f - valueV });// UV
                }
            }

            List<uint> indices = new List<uint>();
            for (uint i = 0; i < rings; i++)
            {
                for (uint j = 0; j < segments; j++)
                {
                    uint i0 = i * (segments + 1) + j;
                    uint i1 = i0 + 1;
                    uint i2 = (i + 1) * (segments + 1) + j;
                    uint i3 = i2 + 1;

                    indices.AddRange(new uint[] { i0, i2, i3 });
                    indices.AddRange(new uint[] { i0, i3, i1 });
                }
            }

            return new Mesh(vertices.ToArray(), indices.ToArray());
        }
    }
}
