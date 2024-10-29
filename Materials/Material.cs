namespace CG.Materials
{
    internal class Material
    {
        private ShaderProgram _program;

        public ShaderProgram Program => _program;

        public Material(ShaderProgram program)
        {
            _program = program;
        }

        public void Use()
        {
            _program.Use();
            InternalUse();
        }

        protected virtual void InternalUse() { }
    }
}
