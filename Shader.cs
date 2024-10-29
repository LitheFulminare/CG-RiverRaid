using OpenTK.Graphics.OpenGL;

namespace CG
{
    // Classe que representa uma etapa de um shader program, seja Vertex, Fragment, ou outra
    internal class Shader
    {
        private int _id;
        public int Id => _id;

        public static Shader CreateFromString(string source, ShaderType type)
        {
            Shader shader = new Shader();

            shader._id = GL.CreateShader(type);
            GL.ShaderSource(shader._id, source);
            GL.CompileShader(shader._id);

            GL.GetShader(shader._id, ShaderParameter.CompileStatus, out int success);
            if(success != (int)All.True)
            {
                throw new Exception($"Erro de compilação do shader: {GL.GetShaderInfoLog(shader._id)}");
            }

            return shader;
        }

        public static Shader CreateFromFile(string path, ShaderType type)
        {
            return CreateFromString(File.ReadAllText(path), type);
        }
    }
}
