using Dapper;
using Nancy;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using WebTeste.Models;


namespace WebTeste.entites
{
    public class conexao 
    {     
        string conect_bancoNovo = @"Data Source=DESKTOP-II0SEOF\SQLEXPRESS;Initial Catalog=DB_WEB;Integrated Security=True";

        public (bool,UsuarioModel) ConsultarUsuario(string usu, string psw)
        {
            var con = new SqlConnection(conect_bancoNovo);            
            try
            {
                con.Open();
                string query = $"select * from TB_Login where usuario = '{usu}' and senha ='{psw}'";

                var usuario = con.Query<UsuarioModel>(query).First();
                if (usuario != null)
                {
                    return (true, usuario);
                }
                else
                {
                    return (false,new UsuarioModel());
                }
            }
            catch (Exception)
            {
                return (false, new UsuarioModel());
            }
            finally
            {
                con.Close();
            }
          
        }
        public IEnumerable<Contas> GetContas(string? mes ,int IdUser)
        {
            mes = Regex.Replace(mes, @"[$,"",}]", "").Trim();
            var Mes = 0;
            if (mes == null)
            {
                Mes = DateTime.Now.Month;
            }
            else
            {
                Mes = int.Parse(mes);
            }
             

            string query = $"select * from TB_CONTAS where MONTH(Vencimento) = {Mes} and year(Vencimento) = DATEPART(year, getdate()) and Ativo = 1 and ID_USUARIO = {IdUser}";

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

        public void AtualizarConta(Contas conta, int idUser)
        {
            string valor = conta.Valor.ToString().Replace(",", ".");          

            string query = $"exec Update_Conta {conta.Id}, '{conta.Descricao}', {valor}, '{conta.Vencimento}', '{conta.Situacao}', {idUser}";

            using(var con = new SqlConnection(conect_bancoNovo))
            {
                con.Open();

                try
                {
                    con.Execute(query);
                }
                catch (Exception)
                {

                    throw;
                }
            }

        }
        public Contas GetContasPorID(int id)
        {      
            var Mes = DateTime.Now.Month;
            string query = $"select *from TB_CONTAS where  ID = {id}";
                
                //$"select ID,descricao,Valor,Vencimento,Situacao from TB_CONTAS where MONTH(Vencimento) = {Mes} and year(Vencimento) = DATEPART(year, getdate())and ID = {id}";

            var con = new SqlConnection(conect_bancoNovo);
            con.Open();
            var contas = con.Query<Contas>(query).First();
            if (contas != null)
            {
                return contas;
            }
            con.Close();
            return contas;
        }
        public void Cadastrar(Contas conta, string request)
        {
            string valor = conta.Valor.ToString().Replace(",",".");            
            string query = $"INSERT INTO TB_CONTAS(descricao,Valor,Vencimento,Situacao, ID_USUARIO) VALUES ('{conta.Descricao}',{valor},'{conta.Vencimento}','{conta.Situacao}', '{request}')";

            var con = new SqlConnection(conect_bancoNovo);
            con.Open();
            con.Query(query);
            con.Close();
        }
        public void CadastrarUsuario(UsuarioModel UsuarioModel) 
        {
            string query = $"INSERT INTO TB_LOGIN (usuario,senha,nome)VALUES(@USUARIO,@SENHA,@Nome)";
            var con = new SqlConnection(conect_bancoNovo);
            con.Open();
            con.Execute(query, UsuarioModel);
            con.Close();
        }

        public void deleteConta(int id)
        {
            
            string query = $"update TB_CONTAS set Ativo = 0 where ID = {id}";

            try
            {
                using (var con = new SqlConnection(conect_bancoNovo))
                {
                    con.Open();
                    con.Execute(query);
                };
            }
            catch (Exception)
            {

                throw;
            }                 
        }
    }
}
