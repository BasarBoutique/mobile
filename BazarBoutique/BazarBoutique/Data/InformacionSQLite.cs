using BazarBoutique.Modelos.ModeloSQL;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BazarBoutique.Data
{
    public class InformacionSQLite
    {

        private readonly SQLiteAsyncConnection BD;

        public InformacionSQLite(string dbPath)
        {
            BD = new SQLiteAsyncConnection(dbPath);
            BD.CreateTableAsync<UsuarioSql>().Wait();
        }

        public Task<int>GuardandoUsuarioAsync(UsuarioSql usuario)
        {
            return BD.InsertAsync(usuario);
        }

        public Task<List<UsuarioSql>> RecivirUsuarioAsync()
        {
            var usuarios = BD.Table<UsuarioSql>().ToListAsync();

            return usuarios;
        }

        public Task<int> DeleteAllAsync()
        {
            var resultUsers = BD.DeleteAllAsync<UsuarioSql>();
            return resultUsers;
        }

    }
}
