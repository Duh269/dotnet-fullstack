class Fornecedor {

    IdSelected;
    empresas;

   static HideModalConfirm() {
        $("#confmexclusao").modal('hide');
    }

    HideModalEmpresas() {
        $("#confEmpresas").modal('hide');
    }

    SalvarEmpresas() {
        var arrEmpresas = Generic.getArraySelected("gridempresas", "selected_primary_grid");
        var arrFornecedor = Generic.getArraySelected("datalist", "selected_primary_grid");
        if (arrEmpresas.length > 0) {

            $.ajax({
                url: "/Fornecedor/SalvarEmpresas", data: { fornecedor_id: arrFornecedor[0], empresas: arrEmpresas.toString() }, type: "POST", success: function (result) {

                    
                }
            });

        }
    }

    ShowModalEmpresas() {
        
        Empresa.Consultar("", "gridempresas");
        var arrEmpresas = Generic.getArraySelected("datalist_dependency", "selected_primary_grid")
        this.empresas = arrEmpresas;
        $("#confEmpresas").modal('show');
        
    }

    ShowModalConfirm() {
        $("#InformMsg").css("display", "none");
        $("#InformMsg").html('');
        $("#confmexclusao").modal('show');
    
    }

   

    Excluir() {

        var arr = Generic.getArraySelected("datalist","selected_primary_grid");

        if (arr.length == 0) {
            return;
        } else if (arr.length > 1) {
            return;
        }

        $.ajax({
            url: "/Fornecedor/Excluir", data: { id: arr[0] }, type: "POST", success: function (result) {

                $("#InformMsg").html(result.msg);
                $("#InformMsg").css('display', 'block');
                if (result.error == false) {
                    $("#InformMsg").addClass("alert-success");
                    Fornecedor.HideModalConfirm();
                    Fornecedor.Consultar("");
                } else {
                    $("#InformMsg").addClass("alert-danger");
                }
            }
        });

    }


 


  static Consultar(texto) {
    
      $.ajax({
          url: "/Fornecedor/Consultar",data: { busca: texto }, type: "POST", success: function (result) {
            $("#datalist").html(result);
        }
       });
    }

    IncluirShow() {
        Generic.clearForm("formedit");
        $("#dataedit").show();
        $("#fornecedor_id").val('0');
        $("#InformMsg").html('');
        $("#InformMsg").css('display', 'none');
        $("#cnpjorcpf").prop("disabled", false);
    }
    IncluirHide() {
        $("#dataedit").hide();
        $("#InformMsg").html('');
        $("#InformMsg").css('display', 'none');
    }

    EditarShow() {
        this.IdSelected = $(".selected_primary_grid:checked").val();
        $("#InformMsg").html('');
        $("#InformMsg").css('display', 'none');
        Generic.clearForm("formedit");
        GetData(); // pega dados do grid carregado
        $("#cnpjorcpf").prop("disabled", true);
        $("#dataedit").show();
        
    }

   

    Salvar(IdForm) {

        var fornecedor_id = parseInt($("#fornecedor_id").val());
        var action = "";
        if (fornecedor_id == 0) {
            action = "Incluir";
        }

        if (fornecedor_id != 0) {
            action = "Atualizar";
        }

        $.ajax({
            url: "/Fornecedor/" + action, type: "POST", data: $("#" + IdForm).serialize(), success: function (result) {
                
                if (result.error == false) {
                    $("#InformMsg").addClass("alert-success");

                    Fornecedor.Consultar("");

                    $("#InformMsg").html(result.msg);
                    $("#InformMsg").css('display', 'block');
                    $("#dataedit").hide();

                    
                } else {
                    $("#InformMsg").addClass("alert-danger");
                    $("#InformMsg").html(result.msg);
                    $("#InformMsg").css('display', 'block');
                    $("#dataedit").hide();
                }
                
            }
        });
    }

    static ConsultarEmpresas(fornecedor) {
        $.ajax({
            url: "/Fornecedor/ConsultaEmpresas", type: "POST", data: { fornecedor_id: fornecedor }, success: function (result) {
                $("#datalist_dependency").html(result)
            }
        });
    }

     ChangeGrid() {
        $(".selected_primary_grid").change(function () {
            
            if ($(this).is(":checked") == true) {
                $("#clients_dependecy").show();
               Fornecedor.ConsultarEmpresas($(this).val());
            } else {
                $("#clients_dependecy").hide();
                //IncluirHide();
            }

        })
    }

    CEP() {

        $("#cep").change(function () {
            var cep = $(this).val();
            var dataNascimento = $("#data_nascimento").val();

            if (cep == "") {
                return;
            }


            $.ajax({
                url: "https://cdn.apicep.com/file/apicep/" + cep +".json", dataType: 'json', type: "POST", success: function (data) {
                    console.log(data);
                    if (data.uf == "PR" && dataNascimento != "") {
                        var dtNascimento = new Date(dataNascimento);
                        var interval = Date.now().getFullYear() - dtNascimento.getFullYear();
                        if (interval < 18) {
                            $("#msgValidaton").show();
                            $("#msgValidaton").html('Cadastro não permitido');
                        }
                    }
                }
            });
        })

    }

    KeyPressBusca(value) {
        this.Consultar($.trim(value));
    }

    
}


class Generic {
    static getArraySelected(IdDiv, selector) {
        var arr = new Array();
        $("#" + IdDiv + " ." + selector).each(function () {
            if ($(this).is(":checked") == true) {
                arr.push($(this).val());
            }
        })

        return arr;
    }

    static clearForm(idForm) {
        $("#" + idForm + " input[type='text']").val('');
        $("#" + idForm + " input[type='email']").val('');
        $("#" + idForm + " input[type='date']").val('');
    }
}

class Empresa {

    IdSelected;

    ChangeGrid() {
        $(".selected_primary_grid").change(function () {
            //ajax
            if ($(this).is(":checked") == true) {
                $("#fornecedores_dependecy").show();
            } else {
                $("#fornecedores_dependecy").hide();
            }

        })
    }

   static Consultar(texto,idDiv) {
        $.ajax({
            url: "/Empresa/Consultar", data: { busca : texto }, type: "POST", success: function (result) {
                $("#" + idDiv).html(result);
            }
        });
    }

    IncluirShow() {
        Generic.clearForm("formedit");
        $("#empresa_id").val('0');
        $("#cnpj").prop("disabled", false);
        $("#dataedit").show();
    }

   static HideModalConfirm() {
        $("#confmexclusao").modal('hide');
    }

    ShowModalConfirm() {
        $("#InformMsg").css("display", "none");
        $("#InformMsg").html('');
        $("#confmexclusao").modal('show');

    }

    Excluir() {
        var arr = Generic.getArraySelected("datalist", "selected_primary_grid");

        if (arr.length == 0) {
            return;
        } else if (arr.length > 1) {
            return;
        }

        $.ajax({
            url: "/Empresa/Excluir", data: { id: arr[0] }, type: "POST", success: function (result) {

                $("#InformMsg").html(result.msg);
                $("#InformMsg").css('display', 'block');
                if (result.error == false) {
                    $("#InformMsg").addClass("alert-success");
                    Empresa.HideModalConfirm();
                    Empresa.Consultar("","datalist");
                } else {
                    $("#InformMsg").addClass("alert-danger");
                }
            }
        });
    }

    IncluirHide() {
        $("#dataedit").hide();
    }

    UnMask() {
        $("#cnpj").unmask();
        $("#cep").unmask();
    }

    Salvar(IdForm) {

        //this.UnMask();
        var empresa_id = parseInt($("#empresa_id").val());
      
        var action = "";
        if (empresa_id == 0) {
            action = "Incluir";
        } 

        if (empresa_id != 0) {
            action = "Alterar";
        } 

        $.ajax({
            url: "/Empresa/" + action, type: "POST", data: $("#" + IdForm).serialize(), success: function (result) {
                $("#InformMsg").html(result.msg);
                $("#InformMsg").css('display', 'block');
                $("#dataedit").hide();
                if (result.error == false) {
                    $("#InformMsg").addClass("alert-success");

                    Empresa.Consultar("","datalist");
                } else {
                    $("#InformMsg").addClass("alert-danger");
                }
            }
        });
    }

    EditarShow() {
        this.IdSelected = Generic.getArraySelected("datalist", "selected_primary_grid")[0];
        Generic.clearForm("formedit");
        GetDataEmpresa();
        $("#cnpj").prop("disabled", true);
        $("#dataedit").show();
        
    }
}

