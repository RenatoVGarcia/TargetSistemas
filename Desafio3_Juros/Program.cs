using System;
using System.Globalization;
using System.Threading;

namespace Desafio3_Juros
{
    internal class Program
    {
        private const decimal TaxaDiariaMora = 2.5m; 
        
        public static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("pt-BR");
            
            Console.WriteLine("\n--- Desafio 3: Cálculo de Juros de Dívida ---");
            
            decimal valorOriginal;
            string dataVencimentoStr;
            
            Console.Write("Digite o valor principal da dívida (Ex: 1500,00): R$ ");
            if (!decimal.TryParse(Console.ReadLine(), NumberStyles.Currency, CultureInfo.CurrentCulture, out valorOriginal) || valorOriginal <= 0)
            {
                Console.WriteLine("Valor inválido. O valor deve ser um número positivo.");
                return;
            }

            Console.Write("Digite a data de vencimento (DD/MM/AAAA): ");
            dataVencimentoStr = Console.ReadLine() ?? string.Empty;

            CalcularJuros(valorOriginal, dataVencimentoStr);
        }

        public static void CalcularJuros(decimal valorOriginal, string dataVencimentoStr)
        {
            DateTime hoje = DateTime.Today;

            if (!DateTime.TryParseExact(dataVencimentoStr, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dataVencimento))
            {
                Console.WriteLine("Erro: Formato de data inválido. Use DD/MM/AAAA.");
                return;
            }

            decimal jurosTotal = 0.0m;
            int diasAtraso = 0;

            Console.WriteLine($"\nData de Vencimento: {dataVencimento:dd/MM/yyyy}");
            Console.WriteLine($"Data de Cálculo (Hoje): {hoje:dd/MM/yyyy}");

            if (hoje > dataVencimento)
            {
                Console.WriteLine("\nStatus: ATRAZADO");
                
                diasAtraso = (hoje - dataVencimento).Days;
                
                decimal taxaDiaria = TaxaDiariaMora / 100.0m;
                jurosTotal = valorOriginal * taxaDiaria * diasAtraso;
                
                Console.WriteLine($"Dias de Atraso: {diasAtraso}");
                Console.WriteLine($"Taxa Aplicada: {TaxaDiariaMora:N2}% ao dia sobre o valor principal");
                Console.WriteLine($"Juros/Multa Total Acumulada: R$ {jurosTotal:N2}");
            }
            else
            {
                Console.WriteLine("\nStatus: EM DIA ou VENCIMENTO FUTURO");
                Console.WriteLine("Juros não aplicáveis.");
            }

            decimal valorTotalAPagar = valorOriginal + jurosTotal;
            
            Console.WriteLine($"\nValor Original: R$ {valorOriginal:N2}");
            Console.WriteLine($"**Valor Total a Pagar: R$ {valorTotalAPagar:N2}**");
        }
    }
}