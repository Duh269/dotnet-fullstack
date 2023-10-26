using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using TestFullStack.Models;

namespace TestFullStack.Models
{
    public class EmpresaDto
    {
        public long empresa_id { get; set; }
        public int cnpj { get; set; }
        public string nome_fantasia { get; set; }
        public int cep { get; set; }
    }
}

namespace TestFullStack.Business
{
    public class Empresa {
    
        public List<FornecedorDto> ConsultarFornecedores(int empresa_id)
        {
            try
            {
                List<FornecedorDto> list = new List<FornecedorDto>();
                Fornecedor Fornecedor = new Fornecedor();

                DataTable dt = DBContext.DataTable("select * from empresa_fornecedor  where empresa_id=" + empresa_id);

                list = Fornecedor.GetListDto(dt);


                return list;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<EmpresaDto> GetListDto(DataTable dt)
        {
            List<EmpresaDto> list = new List<EmpresaDto>();

            foreach (DataRow item in dt.Rows)
            {
                EmpresaDto empresa = new EmpresaDto();

                empresa.empresa_id = Convert.ToInt64(item["empresa_id"]);
                empresa.cnpj = Convert.ToInt32(item["empresa_id"]);
                empresa.nome_fantasia = item["nome_fantasia"].ToString();
                empresa.cep = Convert.ToInt32(item["cep"]);
                list.Add(empresa);
            }

            return list;
        }
        public List<EmpresaDto> Consultar()
        {
            try
            {
                List<EmpresaDto> list = new List<EmpresaDto>();

                DataTable dt = DBContext.DataTable("select * from empresa");
               list = GetListDto(dt);
               

                return list;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<EmpresaDto> Consultar(bool fake)
        {
            try
            {
                List<EmpresaDto> list = null;

                list = new List<EmpresaDto>()
                {
                    new EmpresaDto() { empresa_id = 1, cnpj = 564564564 , nome_fantasia = "Empresa 1" },
                    new EmpresaDto() { empresa_id = 2, cnpj = 14516563 , nome_fantasia = "Empresa 2" }
                };

                return list;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public int Inserir(EmpresaDto model)
        {
            string query = "insert into empresa (nome_fantasia,cep,cnpj) values ('" + model .nome_fantasia + "',"+ model.cep +", "+ model.cnpj +")";

            var retorno = DBContext.Executar(query);

            return retorno;
        }

        public int Atualizar(EmpresaDto model)
        {
            string query = "update empresa set nome_fantasia='"+ model.nome_fantasia + "', cep='"+ model.cep + "' where empresa_id=" + model.empresa_id;

            var retorno = DBContext.Executar(query);

            return retorno;
        }
    
        public int Deletar(int empresa_id)
        {
            string query = "delete from empresa where empresa_id=" + empresa_id;

            var retorno = DBContext.Executar(query);

            return retorno;
        }

    }

}