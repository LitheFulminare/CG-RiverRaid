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

        Transform playerTransform = new Transform();
        Transform transform2 = new Transform();
        Transform mapTransform = new Transform();
        
        TexturedMaterial? playerMaterial;
        TexturedMaterial? material2;
        TexturedMaterial? mapMaterial;
        
        Camera camera = new Camera();
        
        DirectionalLight light = new DirectionalLight();

        float playerSpeed = 3f;
        float startTime = (float)GLFW.GetTime();

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

            // camera -> proporção da tela e posição
            camera.aspectRatio = (float)Size.X / Size.Y;
            camera.position.Z = 8f;
            camera.position.Y = 3.5f;
            camera.rotation.X = 320f;

            // posição do player
            playerTransform.position.Z = 5.5f;

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

            playerTransform.rotation.Y += delta * 9f;

            // Modificação do componente X do offset quando pressionadas teclas
            //para a esquerda ou para a direita.
            if (KeyboardState.IsKeyDown(Keys.Right))
            {
                playerTransform.position.X += playerSpeed * delta;
            }
            if (KeyboardState.IsKeyDown(Keys.Left))
            {
                playerTransform.position.X -= playerSpeed * delta;
            }
            if (KeyboardState.IsKeyDown(Keys.Space))
            {
                //transform.position += transform.Forward * delta;
            }

            /*
            // Movimento da câmera -> fica estática, será o movimento do player
            if (KeyboardState.IsKeyDown(Keys.D))
            {
                //player.position += player.Right* delta;

                //camera.position += camera.Right * delta;
            }
            if (KeyboardState.IsKeyDown(Keys.A))
            {
                //camera.position -= camera.Right * delta;
            }
            if (KeyboardState.IsKeyDown(Keys.W))
            {
                //camera.position += camera.Forward * delta;
            }
            if (KeyboardState.IsKeyDown(Keys.S))
            {
                //camera.position -= camera.Forward * delta;
            }
            if (KeyboardState.IsKeyDown(Keys.Q))
            {
                //camera.position -= camera.Up * delta;
            }
            if (KeyboardState.IsKeyDown(Keys.E))
            {
                //camera.position += camera.Up * delta;
            }

            // Scroll do mapa -> não mudará a posição,tudo será feito por shader
            //mapTransform.position -= mapTransform.Forward * delta;

            // Rotação da câmera
            //camera.rotation.Y -= MouseState.Delta.X * 0.1f;
            //camera.rotation.X -= MouseState.Delta.Y * 0.1f;

            */
        }

        // Função de atualização visual, chamada múltiplas vezes por segundo em
        //um intervalo pré-definido que pode variar dependendo de configurações 
        //de VSync, por exemplo.
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            //float delta = (float)args.Time;
            
            // a quanto tempo a aplicação tá rodando
            // não pode ser delta pq no geral ele é constante, e se o parametro não mudar a textura também não muda
            float time = (float)GLFW.GetTime() - startTime;
            Console.WriteLine($"Time: {time}");

            base.OnRenderFrame(args);
            GL.ClearColor(0f, 0f, 0f, 1f);            
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Envio das informações da luz
            program?.ApplyDirectionalLight(light);
            program?.SetUniform("u_AmbientLight", new Vector3(0.1f, 0.1f, 0.2f));

            programScroll?.ApplyDirectionalLight(light);
            programScroll?.SetUniform("u_AmbientLight", new Vector3(0.1f, 0.1f, 0.2f));

            // parametros para scrollar o mar
            programScroll?.SetUniform("time", time);

            // Envio das matrizes de câmera para o shader program.
            program?.ApplyCamera(camera);
            programScroll?.ApplyCamera(camera);

            // Desenho do primeiro transform
            playerMaterial?.Use();
            program?.ApplyTransform(playerTransform);
            playerMesh?.Draw();

            // Desenho do segundo transform
            material2?.Use();
            program?.ApplyTransform(transform2);
            mesh2?.Draw();

            // terceiro transform
            mapMaterial?.Use();
            programScroll?.ApplyTransform(mapTransform);
            mapMesh?.Draw();

            SwapBuffers();
        }
    }
}
