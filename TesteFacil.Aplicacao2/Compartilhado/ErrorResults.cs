using FluentResults;

namespace TesteFacil.Aplicacao.Compartilhado;

public abstract class ResultadosErro
{
    public static Error RegistroDuplicadoErro(string mensagemErro)
    {
        return new Error("Registro duplicado")
            .CausedBy(mensagemErro)
            .WithMetadata("TipoErro", "RegistroDuplicado");
    }

    public static Error RegistroNaoEncontradoErro(Guid id)
    {
        return new Error("Registro não encontrado")
            .CausedBy("Não foi possível obter o registro com o ID: " + id)
            .WithMetadata("TipoErro", "RegistroNaoEncontrado");
    }

    public static Error ExclusaoBloqueadaErro(string mensagemErro)
    {
        return new Error("Exclusão bloqueada")
            .CausedBy(mensagemErro)
            .WithMetadata("TipoErro", "ExclusaoBloqueada");
    }


    public static Error ExcecaoInternaErro(Exception ex)
    {
        return new Error("Ocorreu um erro interno do servidor")
            .CausedBy(ex)
            .WithMetadata("TipoErro", "ExcecaoInterna");
    }
}