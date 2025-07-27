using FluentResults;
using Microsoft.AspNetCore.Mvc;
using TesteFacil.WebApp.Models;

namespace TesteFacil.WebApp.Extensions;

public static class ControllerExtensions
{
    public static IActionResult RedirecionarParaNotificacao(this Controller controller, Result resultado)
    {
        foreach (var error in resultado.Errors)
        {
            var notificacaoJson = NotificacaoViewModel.GerarNotificacaoSerializada(
                error.Message,
                error.Reasons[0].Message
            );

            controller.TempData.Add(nameof(NotificacaoViewModel), notificacaoJson);
        }

        return controller.RedirectToAction("Index");
    }

    public static IActionResult PreencherErrosModelState(this Controller controller, Result resultado, object viewModel)
    {
        foreach (var erro in resultado.Errors)
        {
            if (erro.Metadata["TipoErro"].ToString() == "RegistroDuplicado")
            {
                controller.ModelState.AddModelError("CadastroUnico", erro.Reasons[0].Message);
            }
        }

        return controller.View(viewModel);
    }
}