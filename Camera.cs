using OpenTK.Mathematics;

namespace CG
{
    // Classe de câmera. Herda de Transform, utilizando informações de rotação
    //e posição para calcular as matrizes de transformação de tela.
    internal class Camera : Transform
    {
        public float fieldOfView = 60f;// Campo de visão da câmera, em graus
        public float aspectRatio = 1f;// Proporção largura/altura da tela
        public float nearPlane = 0.3f;// plano próximo da câmera, pixels antes desse ponto serão ignorados
        public float farPlane = 500f;// plano distante da câmera, pixels após esse ponto serão ignorados

        // Matriz de visão. "Gira" o mundo em relação à câmera.
        public Matrix4 ViewMatrix => Matrix4.LookAt(position, position + Forward, Up);
        // Matriz de projeção. "Comprime" o mundo para que fique dentro do frustum da câmera.
        public Matrix4 ProjectionMatrix => Matrix4.CreatePerspectiveFieldOfView(fieldOfView * (MathF.PI / 180f), aspectRatio, nearPlane, farPlane);
    }
}
