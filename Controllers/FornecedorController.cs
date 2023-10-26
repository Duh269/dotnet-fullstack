using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using TestFullStack.Business;
using TestFullStack.Models;

namespace TestFullStack.Controllers
{
    public class FornecedorController : Controller
    {
        // GET: Fornecedor

        public ActionResult Index()
        {
            return View();
        }

      
       public ActionResult Consultar(string busca)
        {
            try
            {
                Fornecedor Fornecedor = new Fornecedor();
                List<FornecedorDto> list = null;
                if (busca != null)
                {
                    if (busca == "")
                    {
                        list = Fornecedor.Consultar();
                        TempData["listf"] = list;
                    } else
                    {
                        list = (List<FornecedorDto>)TempData["listf"];
                        list = list.Where(d => d.cnpj.Contains(busca) || d.cpf.Contains(busca)).ToList();
                        TempData["listf"] = list;
                        TempData.Keep("listf");
                    }

                    return PartialView("Grid", list.ToList());
                }  else
                {
                    list = (List<FornecedorDto>)TempData["listf"];
                    TempData.Keep("listf");
                    return View("Index");
                }
               

               ;
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);

            }
            
        }

        public static string RenderPartialToString(string viewName, object model, ControllerContext ControllerContext)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");
            ViewDataDictionary ViewData = new ViewDataDictionary();
            TempDataDictionary TempData = new TempDataDictionary();
            ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }

        }

        [HttpPost]
        public ActionResult Incluir(FornecedorDto model)
        {
            try
            {
                Fornecedor Fornecedor = new Fornecedor();
                var list = Fornecedor.Consultar();
                var filter = list.Where(f => f.cnpj == model.cnpj || f.cpf == model.cpf).ToList();
                if (filter.Count > 0)
                {
                    return Json(new { error = true, msg = "CPF/CNPJ já existe !" });
                }

                if (ModelState.IsValid)
                {
                    Fornecedor.Inserir(model);
                }
                else
                {
                    return Json(new { error = "validation", view = RenderPartialToString("~/Views/Fornecedor/Form.cshtml", model, this.ControllerContext) });
                }
                

                return Json(new { error = false, msg = "Registro Inserido com Sucesso!" });
            }
            catch (Exception ex)
            {

                return Json(new { error = true, msg = ex.Message });
            }  

        }

        [HttpPost]
        public ActionResult Atualizar(FornecedorDto model)
        {
            try
            {
                Fornecedor Fornecedor = new Fornecedor();

                if (ModelState.IsValid)
                {
                    Fornecedor.Atualizar(model);
                } else
                {
                    return Json(new { error = "validation", view = PartialView("Form", model), });
                }

               

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
                Fornecedor Fornecedor = new Fornecedor();

                Fornecedor.Deletar(Convert.ToInt32(id));

                return Json(new { error = false, msg = "Registro Excluido com Sucesso!" });
            }
            catch (Exception ex)
            {

                return Json(new { error = true, msg = "Ocorreu um erro deletar! Verifique as dependencias de empresas !" });
            }
        }

        public ActionResult ConsultaEmpresas(int fornecedor_id)
        {
            try
            {
                Fornecedor Fornecedor = new Fornecedor();
                List<EmpresaDto> list = null;
                ViewBag.Fornecedor = true;
                list =  Fornecedor.ConsultarEmpresas(fornecedor_id);

                return PartialView("~/Views/Empresa/Grid.cshtml",list);
            }
            catch (Exception ex)
            {

                return Json(ex.Message);
            }
            
        }

        public ActionResult SalvarEmpresas(string fornecedor_id, string empresas)
        {
            try
            {
                string[] arr = empresas.Split(',');


                EmpresaFornecedor EmpresaFornecedor = new EmpresaFornecedor();

                foreach (var item in arr)
                {
                    EmpresaFornecedor.Incluir(Convert.ToInt32(item),Convert.ToInt32(fornecedor_id));
                }

              
                return Json(new { error = false, msg = "Empresas Incluidas com Sucesso !" });
            }
            catch (Exception ex)
            {

                return Json(new { error = true, msg = ex.Message });
            }
        }
    }
}