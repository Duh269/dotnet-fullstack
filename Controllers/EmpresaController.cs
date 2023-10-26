using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestFullStack.Models;
using TestFullStack.Business;

namespace TestFullStack.Controllers
{
    public class EmpresaController : Controller
    {
        // GET: Cliente
        public ActionResult Index()
        {


            return View();
        }

       
        public ActionResult Consultar(string busca)
        {
            try
            {
                Empresa Empresa = new Empresa();
                List<EmpresaDto> list = null;
                if (busca != null)
                {
                    if (busca == "")
                    {
                        list = Empresa.Consultar();
                        TempData["listf"] = list;

                    } else
                    {
                        list = (List<EmpresaDto>)TempData["listf"];
                        list.Find(f => f.cnpj == Convert.ToInt32(busca) && f.cnpj == Convert.ToInt32(busca));
                        TempData.Keep("listf");
                    }

                    return PartialView("Grid", list.ToList());
                }
                else
                {
                    list = (List<EmpresaDto>)TempData["listf"];             
                    TempData.Keep("listf");

                    return View("Index");
                }


                
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);

            }

        }


        [HttpPost]
        public ActionResult ConsultarFornecedores(int empresa_id)
        {
            Empresa Empresa = new Empresa();

           var list = Empresa.ConsultarFornecedores(empresa_id);

            return PartialView("~/Views/Fornecedores/Grid.cshtml", list.ToList());
        }

        [HttpPost]
        public ActionResult Incluir(EmpresaDto model)
        {
            try
            {
                Empresa empresa = new Empresa();
                var list = empresa.Consultar();
                var filter = list.Where(f => f.cnpj == model.cnpj).ToList();
                if (filter.Count > 0)
                {
                    return Json(new { error = true, msg = "CNPJ já existe !" });
                }

                empresa.Inserir(model);

                return Json(new { error = false, msg = "Registro Inserido com Sucesso!" });
            }
            catch (Exception ex)
            {

                return Json(new { error = true, msg = ex.Message });
            }

        }

        [HttpPost]
        public ActionResult Alterar(EmpresaDto model)
        {
            try
            {
                Empresa Empresa = new Empresa();

                Empresa.Atualizar(model);

                return Json(new { error = false, msg = "Registro Atualizado com Sucesso!" });
            }
            catch (Exception ex)
            {

                return Json(new { error = true, msg = ex.Message });
            }

        }

        [HttpPost]
        public ActionResult Excluir(string id)
        {
            try
            {
                Empresa empresa = new Empresa();

                empresa.Deletar(Convert.ToInt32(id));

                return Json(new { error = false, msg = "Registro Excluido com Sucesso!" });
            }
            catch (Exception ex)
            {

                return Json(new { error = true, msg = "Ocorreu um erro deletar! Verifique as dependencias de fornecedores !" });
            }
        }
    }
}