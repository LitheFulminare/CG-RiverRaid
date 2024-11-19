using OpenTK.Mathematics;

namespace CG.Materials
{
    internal class TexturedMaterial : Material
    {
        public Vector4 color;
        public Texture? texture;

        public TexturedMaterial(ShaderProgram program, Vector4 color, Texture? texture = null) : base(program)
        {
            this.color = color;
            this.texture = texture;
        }

        protected override void InternalUse()
        {
            texture?.Use(0);
            Program.SetUniform("u_Texture", 0);
            Program.SetUniform("u_Color", color);
        }
    }
}
