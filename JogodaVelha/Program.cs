using System;

public class JogoDaVelha
{
    char[,] tabuleiro = new char[3, 3];

    public JogoDaVelha()
    {
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                tabuleiro[i, j] = ' ';
    }

    public void ExibirTabuleiro()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Console.Write(tabuleiro[i, j] == ' ' ? "-" : tabuleiro[i, j]);
                if (j < 2) Console.Write("|");
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    private int AvaliarPosicao(int linha, int coluna, char jogador)
    {
        if (tabuleiro[linha, coluna] != ' ') return int.MinValue; // Posição já ocupada

        int pontuacao = 0;
        char adversario = jogador == 'X' ? 'O' : 'X';

        // Pontos para posições centrais e cantos
        if (linha == 1 && coluna == 1)
            pontuacao += 2;
        if ((linha == 0 || linha == 2) && (coluna == 0 || coluna == 2))
            pontuacao += 1;

        // Pontuação negativa se existem peças do adversário na mesma linha, coluna ou diagonal
        if (LinhaColunaDiagonalComAdversario(linha, coluna, adversario))
            pontuacao -= 2;

        // Pontos adicionais para bloquear ou alcançar vitória
        if (BloquearOuVencer(linha, coluna, adversario))
            pontuacao += 4;
        if (BloquearOuVencer(linha, coluna, jogador))
            pontuacao += 4;

        return pontuacao;
    }

    private bool LinhaColunaDiagonalComAdversario(int linha, int coluna, char adversario)
    {
        for (int i = 0; i < 3; i++)
        {
            if (tabuleiro[linha, i] == adversario || tabuleiro[i, coluna] == adversario)
                return true;
            if (linha == coluna && tabuleiro[i, i] == adversario)
                return true;
            if (linha + coluna == 2 && tabuleiro[i, 2 - i] == adversario)
                return true;
        }
        return false;
    }

    private bool BloquearOuVencer(int linha, int coluna, char jogador)
    {
        tabuleiro[linha, coluna] = jogador;
        bool resultado = ChecarVitoria(jogador);
        tabuleiro[linha, coluna] = ' ';
        return resultado;
    }

    public void MelhorJogada(char jogador)
    {
        int melhorPontuacao = int.MinValue;
        int melhorLinha = -1, melhorColuna = -1;

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (tabuleiro[i, j] == ' ')
                {
                    int pontuacaoAtual = AvaliarPosicao(i, j, jogador);
                    if (pontuacaoAtual > melhorPontuacao)
                    {
                        melhorPontuacao = pontuacaoAtual;
                        melhorLinha = i;
                        melhorColuna = j;
                    }
                }
            }
        }

        if (melhorLinha != -1 && melhorColuna != -1)
            tabuleiro[melhorLinha, melhorColuna] = jogador;
    }

    public bool ChecarVitoria(char jogador)
    {
        for (int i = 0; i < 3; i++)
        {
            if ((tabuleiro[i, 0] == jogador && tabuleiro[i, 1] == jogador && tabuleiro[i, 2] == jogador) ||
                (tabuleiro[0, i] == jogador && tabuleiro[1, i] == jogador && tabuleiro[2, i] == jogador))
                return true;
        }
        if ((tabuleiro[0, 0] == jogador && tabuleiro[1, 1] == jogador && tabuleiro[2, 2] == jogador) ||
            (tabuleiro[0, 2] == jogador && tabuleiro[1, 1] == jogador && tabuleiro[2, 0] == jogador))
            return true;
        return false;
    }

    public bool TabuleiroCheio()
    {
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                if (tabuleiro[i, j] == ' ')
                    return false;

        return true;
    }

    public void JogadaHumana(char jogador)
    {
        int linha, coluna;
        do
        {
            Console.WriteLine($"Jogador {jogador}, escolha sua linha e coluna (0-2) para jogar:");
            linha = int.Parse(Console.ReadLine());
            coluna = int.Parse(Console.ReadLine());
        } while (!PosicaoValida(linha, coluna));

        tabuleiro[linha, coluna] = jogador;
    }

    private bool PosicaoValida(int linha, int coluna)
    {
        return linha >= 0 && linha < 3 && coluna >= 0 && coluna < 3 && tabuleiro[linha, coluna] == ' ';
    }

    public static void JogoDaVelhaMain()
    {
        JogoDaVelha jogo = new JogoDaVelha();
        char jogadorAtual = 'X';
        bool fimDeJogo = false;

        while (!fimDeJogo)
        {
            jogo.ExibirTabuleiro();
            if (jogadorAtual == 'O')
            {
                jogo.MelhorJogada(jogadorAtual);
                Console.WriteLine("Jogada do Computador:");
            }
            else
            {
                jogo.JogadaHumana(jogadorAtual);
            }

            if (jogo.ChecarVitoria(jogadorAtual))
            {
                jogo.ExibirTabuleiro();
                Console.WriteLine($"Jogador {jogadorAtual} venceu!");
                fimDeJogo = true;
            }
            else if (jogo.TabuleiroCheio())
            {
                jogo.ExibirTabuleiro();
                Console.WriteLine("Empate!");
                fimDeJogo = true;
            }

            jogadorAtual = jogadorAtual == 'X' ? 'O' : 'X';
        }
    }
}

public class TicTacToe
{
    char[] board = new char[9];
    char currentPlayer = 'X';

    public TicTacToe()
    {
        for (int i = 0; i < 9; i++) board[i] = ' ';
    }

    public void PlayGame()
    {
        while (!CheckForWinner() && !IsBoardFull())
        {
            Console.Clear();
            PrintBoard();

            if (currentPlayer == 'O')
            {
                int move = BestMove();
                board[move] = currentPlayer;
                currentPlayer = 'X';
            }
            else
            {
                Console.WriteLine("Turno do jogador X. Escolha uma posição (1-9):");
                int move;
                bool validMove;
                do
                {
                    validMove = int.TryParse(Console.ReadLine(), out move) && IsValidMove(move - 1);
                    if (!validMove) Console.WriteLine("Movimento inválido, tente novamente.");
                } while (!validMove);

                board[move - 1] = currentPlayer;
                currentPlayer = 'O';
            }
        }

        Console.Clear();
        PrintBoard();
        if (CheckForWinner()) Console.WriteLine("Jogador " + (currentPlayer == 'X' ? 'O' : 'X') + " venceu!");
        else Console.WriteLine("Empate!");
    }

    private void PrintBoard()
    {
        for (int i = 0; i < 9; i += 3)
        {
            Console.WriteLine($" {board[i]} | {board[i + 1]} | {board[i + 2]} ");
            if (i < 6) Console.WriteLine("---+---+---");
        }
    }

    private bool IsValidMove(int index)
    {
        return index >= 0 && index < 9 && board[index] == ' ';
    }

    private bool CheckForWinner()
    {
        int[,] winConditions = new int[,]
        {
            {0, 1, 2}, {3, 4, 5}, {6, 7, 8}, // Linhas
            {0, 3, 6}, {1, 4, 7}, {2, 5, 8}, // Colunas
            {0, 4, 8}, {2, 4, 6}             // Diagonais
        };

        for (int i = 0; i < 8; i++)
        {
            int a = winConditions[i, 0], b = winConditions[i, 1], c = winConditions[i, 2];
            if (board[a] == board[b] && board[b] == board[c] && board[a] != ' ')
                return true;
        }

        return false;
    }

    private bool IsBoardFull()
    {
        for (int i = 0; i < board.Length; i++)
        {
            if (board[i] == ' ') return false;
        }
        return true;
    }

    private int MinMax(bool isMaximizing)
    {
        if (CheckForWinner()) return isMaximizing ? -1 : 1;
        if (IsBoardFull()) return 0;

        int bestScore = isMaximizing ? int.MinValue : int.MaxValue;
        for (int i = 0; i < board.Length; i++)
        {
            if (board[i] == ' ')
            {
                board[i] = isMaximizing ? 'O' : 'X';
                int score = MinMax(!isMaximizing);
                board[i] = ' ';
                bestScore = isMaximizing ? Math.Max(score, bestScore) : Math.Min(score, bestScore);
            }
        }
        return bestScore;
    }

    private int BestMove()
    {
        int bestScore = int.MinValue;
        int move = -1;
        for (int i = 0; i < board.Length; i++)
        {
            if (board[i] == ' ')
            {
                board[i] = 'O';
                int score = MinMax(false);
                board[i] = ' ';
                if (score > bestScore)
                {
                    bestScore = score;
                    move = i;
                }
            }
        }
        return move;
    }

    public static void TicTacToeMain()
    {
        TicTacToe game = new TicTacToe();
        game.PlayGame();
    }
}

public class Program
{
    public static void Main()
    {
        Console.WriteLine("Escolha o jogo que deseja jogar:");
        Console.WriteLine("1 - Jogo da Velha (tabuleiro 3x3)");
        Console.WriteLine("2 - Tic Tac Toe (tabuleiro 1x9)");
        int escolha = int.Parse(Console.ReadLine());

        switch (escolha)
        {
            case 1:
                JogoDaVelha.JogoDaVelhaMain();
                break;
            case 2:
                TicTacToe.TicTacToeMain();
                break;
            default:
                Console.WriteLine("Opção inválida.");
                break;
        }
    }
}
