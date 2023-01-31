using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using WebTeste.Models;


namespace WebTeste.entites
{
    public class conexao 
    {
        
        //string conectContas = @"Data Source=DESKTOP-II0SEOF\SQLEXPRESS;Initial Catalog=DB_CONTAS;Integrated Security=True";
        string conect_bancoNovo = @"Data Source=DESKTOP-II0SEOF\SQLEXPRESS;Initial Catalog=DB_WEB;Integrated Security=True";


        public bool ConsultarUsuario(string usu, string psw)
        {
            var con = new SqlConnection(conect_bancoNovo);
            bool retorno = false;
            try
            {
                con.Open();
                string query = $"select * from TB_Login where usuario = '{usu}' and senha ='{psw}'";

                var usuario = con.Query(query);
                if (usuario.Count() > 0)
                {
                    retorno = true;
                }
                else
                {
                    retorno = false;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
            return retorno;
        }
        public IEnumerable<Contas> GetContas(int Ofsset)
        {
            //var offset = 1;

            //if (offset != 1)
            //{
            //    offset = (10 * (offset - 1)) + 1;
            //}
            
            var Mes = DateTime.Now.Month;

            string query = $"DECLARE @limit int = 10, @offset int = {Ofsset};with resultado as (select * , ROW_NUMBER() over(order by ID) as linha" +                
                $" from TB_CONTAS where MONTH(Vencimento) = {Mes} and year(Vencimento) = DATEPART(year, getdate())" +
                $")select * from resultado where linha >= @offset and linha < @offset + @limit";

            var con = new SqlConnection(conect_bancoNovo);
            con.Open();
            var contas = con.Query<Contas>(query);
            if (contas.Count() > 0)
            {
                return contas;
            }
            con.Close();
            return contas;
        }
        public IEnumerable<Contas> GetContasPorDescricao(string descricao)
        {
            var Mes = DateTime.Now.Month;
            string query = $"select ID,descricao,Valor,Vencimento,Situacao from TB_CONTAS where MONTH(Vencimento) = {Mes} and year(Vencimento) = DATEPART(year, getdate())and descricao like '%{descricao}%'";

            var con = new SqlConnection(conect_bancoNovo);
            con.Open();
            var contas = con.Query<Contas>(query);
            if (contas.Count() > 0)
            {
                return contas;
            }
            con.Close();
            return contas;
        }
        public void Cadastrar(Contas conta)
        {
            string valor = conta.Valor.ToString().Replace(",",".");            
            string query = $"INSERT INTO TB_CONTAS(descricao,Valor,Vencimento,Situacao) VALUES ('{conta.Descricao}',{valor},'{conta.Vencimento}','{conta.Situacao}')";

            var con = new SqlConnection(conect_bancoNovo);
            con.Open();
            con.Query(query);
            con.Close();
        }
        public void CadastrarUsuario(UsuarioModel UsuarioModel) 
        {
            string query = $"INSERT INTO TB_LOGIN (usuario,senha)VALUES(@USUARIO,@SENHA)";
            var con = new SqlConnection(conect_bancoNovo);
            con.Open();
            con.Execute(query, UsuarioModel);
            con.Close();
        }
    }
}
