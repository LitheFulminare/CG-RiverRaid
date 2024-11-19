﻿using CG.Materials;
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
        // ------ Shader ------
        ShaderProgram? program;// Shader program utilizado.
        ShaderProgram? programScroll; // shader usado pra scrollar o mapa
        ShaderProgram? programScroll2; // scroll mais lento para simular parallax / profundidade

        // ------ Textura ------
        Texture? texture;
        Texture? waterTexture;
        Texture? rockTexture;

        // ------ Mesh ------
        Mesh? playerMesh;
        Mesh? mesh2;
        Mesh? topWaterMesh; // agua translucida que fica em cima
        Mesh? bottomWaterMesh;
        Mesh? obstacleMesh;
        Mesh? projectileMesh;

        // ------ Transform ------
        public static Transform playerTransform = new Transform();
        public static Transform transform2 = new Transform();
        public static Transform topWaterTransform = new Transform();
        public static Transform bottomWaterTransform = new Transform();
        public static Transform projectileTransform = new Transform();

        // ------ Material ------
        TexturedMaterial? playerMaterial;
        TexturedMaterial? material2;
        TexturedMaterial? topWaterMaterial;
        TexturedMaterial? bottomWaterMaterial;
        TexturedMaterial? obstacleMaterial;
        TexturedMaterial? projectileMaterial;

        // ------ Misc ------
        public static Camera camera = new Camera();
        
        DirectionalLight light = new DirectionalLight();

        public static Player player = new Player();

        float playerSpeed = 3f;
        float scrollingSpeed = 4f;
        float startTime = 0f; // por padrao é um double
        float totalElapsedTime;

        public static float obstacleSize = 2f;
        public static float projectileSize = 0.25f;

        public static List<Obstacle> obstacles = new List<Obstacle>();
        public static List<Shot> projectiles = new List<Shot>();
        //Random random = new Random();
        
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

            InitializeMeshes();
            InitializeShaders();
            InitializeTextures();
            InitializeMaterials();

            #region Misc
            // camera -> proporção da tela e posição
            camera.aspectRatio = (float)Size.X / Size.Y;

            light.color = new Vector3(1f, 0.8f, 0.6f);

            // Travamento do cursor do mouse para o centro da tela.
            CursorState = CursorState.Grabbed;
            #endregion Misc

            GameManager.Start();
        }

        private void InitializeMeshes()
        {
            playerMesh = Mesh.CreateSphere(0.5f);
            mesh2 = Mesh.CreateCube(1f);
            topWaterMesh = Mesh.CreatePlane(25f);
            bottomWaterMesh = Mesh.CreatePlane(35f);
            obstacleMesh = Mesh.CreateCube(obstacleSize);
            projectileMesh = Mesh.CreateSphere(projectileSize);
        }

        private void InitializeShaders()
        {
            // shaders basicos
            Shader vertexShader = Shader.CreateFromFile("./assets/shaders/shader.vert", ShaderType.VertexShader);
            Shader fragmentShader = Shader.CreateFromFile("./assets/shaders/shader.frag", ShaderType.FragmentShader);

            // shader para scroll do mapa
            Shader fragmentShaderScroll = Shader.CreateFromFile("./assets/shaders/shaderScroll.frag", ShaderType.FragmentShader);

            program = new ShaderProgram(new Shader[] { vertexShader, fragmentShader });
            program.Use();

            programScroll = new ShaderProgram(new Shader[] { vertexShader, fragmentShaderScroll });
            programScroll.Use();

            programScroll2 = new ShaderProgram(new Shader[] { vertexShader, fragmentShaderScroll });
            programScroll2.Use();
        }

        private void InitializeTextures()
        {
            texture = new Texture("./assets/textures/img.jpg");
            waterTexture = new Texture("./assets/textures/water.jpg");
            rockTexture = new Texture("./assets/textures/rock.jpg");
        }

        private void InitializeMaterials()
        {
            playerMaterial = new TexturedMaterial(program, new Vector4(1f, 0f, 0f, 1f), texture);
            material2 = new TexturedMaterial(program, new Vector4(0f, 0f, 1f, 1f), texture);
            topWaterMaterial = new TexturedMaterial(programScroll, new Vector4(1f, 1f, 1f, 0.5f), waterTexture);
            bottomWaterMaterial = new TexturedMaterial(programScroll2, new Vector4(0.2f, 0.5f, 0.75f, 1f), waterTexture);
            obstacleMaterial = new TexturedMaterial(program, new Vector4(1f, 1f, 1f, 1f), rockTexture);
            projectileMaterial = new TexturedMaterial(program, new Vector4(1f, 0f, 0f, 1f), waterTexture);
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
            if (KeyboardState.IsKeyDown(Keys.Right) || KeyboardState.IsKeyDown(Keys.D))
            {
                player.Move(1, delta);
                //playerTransform.position.X += playerSpeed * delta;
            }
            if (KeyboardState.IsKeyDown(Keys.Left) || KeyboardState.IsKeyDown(Keys.A))
            {
                player.Move(-1, delta);
                //playerTransform.position.X -= playerSpeed * delta;
            }
            if (KeyboardState.IsKeyPressed(Keys.Space) && !player.IsDead)
            {
                projectiles.Add(player.Shoot());
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

            programScroll2?.ApplyDirectionalLight(light);
            programScroll2?.SetUniform("u_AmbientLight", new Vector3(0.1f, 0.1f, 0.2f));

            // parametros para scrollar o mar
            programScroll?.SetUniform("time", totalElapsedTime);
            programScroll?.SetUniform("speed", scrollingSpeed / 10);

            program?.ApplyDirectionalLight(light);
            program?.SetUniform("u_AmbientLight", new Vector3(0.1f, 0.1f, 0.2f));

            programScroll?.ApplyDirectionalLight(light);
            programScroll?.SetUniform("u_AmbientLight", new Vector3(0.1f, 0.1f, 0.2f));

            // parametros para scrollar o mar
            programScroll2?.SetUniform("time", totalElapsedTime);
            programScroll2?.SetUniform("speed", scrollingSpeed / 20f);

            // Envio das matrizes de câmera para o shader program.
            program?.ApplyCamera(camera);
            programScroll?.ApplyCamera(camera);
            programScroll2?.ApplyCamera(camera);

            // Desenho do primeiro transform
            playerMaterial?.Use();
            if (!player.IsDead)
            {
                program?.ApplyTransform(player.Transform);
                playerMesh?.Draw();
            }            

            // Desenho do segundo transform
            material2?.Use();
            program?.ApplyTransform(transform2);
            mesh2?.Draw();

            // obstaculos
            obstacleMaterial?.Use();
            foreach (var obstacle in obstacles)
            {
                program?.ApplyTransform(obstacle.transform);
                obstacleMesh?.Draw();
            }

            // projeteis
            projectileMaterial?.Use();
            foreach (var projectile in projectiles)
            {
                program ?.ApplyTransform(projectile.Transform);
                projectileMesh?.Draw();
            }

            // mapa
            bottomWaterMaterial?.Use();
            programScroll2?.ApplyTransform(bottomWaterTransform);
            bottomWaterMesh?.Draw();

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            topWaterMaterial?.Use();
            programScroll?.ApplyTransform(topWaterTransform);
            topWaterMesh?.Draw();

            SwapBuffers();
        }
    }
}
