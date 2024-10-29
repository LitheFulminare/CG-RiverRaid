using CG.Materials;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace CG
{
    // Classe Game. Representa a janela de jogo e, herdando de GameWindow, tem
    //várias funções úteis para a execução da inicialização e loop de jogo.
    // Nota-se que 
    internal class Game : GameWindow
    {
        ShaderProgram? program;// Shader program utilizado.

        Texture? texture;

        Mesh? mesh;
        Mesh? mesh2;
        Mesh? mesh3;

        Transform transform = new Transform();
        Transform transform2 = new Transform();
        Transform transform3 = new Transform();
        
        TexturedMaterial? material1;
        TexturedMaterial? material2;
        TexturedMaterial? material3;
        
        Camera camera = new Camera();
        
        DirectionalLight light = new DirectionalLight();

        // Construtor base da classe. Por simplicidade, recebe apenas um título
        //e dimensões de altura e largura da janela que será aberta.
        public Game(string title, int width, int height) : base(GameWindowSettings.Default, new NativeWindowSettings() { Title = title, ClientSize = (width, height) })
        {
        }

        // Função que roda uma única vez quando a janela é criada, todo código
        //de inicialização de funcionalidades da OpenGL deve estar contido nela
        //para correto funcionamento.
        protected override void OnLoad()
        {
            base.OnLoad();

            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.DepthTest);

            GL.CullFace(CullFaceMode.Back);

            mesh = Mesh.CreateSphere(0.5f);
            mesh2 = Mesh.CreateCube(1f);

            mesh3 = Mesh.CreatePlane(25f);

            Shader vertexShader = Shader.CreateFromFile("./assets/shaders/shader.vert", ShaderType.VertexShader);
            Shader fragmentShader = Shader.CreateFromFile("./assets/shaders/shader.frag", ShaderType.FragmentShader);

            program = new ShaderProgram(new Shader[] { vertexShader, fragmentShader });
            program.Use();

            texture = new Texture("./assets/textures/img.jpg");

            material1 = new TexturedMaterial(program, new Vector3(1f, 0f, 0f), texture);
            material2 = new TexturedMaterial(program, new Vector3(0f, 0f, 1f), texture);
            material3 = new TexturedMaterial(program, new Vector3(0f, 1f, 0f), texture);

            camera.aspectRatio = (float)Size.X / Size.Y;
            camera.position.Z = 8f;
            camera.position.Y = 3.5f;
            camera.rotation.X = 320f;

            transform2.position.Y = 1f;

            light.color = new Vector3(1f, 0.8f, 0.6f);

            // Travamento do cursor do mouse para o centro da tela.
            CursorState = CursorState.Grabbed;
        }

        // Função de atualização lógica, chamada múltiplas vezes por segundo em
        //um intervalo pré-definido. É nesta função que vai ficar boa parte da
        //lógica de mundo e jogo do projeto.
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            // O delta representa o tempo passado entre frames.
            float delta = (float)args.Time;

            transform.rotation.Y += delta * 9f;

            // Modificação do componente X do offset quando pressionadas teclas
            //para a esquerda ou para a direita.
            if (KeyboardState.IsKeyDown(Keys.Right))
            {
                transform.position.X += 1f * delta;
            }
            if (KeyboardState.IsKeyDown(Keys.Left))
            {
                transform.position.X -= 1f * delta;
            }
            if (KeyboardState.IsKeyDown(Keys.Space))
            {
                transform.position += transform.Forward * delta;
            }

            // Movimento da câmera
            if (KeyboardState.IsKeyDown(Keys.D))
            {
                camera.position += camera.Right * delta;
            }
            if (KeyboardState.IsKeyDown(Keys.A))
            {
                camera.position -= camera.Right * delta;
            }
            if (KeyboardState.IsKeyDown(Keys.W))
            {
                camera.position += camera.Forward * delta;
            }
            if (KeyboardState.IsKeyDown(Keys.S))
            {
                camera.position -= camera.Forward * delta;
            }
            if (KeyboardState.IsKeyDown(Keys.Q))
            {
                camera.position -= camera.Up * delta;
            }
            if (KeyboardState.IsKeyDown(Keys.E))
            {
                camera.position += camera.Up * delta;
            }

            // Scroll do mapa
            // map.position.X += map.Forward * delta;

            // Rotação da câmera
            camera.rotation.Y -= MouseState.Delta.X * 0.1f;
            camera.rotation.X -= MouseState.Delta.Y * 0.1f;
        }

        // Função de atualização visual, chamada múltiplas vezes por segundo em
        //um intervalo pré-definido que pode variar dependendo de configurações 
        //de VSync, por exemplo.
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.ClearColor(0f, 0f, 0f, 1f);            
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Envio das informações da luz
            program?.ApplyDirectionalLight(light);
            program?.SetUniform("u_AmbientLight", new Vector3(0.1f, 0.1f, 0.2f));

            // Envio das matrizes de câmera para o shader program.
            program?.ApplyCamera(camera);

            // Desenho do primeiro transform
            material1?.Use();
            program?.ApplyTransform(transform);
            mesh?.Draw();

            // Desenho do segundo transform
            material2?.Use();
            program?.ApplyTransform(transform2);
            mesh2?.Draw();

            // terceiro transform
            material3?.Use();
            program?.ApplyTransform(transform3);
            mesh3?.Draw();

            SwapBuffers();
        }
    }
}
