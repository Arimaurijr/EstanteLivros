using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace EstanteLivros
{
    [BsonIgnoreExtraElements]
    internal class Book
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }


        [BsonElement("titulo")]
        public string? Titulo { get; set; }


        [BsonElement("Editora")]
        public string? Editora { get; set; }


        [BsonElement("Ano_publicacao")]
        public int? Ano_publicacao { get; set; }


        [BsonElement("Autor")]
        public string? Autor { get; set; }


        [BsonElement("ISBN")]
        public double? ISBN { get; set; }


        [BsonElement("status")]
        public int? Situacao { get; set; }

        public Book(string? titulo, string? editora, int? ano_publicacao, string? autor, double? iSBN, int? situacao)
        {
            Titulo = titulo;
            Editora = editora;
            Ano_publicacao = ano_publicacao;
            Autor = autor;
            ISBN = iSBN;
            Situacao = situacao;
        }

        public override string ToString()
        {
            string livro_impresso = $"Titulo:{Titulo} \nEditora: {Editora} \nAno de publicação:{Ano_publicacao} \nAutor:{Autor} \nISBN: {ISBN}";

            return livro_impresso;
        }
        
      
    }
}
