using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Globalization;

namespace Desafio1_Comissao
{
    public class Venda
    {
        public string Vendedor { get; set; } = string.Empty;
        public decimal Valor { get; set; }
    }

    public class DadosVendas
    {
        public List<Venda> Vendas { get; set; } = null!;
    }

    internal class Program
    {
        private const string NomeArquivo = "vendas.json";

        public static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("pt-BR");
            
            Console.WriteLine("--- Desafio 1: Cálculo de Comissão ---");
            
            var comissoesPorVendedor = new Dictionary<string, decimal>();

            try
            {
                string jsonString = File.ReadAllText(NomeArquivo);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var dados = JsonSerializer.Deserialize<DadosVendas>(jsonString, options);

                if (dados?.Vendas == null || !dados.Vendas.Any())
                {
                    Console.WriteLine("Erro: Nenhuma venda encontrada ou JSON vazio.");
                    return;
                }

                foreach (var venda in dados.Vendas)
                {
                    decimal comissao = CalcularComissao(venda.Valor);

                    if (!comissoesPorVendedor.ContainsKey(venda.Vendedor))
                    {
                        comissoesPorVendedor[venda.Vendedor] = 0;
                    }
                    comissoesPorVendedor[venda.Vendedor] += comissao;
                }

                ExibirResultados(comissoesPorVendedor);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Erro: Arquivo '{NomeArquivo}' não encontrado. Verifique a pasta.");
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Erro ao desserializar o JSON: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro inesperado: {ex.Message}");
            }
        }

        private static decimal CalcularComissao(decimal valorVenda)
        {
            if (valorVenda >= 500.00m)
            {
                return valorVenda * 0.05m;
            }
            else if (valorVenda >= 100.00m)
            {
                return valorVenda * 0.01m;
            }
            else
            {
                return 0.0m;
            }
        }

        private static void ExibirResultados(Dictionary<string, decimal> comissoes)
        {
            Console.WriteLine("\n--- Relatório de Comissão Total por Vendedor ---");
            foreach (var kvp in comissoes.OrderBy(k => k.Key))
            {
                Console.WriteLine($"Vendedor: **{kvp.Key,-15}** | Comissão Total: **R$ {kvp.Value:N2}**");
            }
            Console.WriteLine("-------------------------------------------------");
        }
    }
}