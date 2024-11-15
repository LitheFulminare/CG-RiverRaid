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
        ShaderProgram? programScroll; // shader usado pra scrollar o mapa

        Texture? texture;

        Mesh? playerMesh;
        Mesh? mesh2;
        Mesh? mapMesh;
        Mesh? obstacleMesh;

        public static Transform playerTransform = new Transform();
        public static Transform transform2 = new Transform();
        public static Transform mapTransform = new Transform();
        
        TexturedMaterial? playerMaterial;
        TexturedMaterial? material2;
        TexturedMaterial? mapMaterial;
        TexturedMaterial? obstacleMaterial;

        public static Camera camera = new Camera();
        
        DirectionalLight light = new DirectionalLight();

        public static Player player = new Player();

        float playerSpeed = 3f;
        float scrollingSpeed = 4f;
        float startTime = 0f; // por padrao é um double
        float totalElapsedTime;

        public static List<Obstacle> obstacles = new List<Obstacle>();
        Random random = new Random();
        
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

            playerMesh = Mesh.CreateSphere(0.5f);
            mesh2 = Mesh.CreateCube(1f);

            mapMesh = Mesh.CreatePlane(25f);

            // shaders basicos
            Shader vertexShader = Shader.CreateFromFile("./assets/shaders/shader.vert", ShaderType.VertexShader);
            Shader fragmentShader = Shader.CreateFromFile("./assets/shaders/shader.frag", ShaderType.FragmentShader);

            // shader para scroll do mapa
            Shader fragmentShaderScroll = Shader.CreateFromFile("./assets/shaders/shaderScroll.frag", ShaderType.FragmentShader);

            program = new ShaderProgram(new Shader[] { vertexShader, fragmentShader });
            program.Use();

            programScroll = new ShaderProgram(new Shader[] { vertexShader, fragmentShaderScroll });
            programScroll.Use();

            texture = new Texture("./assets/textures/img.jpg");

            playerMaterial = new TexturedMaterial(program, new Vector3(1f, 0f, 0f), texture);
            material2 = new TexturedMaterial(program, new Vector3(0f, 0f, 1f), texture);
            mapMaterial = new TexturedMaterial(programScroll, new Vector3(0f, 0.7f, 1f), texture);

            obstacleMesh = Mesh.CreateCube(0.5f); // Example shape; adjust as needed
            obstacleMaterial = new TexturedMaterial(program, new Vector3(0.7f, 0f, 0f), texture);

            // camera -> proporção da tela e posição
            camera.aspectRatio = (float)Size.X / Size.Y;

            light.color = new Vector3(1f, 0.8f, 0.6f);

            // Travamento do cursor do mouse para o centro da tela.
            CursorState = CursorState.Grabbed;

            GameManager.Start();
        }

        // Função de atualização lógica, chamada múltiplas vezes por segundo em
        //um intervalo pré-definido. É nesta função que vai ficar boa parte da
        //lógica de mundo e jogo do projeto.
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            // O delta representa o tempo passado entre frames.
            float delta = (float)args.Time;
            totalElapsedTime += delta;

            GameManager.Update(delta);

            // Modificação do componente X do offset quando pressionadas teclas
            //para a esquerda ou para a direita.
            if (KeyboardState.IsKeyDown(Keys.Right))
            {
                player.Move(1, delta);
                //playerTransform.position.X += playerSpeed * delta;
            }
            if (KeyboardState.IsKeyDown(Keys.Left))
            {
                player.Move(-1, delta);
                //playerTransform.position.X -= playerSpeed * delta;
            }
            if (KeyboardState.IsKeyDown(Keys.Space))
            {
                for (int i = 0; i < obstacles.Count; i++)
                {
                    //obstacles.RemoveAt(i);
                }
            }
        }

        // Função de atualização visual, chamada múltiplas vezes por segundo em
        //um intervalo pré-definido que pode variar dependendo de configurações 
        //de VSync, por exemplo.
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            // a quanto tempo a aplicação tá rodando
            // não pode ser delta pq no geral ele é constante, e se o parametro não mudar a textura também não muda
            //Console.WriteLine($"Time: {totalElapsedTime}");
            
            GL.ClearColor(0f, 0f, 0f, 1f);            
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Envio das informações da luz
            program?.ApplyDirectionalLight(light);
            program?.SetUniform("u_AmbientLight", new Vector3(0.1f, 0.1f, 0.2f));

            programScroll?.ApplyDirectionalLight(light);
            programScroll?.SetUniform("u_AmbientLight", new Vector3(0.1f, 0.1f, 0.2f));

            // parametros para scrollar o mar
            programScroll?.SetUniform("time", totalElapsedTime);
            programScroll?.SetUniform("speed", scrollingSpeed / 10);

            // Envio das matrizes de câmera para o shader program.
            program?.ApplyCamera(camera);
            programScroll?.ApplyCamera(camera);

            // Desenho do primeiro transform
            playerMaterial?.Use();
            program?.ApplyTransform(player.Transform);
            playerMesh?.Draw();

            // Desenho do segundo transform
            material2?.Use();
            program?.ApplyTransform(transform2);
            mesh2?.Draw();

            // terceiro transform
            mapMaterial?.Use();
            programScroll?.ApplyTransform(mapTransform);
            mapMesh?.Draw();

            // Render obstacles
            obstacleMaterial?.Use();
            foreach (var obstacle in obstacles)
            {
                program?.ApplyTransform(obstacle.transform);
                obstacleMesh?.Draw();
            }

            SwapBuffers();
        }
    }
}
