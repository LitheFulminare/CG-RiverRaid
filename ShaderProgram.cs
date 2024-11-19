using OpenTK.Compute.OpenCL;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace CG
{
    // Classe que junta vários shaders em um único programa.
    internal class ShaderProgram
    {
        private int _id;

        public ShaderProgram(Shader[] shaders)
        {
            _id = GL.CreateProgram();
            foreach (Shader shader in shaders)
            {
                GL.AttachShader(_id, shader.Id);
            }
            GL.LinkProgram(_id);

            GL.GetProgram(_id, GetProgramParameterName.LinkStatus, out int success);
            if (success != (int)All.True)
            {
                throw new Exception($"Erro de link no shader program: {GL.GetProgramInfoLog(_id)}");
            }

            foreach (Shader shader in shaders)
            {
                GL.DeleteShader(shader.Id);
            }
        }

        public void Use()
        {
            GL.UseProgram(_id);
        }

        public void SetUniform(string name, int value)
        {
            int location = GL.GetUniformLocation(_id, name);
            Use();
            GL.Uniform1(location, value);
        }

        public void SetUniform(string name, float value)
        {
            int location = GL.GetUniformLocation(_id, name);
            Use();
            GL.Uniform1(location, value);
        }

        public void SetUniform(string name, Vector2 value)
        {
            int location = GL.GetUniformLocation(_id, name);
            Use();
            GL.Uniform2(location, value);
        }

        public void SetUniform(string name, Vector3 value)
        {
            int location = GL.GetUniformLocation(_id, name);
            Use();
            GL.Uniform3(location, value);
        }

        public void SetUniform(string name, Vector4 value)
        {
            int location = GL.GetUniformLocation(_id, name);
            Use();
            GL.Uniform4(location, value);
        }

        public void SetUniform(string name, Matrix4 value)
        {
            int location = GL.GetUniformLocation(_id, name);
            Use();
            GL.UniformMatrix4(location, false, ref value);
        }

        public void ApplyCamera(Camera camera)
        {
            SetUniform("u_View", camera.ViewMatrix);
            SetUniform("u_Projection", camera.ProjectionMatrix);
        }

        public void ApplyDirectionalLight(DirectionalLight light)
        {
            SetUniform("u_LightDirection", light.Direction);
            SetUniform("u_LightColor", light.color);
        }

        public void ApplyTransform(Transform transform)
        {
            SetUniform("u_Model", transform.ModelMatrix);
            SetUniform("u_Rotation", transform.RotationMatrix);
        }
    }
}
