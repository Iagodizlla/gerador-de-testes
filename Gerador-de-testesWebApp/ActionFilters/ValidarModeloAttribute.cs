﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Gerador_de_testes.WebApp.ActionFilters;

public class ValidarModeloAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        // Lógica ANTES da execução do método de ação
        var modelState = context.ModelState;

        if (!modelState.IsValid)
        {
            var controller = (Controller)context.Controller;

            var viewModel = context.ActionArguments
                .Values
                .FirstOrDefault(x => x is not null && x.GetType().Name.EndsWith("ViewModel"));

            //if(viewModel == null)
            //    context.Result = controller.View("/Home/Index");
            //else
            context.Result = controller.View(viewModel);
        }
    }
}