using EstanteLivros;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
internal class Program
{
    private static void Main(string[] args)
    {
        MongoClient mongo = Conexao.ConexaoBanco();

        var base_de_dados = mongo.GetDatabase("estante");
        var collection = base_de_dados.GetCollection<BsonDocument>("book");
        var documents = collection.Find(new BsonDocument()).ToList();
        string t = "";
        
        int op = 0;
        do
        {
            Console.WriteLine("Escolha um das opções");
            Console.WriteLine("[1] - PROCURAR POR LIVRO");
            Console.WriteLine("[2] - ALTERAR INFORMAÇÕES DO LIVRO");
            Console.WriteLine("[3] - INSERIR NOVO LIVRO");
            Console.WriteLine("[4] - REMOVER LIVRO");
            Console.WriteLine("[5] - LISTAR LIVROS NA ESTANTE");
            Console.WriteLine("[6] - LISTAR LIVROS EMPRESTADOS");
            Console.WriteLine("[7] - LISTAR LIVROS QUE ESTÃO SENDO LIDOS");
            Console.WriteLine("[8] - MUDAR O STATUS");
            Console.WriteLine("[9] - SAIR");

            op = int.Parse(Console.ReadLine());

            switch(op)
            {
                case 1:
                    Console.WriteLine("Informe o titulo do livro: ");
                    t = Console.ReadLine();
                    var filtro = Builders<BsonDocument>.Filter.Regex("titulo", t);
                    var p = collection.Find(filtro).FirstOrDefault();
                    var book = BsonSerializer.Deserialize<Book>(p);
                    Console.WriteLine(book.ToString());    
                break;

                case 2:
                    Console.WriteLine("Digite o título atual:");
                    string titulo_atual = Console.ReadLine();

                    Console.WriteLine("Digite o novo título: ");
                    string novo_titulo = Console.ReadLine();

                    filtro = Builders<BsonDocument>.Filter.Regex("titulo", titulo_atual);
                    var update = Builders<BsonDocument>.Update.Set("titulo", novo_titulo);

                    collection.UpdateMany(filtro, update);

                break;

                case 3:
                    Console.WriteLine("Digite o título: ");
                    string titulo = Console.ReadLine();

                    Console.WriteLine("Digite a editora: ");
                    string editora = Console.ReadLine();

                    Console.WriteLine("Digite o ano de publicação:");
                    int ano = int.Parse(Console.ReadLine());

                    Console.WriteLine("Digite o autor");
                    string autor = Console.ReadLine();

                    Console.WriteLine("Digite o ISBN: ");
                    double isbn = double.Parse(Console.ReadLine());

                    Book livro = new Book(titulo, editora, ano, autor, isbn, 0);
                    Console.WriteLine(livro.ToString());

                    var construcao_livro = new BsonDocument
                    {
                        {"titulo",livro.Titulo},
                        {"Editora",livro.Editora},
                        {"Ano_publicacao",livro.Ano_publicacao},
                        {"Autor",livro.Autor},
                        {"ISBN",livro.ISBN},
                        {"status",livro.Situacao }
                    };

                    Console.WriteLine(construcao_livro);
                    collection.InsertOne(construcao_livro);

                break;

                case 4:
                    Console.WriteLine("Digite o título do livro");
                    titulo = Console.ReadLine();
                    filtro = Builders<BsonDocument>.Filter.Regex("titulo", titulo);
                    collection.DeleteOne(filtro);

                break;

                case 5:
                    //  LISTAR LIVROS NA ESTANTE
                    List<Book> livros_estante = new List<Book>();
                    foreach (var document in documents)
                    {
                        //Console.WriteLine(document);
                        book = BsonSerializer.Deserialize<Book>(document);
                        if(book.Situacao == 0)
                        {
                            //Console.WriteLine(book.ToString());
                            //Console.WriteLine();
                            livros_estante.Add(book);
                        }
                    }
                    if(livros_estante.Count == 0)
                    {
                        Console.WriteLine("Não há livros na estante");
                    }
                    else
                    {
                        Console.WriteLine("LIVROS NA ESTANTE: ");
                        Console.WriteLine();
                        livros_estante.ForEach(livro => Console.WriteLine(livro.ToString() + "\n"));
                    } 
                    
                break;

                case 6:
                    // LISTAR LIVROS EMPRESTADOS
                    List<Book> livros_emprestados = new List<Book>();
                    foreach (var document in documents)
                    {
                        //Console.WriteLine(document);
                        book = BsonSerializer.Deserialize<Book>(document);
                        if (book.Situacao == 1)
                        {
                            //Console.WriteLine(book.ToString());
                            //Console.WriteLine();
                            livros_emprestados.Add(book);
                        }
                    }
                    if(livros_emprestados.Count == 0)
                    {
                        Console.WriteLine("NENHUM LIVRO FOI EMPRESTADO !");
                    }
                    else
                    {
                        Console.WriteLine("LIVROS EMPRESTADOS: ");
                        Console.WriteLine();
                        livros_emprestados.ForEach(livro => Console.WriteLine(livro.ToString() + "\n"));
                    }
                    
                break;

                case 7:
                    // LISTAR LIVROS QUE ESTÃO SENDO LIDOS
                    List<Book> livros_lidos = new List<Book>();
                    foreach (var document in documents)
                    {
                        //Console.WriteLine(document);
                        book = BsonSerializer.Deserialize<Book>(document);
                        if (book.Situacao == 2)
                        {
                            //Console.WriteLine(book.ToString());
                            //Console.WriteLine();
                            livros_lidos.Add(book);
                        }
                    }
                    if (livros_lidos.Count == 0)
                    {
                        Console.WriteLine("NENHUM LIVRO ESTÁ SENDO LIDO !");
                    }
                    else
                    {
                        Console.WriteLine("LIVROS QUE ESTÃO SENDO LIDOS: ");
                        Console.WriteLine();
                        livros_lidos.ForEach(livro => Console.WriteLine(livro.ToString() + "\n"));
                    }
                    break;

                case 8:
                    // MUDAR O STATUS
                    Console.WriteLine("Digite o título do livro: ");
                    t = Console.ReadLine();
                    int op_status = 0;

                    do
                    {
                        Console.WriteLine("[1] - DEVOLVER PARA ESTANTE");
                        Console.WriteLine("[2] - EMPRESTAR");
                        Console.WriteLine("[3] - COMEÇAR LEITURA");
                        op_status = int.Parse(Console.ReadLine());

                    } while (op_status != 1 && op_status != 2 && op_status != 3);

                    filtro = Builders<BsonDocument>.Filter.Regex("titulo", t);

                    switch (op_status)
                    { 
                        case 1:
                            update = Builders<BsonDocument>.Update.Set("status", 0);
                            collection.UpdateMany(filtro, update);
                        break;

                        case 2:
                            update = Builders<BsonDocument>.Update.Set("status", 1);
                            collection.UpdateMany(filtro, update);
                        break;

                        case 3:
                            update = Builders<BsonDocument>.Update.Set("status", 2);
                            collection.UpdateMany(filtro, update);
                       break;
                    }

                break;

                case 9:
                    Console.WriteLine("Saindo");
                break;

                default:
                    Console.WriteLine("Opção inválida !!!");
                break;
            }

          
        }while(op != 9);
        
    }
}