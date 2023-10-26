using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using TestFullStack.Models;

namespace TestFullStack.Models
{
    public class FornecedorDto
    {
        public long fornecedor_id { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        public string cnpj { get; set; }
        public string cpf { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        public string nome { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        public string email { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        public int cep { get; set; }
        public DateTime? data_nascimento { get; set; }
        public int rg { get; set; }
    }
}

namespace TestFullStack.Business
{
    public class Fornecedor 
    {

        public List<FornecedorDto> GetListDto(DataTable dt) {

            List<FornecedorDto> list = new List<FornecedorDto>();

            try
            {
                foreach (DataRow item in dt.Rows)
                {
                    FornecedorDto fornecedor = new FornecedorDto();
                    fornecedor.rg = Convert.ToInt32(item["rg"].ToString());
                    fornecedor.email = item["email"].ToString();
                    fornecedor.cnpj = item["cnpj"].ToString();
                    fornecedor.cpf = item["cpf"].ToString();
                    fornecedor.data_nascimento = Convert.ToDateTime(item["data_nascimento"]);
                    fornecedor.fornecedor_id = Convert.ToInt64(item["fornecedor_id"].ToString());
                    fornecedor.nome = item["nome"].ToString();
                    fornecedor.cep = Convert.ToInt32(item["cep"].ToString());
                   

                    list.Add(fornecedor);

                }

                return list;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        } 
        public List<FornecedorDto> Consultar()
        {
            try
            {
                DataTable dt = DBContext.DataTable("select * from fornecedor");

                List<FornecedorDto> list = new List<FornecedorDto>();

                list = GetListDto(dt);

                return list;
            }
            catch (Exception ex)
            {

                throw ex;
            }

           
        }

        public List<FornecedorDto> Consultar(bool fake)
        {
            List<FornecedorDto> list = new List<FornecedorDto>() { 
                new FornecedorDto{ fornecedor_id = 1, nome = "teste 1", email = "teste@hotmail.com", cep = 8340387 },
                new FornecedorDto{ fornecedor_id =2 , nome = "teste 2", email = "teste2@gmail.com" ,cep = 4654654, cnpj = "541654654" }
            
            };

            return list;
        }

        public int Inserir(FornecedorDto model)
        {
            string dt = model.data_nascimento.Value.ToString("yyyy-MM-dd");

            string query  = "insert into fornecedor (nome,rg,email,cep,cnpj,cpf,data_nascimento) values ('" + model.nome + "', "+ model.rg
                +", '"+ model.email +"', "+ model.cep +",'"+ model.cnpj  +"', '"+ model.cpf+"','"+ dt + "')";


           var retorno = DBContext.Executar(query);

            return retorno;
        }

        public int Atualizar(FornecedorDto model)
        {
            string dt = model.data_nascimento.Value.ToString("yyyy-MM-dd");


            string query = "update fornecedor set nome='"+ model.nome + "',rg= '"+ model.rg +"',email='" + model.email +"',cep= "+ model.cep +",data_nascimento= '"+ dt + "'  where fornecedor_id=" + model.fornecedor_id;


            var retorno = DBContext.Executar(query);

            return retorno;
        }

        public int Deletar(int id)
        {
            string query = "delete from fornecedor where fornecedor_id=" + id;

            var retorno = DBContext.Executar(query);

            return retorno;

        }

        public List<EmpresaDto> ConsultarEmpresas(int fornecedor_id)
        {
            try
            {
                DataTable dt = DBContext.DataTable("select e.* from empresa_fornecedor f inner join empresa e where f.fornecedor_id=" + fornecedor_id);
                Empresa empresa = new Empresa();
               var list = empresa.GetListDto(dt);

                return list;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }

}