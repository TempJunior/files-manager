using System.Text;
using System.Text.Json;

namespace ByteBankIO;

partial class Program
{
    private string filePath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "resources", "contas.txt");

    public void GeneratedJsonFile()
    {
        // Pega a lista de contas com saldo acima de 2000
        var contasFiltradas = FilterByBalanceAboveTwoThousand();
        
        // Configurações para o JSON (formatação bonita)
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        
        // Converte a lista para JSON
        var jsonString = JsonSerializer.Serialize(contasFiltradas, options);
        
        // Define o caminho do arquivo JSON
        var jsonFilePath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "resources", "contas_filtradas.json");
        
        // Escreve o arquivo JSON
        File.WriteAllText(jsonFilePath, jsonString);
        
        Console.WriteLine($"✅ Arquivo JSON gerado com sucesso!");
        Console.WriteLine($"📁 Localização: {Path.GetFullPath(jsonFilePath)}");
        Console.WriteLine($"📊 Total de contas: {contasFiltradas.Count}");
    }
    
    public List<ContaCorrente> FilterByBalanceAboveTwoThousand()
    {
        List<ContaCorrente> list = ListContas();
        return list.Where(con => con.Saldo > 2000)
            .ToList();
    }

    public List<ContaCorrente> ListContas()
    {
        List <ContaCorrente> contas = new List<ContaCorrente>();
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
                contas.Add(conta);
                Console.WriteLine($"Conta numero: {conta.Numero} -- Agencia : {conta.Agencia} - Saldo: {conta.Saldo} -- Titular: {conta.Titular.Nome}");
            }

            return contas;
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
        var numberAccountInt = int.Parse(numberAccount);
        var agenceInt = int.Parse(agence);
        var balanceDouble = double.Parse(balance);
        var holderParse = new Cliente();
        holderParse.Nome = holder;

        var concreteConta = new ContaCorrente(numberAccountInt, agenceInt);

        concreteConta.Depositar(balanceDouble);
        concreteConta.Titular = holderParse;
        return concreteConta;
    }

    static List<string> ProcessFile(string pathArquive)
    {
        List<string> text = new List<string>();
        try
        {
            //Informa o arquivo que quer trabalhar e que quer abrir o arquivo
            using var fileStream = new FileStream(pathArquive, FileMode.Open);
            var buffer = new byte[1024]; // um buffer de 1k

            // Devoluções:
            // 0 número total de bytes lidos do buffer. Isso poderá ser menor que o número de
            // bytes solicitado se esse número de bytes não estiver disponível no momento, ou
            //zero, se o final do fluxo for atingido
            var bytesRead = -1;


            while (bytesRead != 0)
            {
                bytesRead = fileStream.Read(buffer, 0, buffer.Length);
                var lines = WriteBuffer(buffer, bytesRead);
                text.Add(lines);
            }
        }
        catch (FileNotFoundException e)
        {
            Console.WriteLine(e.Message);
        }

        return text;
    }

    static void FoundFileAndListOnDirectory(string path)
    {
        Console.WriteLine($"Base Directory: {AppContext.BaseDirectory}");

        Console.WriteLine($"Caminho construído: {path}");
        Console.WriteLine($"Caminho absoluto: {Path.GetFullPath(path)}");

        Console.WriteLine($"Caminho construído: {path}");
        Console.WriteLine($"Caminho absoluto: {Path.GetFullPath(path)}");

        try
        {
            if (File.Exists(path))
            {
                Console.WriteLine("✅ Arquivo encontrado!");

                var pastaResources = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "resources");
                var allFiles = Directory.GetFiles(pastaResources, "*.txt");
                var nomesArquivos = allFiles.Select(f => Path.GetFileName(f));
                Console.WriteLine($"Arquivos .txt encontrados: {string.Join(", ", nomesArquivos)}");
            }
            else
            {
                Console.WriteLine("❌ Arquivo não encontrado");

                // Vamos verificar se a pasta resources existe
                var pastaResources = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "resources");
                Console.WriteLine($"Pasta resources existe: {Directory.Exists(pastaResources)}");
                Console.WriteLine($"Caminho da pasta: {Path.GetFullPath(pastaResources)}");

                // Listar arquivos na pasta resources
                if (Directory.Exists(pastaResources))
                {
                    var arquivos = Directory.GetFiles(pastaResources);
                    Console.WriteLine($"Arquivos na pasta resources: {string.Join(", ", arquivos)}");
                }
            }
        }
        catch (FileNotFoundException fileNotFoundException)
        {
            Console.WriteLine($"Archive not found: {fileNotFoundException.Message}");
        }
    }

// Recebe o array que vai guardar o buffer temporario
    static string WriteBuffer(byte[] buffer, int bytesRead)
    {
        //Faz a decodificação dos Bytes para a tabela unicode - utf8
        var utf8 = new UTF8Encoding();
        // Mostra o buffer a partir do início (índice 0) até a posição em que novos bytes foram lidos:
        var text = utf8.GetString(buffer, 0, bytesRead);
        Console.Write(text);

        //Vai passar por cada um dos bytes e mostrar
        // foreach (var b in buffer)
        // {
        //     Console.Write(b);
        //     Console.Write(" ");
        // }

        return text;
    }
}