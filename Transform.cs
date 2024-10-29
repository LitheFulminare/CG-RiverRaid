using OpenTK.Mathematics;

namespace CG
{
    // Classe de transform, que agrupa as transformações de escala, rotação e
    //translação que um objeto pode sofrer.
    internal class Transform
    {
        public Vector3 position = Vector3.Zero;
        public Vector3 rotation = Vector3.Zero;
        public Vector3 scale = Vector3.One;

        public Matrix4 TranslationMatrix => Matrix4.CreateTranslation(position);
        public Matrix4 RotationMatrix =>
            Matrix4.CreateRotationX(rotation.X * (MathF.PI / 180f)) *
            Matrix4.CreateRotationY(rotation.Y * (MathF.PI / 180f)) *
            Matrix4.CreateRotationZ(rotation.Z * (MathF.PI / 180f));
        public Matrix4 ScaleMatrix => Matrix4.CreateScale(scale);
        // Matriz combinada de escala, rotação e translação
        public Matrix4 ModelMatrix => ScaleMatrix * RotationMatrix * TranslationMatrix;

        public Vector3 Right => (new Vector4(Vector3.UnitX, 1f) * RotationMatrix).Xyz;
        public Vector3 Up => (new Vector4(Vector3.UnitY, 1f) * RotationMatrix).Xyz;
        public Vector3 Forward => (new Vector4(-Vector3.UnitZ, 1f) * RotationMatrix).Xyz;
    }
}
