using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Globalization;
using System.Threading;

namespace Desafio2_Estoque
{
    public class ProdutoEstoque
    {
        public int CodigoProduto { get; set; }
        public string DescricaoProduto { get; set; } = string.Empty;
        public int EstoqueAtual { get; set; }
    }

    public class Movimentacao
    {
        public Guid IdMovimentacao { get; set; } = Guid.NewGuid();
        public int CodigoProduto { get; set; }
        public string TipoMovimentacao { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public int EstoqueFinal { get; set; }
        public DateTime DataMovimentacao { get; set; } = DateTime.Now;
    }

    public class DadosEstoque
    {
        public List<ProdutoEstoque> Estoque { get; set; } = null!;
    }

    internal class Program
    {
        private const string ArquivoEstoque = "estoque.json";
        private static List<ProdutoEstoque> _produtos = new List<ProdutoEstoque>();
        private static readonly List<Movimentacao> LogMovimentacoes = new List<Movimentacao>();

        public static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("pt-BR");

            Console.WriteLine("--- Desafio 2: Sistema de Movimentação de Estoque ---");
            _produtos = CarregarEstoqueInicial();
            
            if (_produtos.Count == 0)
            {
                Console.WriteLine("❌ Não é possível iniciar. Verifique o arquivo estoque.json e se ele tem conteúdo.");
                return;
            }
            
            while (true)
            {
                ExibirEstoque();
                
                Console.WriteLine("\n--- Menu ---");
                Console.WriteLine("1. Realizar Movimentação (Entrada/Saída)");
                Console.WriteLine("2. Exibir Log de Movimentações");
                Console.WriteLine("3. Sair e Salvar Estoque Atualizado");
                Console.Write("Escolha uma opção: ");

                var opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        RealizarMovimentacao();
                        break;
                    case "2":
                        ExibirLogMovimentacoes();
                        break;
                    case "3":
                        SalvarEstoque();
                        return;
                    default:
                        Console.WriteLine("Opção inválida.");
                        break;
                }
            }
        }

        private static List<ProdutoEstoque> CarregarEstoqueInicial()
        {
            try
            {
                string jsonString = File.ReadAllText(ArquivoEstoque);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var dados = JsonSerializer.Deserialize<DadosEstoque>(jsonString, options);
                
                return dados?.Estoque ?? new List<ProdutoEstoque>();
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Erro: Arquivo '{ArquivoEstoque}' não encontrado.");
                return new List<ProdutoEstoque>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar estoque: {ex.Message}");
                return new List<ProdutoEstoque>();
            }
        }

        private static void ExibirEstoque()
        {
            Console.WriteLine("\n*** Estoque Atualizado ***");
            Console.WriteLine("Código | Descrição                             | Qtd. Atual");
            Console.WriteLine("-----------------------------------------------------");
            foreach (var p in _produtos)
            {
                Console.WriteLine($"{p.CodigoProduto,-6} | {p.DescricaoProduto,-30} | {p.EstoqueAtual}");
            }
            Console.WriteLine("*****************************************************");
        }

        private static void ExibirLogMovimentacoes()
        {
            if (!LogMovimentacoes.Any())
            {
                Console.WriteLine("\nNão há movimentações registradas ainda.");
                return;
            }
            
            Console.WriteLine("\n*** Log de Movimentações ***");
            Console.WriteLine("ID Unico (Parte) | Código | Tipo    | Quantidade | Estoque Final | Descrição");
            Console.WriteLine("----------------------------------------------------------------------------------");
            foreach (var m in LogMovimentacoes)
            {
                Console.WriteLine(
                    $"{m.IdMovimentacao.ToString().Substring(0, 8),-16} | {m.CodigoProduto,-6} | {m.TipoMovimentacao,-7} | {m.Quantidade,-10} | {m.EstoqueFinal,-13} | {m.Descricao}"
                );
            }
            Console.WriteLine("----------------------------------------------------------------------------------");
        }

        private static void SalvarEstoque()
        {
            try
            {
                var dados = new DadosEstoque { Estoque = _produtos };
                var options = new JsonSerializerOptions { 
                    WriteIndented = true, 
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping 
                };
                string jsonString = JsonSerializer.Serialize(dados, options);
                File.WriteAllText(ArquivoEstoque, jsonString);
                Console.WriteLine("\n✅ Estoque atualizado salvo em 'estoque.json'.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao salvar estoque: {ex.Message}");
            }
        }

        private static void RealizarMovimentacao()
        {
            Console.Write("Digite o código do produto para movimentar: ");
            if (!int.TryParse(Console.ReadLine(), out int cod))
            {
                Console.WriteLine("Código inválido.");
                return;
            }

            var produto = _produtos.FirstOrDefault(p => p.CodigoProduto == cod);

            if (produto == null)
            {
                Console.WriteLine("Produto não encontrado.");
                return;
            }
            
            Console.WriteLine($"Produto Selecionado: {produto.DescricaoProduto}");

            Console.Write("Tipo de movimentação (E para Entrada / S para Saída): ");
            var tipoStr = Console.ReadLine()?.ToUpper() ?? string.Empty;
            
            if (tipoStr != "E" && tipoStr != "S")
            {
                Console.WriteLine("Tipo de movimentação inválido. Use 'E' ou 'S'.");
                return;
            }

            Console.Write("Quantidade: ");
            if (!int.TryParse(Console.ReadLine(), out int qtde) || qtde <= 0)
            {
                Console.WriteLine("Quantidade inválida ou não positiva.");
                return;
            }

            Console.Write("Descrição da movimentação (Ex: Venda, Compra, Ajuste): ");
            var desc = Console.ReadLine() ?? string.Empty;

            int estoqueAtual = produto.EstoqueAtual;
            int novoEstoque;
            string tipoMovimentacaoDesc;

            if (tipoStr == "E")
            {
                novoEstoque = estoqueAtual + qtde;
                tipoMovimentacaoDesc = "ENTRADA";
            }
            else
            {
                if (qtde > estoqueAtual)
                {
                    Console.WriteLine($"Erro: Saída de {qtde} excede o estoque atual de {estoqueAtual}.");
                    return;
                }
                novoEstoque = estoqueAtual - qtde;
                tipoMovimentacaoDesc = "SAÍDA";
            }

            produto.EstoqueAtual = novoEstoque;

            var mov = new Movimentacao
            {
                CodigoProduto = cod,
                TipoMovimentacao = tipoMovimentacaoDesc,
                Descricao = desc,
                Quantidade = qtde,
                EstoqueFinal = novoEstoque
            };
            LogMovimentacoes.Add(mov);

            Console.WriteLine($"\n✅ Movimentação de {tipoMovimentacaoDesc} realizada para {produto.DescricaoProduto}.");
            Console.WriteLine($"Estoque final: **{novoEstoque}**");
        }
    }
}