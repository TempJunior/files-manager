using System.Text;

namespace ByteBankIO;

partial class Program
{
    static void Main(string[] args)
    {
        var filePath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "resources", "contas.txt");
        using (var fileStream = new FileStream(filePath, FileMode.Open))
        {
            // StreamReader já lida com Strings ao inves de Bytes
            var reader = new StreamReader(fileStream);
            //Lê apenas uma linha do arquivo
            //var line = reader.ReadLine();
            Console.WriteLine("Printing line");
            //Console.Write(line);
            
            //var text = reader.ReadToEnd(); // Lê o arquivo completo
            // Carrega o arquivo completo de uma só vez
            //Console.WriteLine(text);

            // EndOfStream entende o final e ultima linha do arquivo. Imprime uma linha de cada vez e nao carrega
            // o arquivo completo
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine()!;
                var conta = ConvertFileToContaCorrente(line);
                Console.WriteLine($"Conta numero: {conta.Numero} -- Agencia : {conta.Agencia} - Saldo: {conta.Saldo} -- Titular: {conta.Titular.Nome}");
            }
        }
    }

    static ContaCorrente ConvertFileToContaCorrente(string line)
    {
        var fields = line.Split(",");
        var numberAccount = fields[0];
        var agence = fields[1];
        var balance = fields[2].Replace('.', ',');
        var holder = fields[3];
        
        //String parse
        var numberAccountInt =  int.Parse(numberAccount);
        var agenceInt =  int.Parse(agence);
        var balanceDouble =  double.Parse(balance);
        var holderParse = new Cliente();
        holderParse.Nome = holder;

        var concreteConta = new ContaCorrente(numberAccountInt, agenceInt);
        
        concreteConta.Depositar(balanceDouble);
        concreteConta.Titular = holderParse;
        return concreteConta;
    }
}