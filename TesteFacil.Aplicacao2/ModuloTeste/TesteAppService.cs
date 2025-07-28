using FluentResults;
using Microsoft.Extensions.Logging;
using TesteFacil.Aplicacao.Compartilhado;
using TesteFacil.Dominio.Compartilhado;
using TesteFacil.Dominio.ModuloDisciplina;
using TesteFacil.Dominio.ModuloMateria;
using TesteFacil.Dominio.ModuloQuestao;
using TesteFacil.Dominio.ModuloTeste;

namespace TesteFacil.Aplicacao.ModuloTeste;

public class TesteAppService
{
    private readonly IRepositorioTeste repositorioTeste;
    private readonly IRepositorioDisciplina repositorioDisciplina;
    private readonly IRepositorioMateria repositorioMateria;
    private readonly IRepositorioQuestao repositorioQuestao;
    private readonly IUnitOfWork unitOfWork;
    private readonly ILogger<TesteAppService> logger;

    public TesteAppService(
        IRepositorioTeste repositorioTeste,
        IRepositorioDisciplina repositorioDisciplina,
        IRepositorioMateria repositorioMateria,
        IRepositorioQuestao repositorioQuestao,
        IUnitOfWork unitOfWork,
        ILogger<TesteAppService> logger
    )
    {
        this.repositorioTeste = repositorioTeste;
        this.repositorioDisciplina = repositorioDisciplina;
        this.repositorioMateria = repositorioMateria;
        this.repositorioQuestao = repositorioQuestao;
        this.unitOfWork = unitOfWork;
        this.logger = logger;
    }

    public Result Cadastrar(Teste teste)
    {
        var registros = repositorioTeste.SelecionarRegistros();
        if (registros.Any(i => i.Titulo.Equals(teste.Titulo)))
            return Result.Fail(ResultadosErro.RegistroDuplicadoErro("Já existe um teste registrado com este nome."));
        try
        {
            repositorioTeste.Cadastrar(teste);
            unitOfWork.Commit();
            return Result.Ok();
        }
        catch (Exception ex)
        {
            unitOfWork.Rollback();
            logger.LogError(
                ex,
                "Ocorreu um erro durante o registro de {@Registro}.",
                teste
            );
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }

    public Result Editar(Guid id, Teste teste)
    {
        var registros = repositorioTeste.SelecionarRegistros();
        if (registros.Any(i => i.Titulo.Equals(teste.Titulo) && !i.Id.Equals(id)))
            return Result.Fail(ResultadosErro.RegistroDuplicadoErro("Já existe um teste registrado com este nome."));
        try
        {
            repositorioTeste.Editar(id, teste);
            unitOfWork.Commit();
            return Result.Ok();
        }
        catch (Exception ex)
        {
            unitOfWork.Rollback();
            logger.LogError(
                ex,
                "Ocorreu um erro durante a edição de {@Registro}.",
                teste
            );
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }

    public Result Excluir(Guid id)
    {
        var teste = repositorioTeste.SelecionarRegistroPorId(id);

        try
        {
            repositorioTeste.Excluir(id);
            unitOfWork.Commit();
            return Result.Ok();
        }
        catch (Exception ex)
        {
            unitOfWork.Rollback();
            logger.LogError(
                ex,
                "Ocorreu um erro durante a exclusão de {@Registro}.",
                teste
            );
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }

    public Result<Teste> SelecionarRegistroPorId(Guid id)
    {
        try
        {
            var registroSelecionado = repositorioTeste.SelecionarRegistroPorId(id);

            if (registroSelecionado is null)
                return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(id));

            return Result.Ok(registroSelecionado);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Ocorreu um erro durante a seleção do registro {Id}.",
                id
            );

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }

    public Result<List<Teste>> SelecionarRegistros()
    {
        try
        {
            var registros = repositorioTeste.SelecionarRegistros();

            return Result.Ok(registros);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Ocorreu um erro durante a seleção de registros."
            );

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}