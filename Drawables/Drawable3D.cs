using CG.Materials;
using System;

namespace CG.Drawables
{
    internal class Drawable3D : Drawable
    {
        public Material material;
        private Transform _transform = new Transform();
        private Mesh _mesh;

        public override Material? Material => material;
        public Transform Transform => _transform;

        public Drawable3D(Material material, Mesh mesh) 
        {
            this.material = material;
            _mesh = mesh;
        }

        public override void Draw()
        {
            material.Use();
            material.Program.ApplyTransform(_transform);
            _mesh.Draw();
        }
    }
}
