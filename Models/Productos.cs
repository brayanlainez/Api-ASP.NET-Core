using System;
using System.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Api_ASP.NET_Core
{
    public class Productos{
        public int id { get; set; }
        public string nombre { get; set; }
        public string codigo_barra { get; set; }
        public double precio { get; set; }
        public int disponible { get; set; }
        public string detalle { get; set; }
        public string imagen { get; set; }

        internal AppDb Db { get; set; }

        public Productos(){

        }

        internal Productos(AppDb db){
            Db = db;
        }

        public async Task InsertAsync(){
            //Basic command and connection initialization 
            using MySqlConnection conn = Db.Connection;            
            using MySqlCommand cmd = new MySqlCommand("spInsertarProduto", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            BindParams(cmd);
            BindLastInsertId(cmd);
            await cmd.ExecuteNonQueryAsync();
            // Get values from the output params
            this.id = (int) cmd.Parameters["@p_last_insert_id"].Value;
            // this.id = (int) cmd.LastInsertedId; Solo funciona con el query Insert directamente 
            // Console.WriteLine(this.id);
        }

        public async Task UpdateAsync(){
            using MySqlConnection conn = Db.Connection;            
            using MySqlCommand cmd = new MySqlCommand("spActualizarProduto", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            // using MySqlCommand cmd = Db.Connection.CreateCommand();
            // cmd.CommandText = @"CALL spActualizarProduto(@p_id, @p_nombre, @p_codigo_barra, @p_precio, @p_disponible, @p_detalle, @p_imagen);";
            BindId(cmd);
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(){
            using MySqlConnection conn = Db.Connection;            
            using MySqlCommand cmd = new MySqlCommand("spEliminarProduto", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            // using MySqlCommand cmd = Db.Connection.CreateCommand();
            // cmd.CommandText = @"CALL spEliminarProduto(@id);";
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        private void BindId(MySqlCommand cmd){
            cmd.Parameters.Add(new MySqlParameter{ParameterName = "@p_id", DbType = DbType.Int32, Value = id });
        }

        private void BindParams(MySqlCommand cmd){
            cmd.Parameters.Add(new MySqlParameter{ParameterName = "@p_nombre", DbType = DbType.String, Value = nombre });
            cmd.Parameters.Add(new MySqlParameter{ParameterName = "@p_codigo_barra", DbType = DbType.String, Value = codigo_barra });
            cmd.Parameters.Add(new MySqlParameter{ParameterName = "@p_precio", DbType = DbType.Double, Value = precio });
            cmd.Parameters.Add(new MySqlParameter{ParameterName = "@p_disponible", DbType = DbType.Int32, Value = disponible });
            cmd.Parameters.Add(new MySqlParameter{ParameterName = "@p_detalle", DbType = DbType.String, Value = detalle });
            cmd.Parameters.Add(new MySqlParameter{ParameterName = "@p_imagen", DbType = DbType.String, Value = imagen });
        }
    
        private void BindLastInsertId(MySqlCommand cmd){
            cmd.Parameters.Add(new MySqlParameter{ParameterName = "@p_last_insert_id", DbType = DbType.Int32 }).Direction = ParameterDirection.Output;
        }
    }
}