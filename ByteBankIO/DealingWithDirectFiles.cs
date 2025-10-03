using System.Text;

namespace ByteBankIO;

partial class Program
{
    public void DealingWithFiles()
    {
        var pathArquive = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "resources", "contas.txt");

        FoundFileAndListOnDirectory(pathArquive);
        ProcessFile(pathArquive);
    }

    static void ProcessFile(string pathArquive)
    {
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
                WriteBuffer(buffer, bytesRead);
            }
        }
        catch (FileNotFoundException e)
        {
            Console.WriteLine(e.Message);
        }
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
    static void WriteBuffer(byte[] buffer, int bytesRead)
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
    }
}