using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace EstanteLivros
{
    internal class Conexao
    {
        public static MongoClient ConexaoBanco()
        {
            MongoClient mongo = null;
            try
            {
                mongo = new MongoClient("mongodb://localhost:27017");
                return mongo;
            }
            catch (Exception ex) 
            {
                Console.WriteLine("Não foi possível se conectar ao banco: " + ex.Message);
                return mongo;
            }
           
        }
    }
}
