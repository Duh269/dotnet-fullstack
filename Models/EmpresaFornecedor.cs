using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestFullStack.Models;

namespace TestFullStack.Models
{
    public class EmpresaFornecedorDto
    {
        public long empresa_id { get; set; }
        public long fornecedor_id { get; set; }
    }

   
}

namespace TestFullStack.Business
{

    public class EmpresaFornecedor
    {
        public int Incluir(int empresa_id, int fornecedor_id)
        {
            try
            {
                string query = "insert into empresa_fornecedor (empresa_id,fornecedor_id) values ("+ empresa_id + ","+ fornecedor_id + ")";

                var retorno = DBContext.Executar(query);


                return 0;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

      
    }
}