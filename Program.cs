using CG;

// Aqui começa o programa. É criada uma instância da classe Game, representando
//a janela. Em seguida, é chamada a função Run, que vai executar o loop de jogo
//até que a janela seja fechada.
//É dentro da classe Game que toda a lógica de jogo ficará situada.
Game game = new Game("Meu Jogo", 800, 600);
game.Run();
