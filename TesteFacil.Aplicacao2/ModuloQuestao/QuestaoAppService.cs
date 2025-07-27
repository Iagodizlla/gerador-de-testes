using FluentResults;
using Microsoft.Extensions.Logging;
using TesteFacil.Aplicacao.Compartilhado;
using TesteFacil.Dominio.Compartilhado;
using TesteFacil.Dominio.ModuloDisciplina;
using TesteFacil.Dominio.ModuloMateria;
using TesteFacil.Dominio.ModuloQuestao;
using TesteFacil.Dominio.ModuloTeste;
using static System.Net.Mime.MediaTypeNames;

namespace TesteFacil.Aplicacao.ModuloQuestao;

public class QuestaoAppService
{
    private readonly IRepositorioQuestao repositorioQuestao;
    private readonly IRepositorioMateria repositorioMateria;
    private readonly IRepositorioTeste repositorioTeste;
    private readonly IUnitOfWork unitOfWork;
    private readonly ILogger<QuestaoAppService> logger;

    public QuestaoAppService(
        IRepositorioQuestao repositorioQuestao,
        IRepositorioMateria repositorioMateria,
        IRepositorioTeste repositorioTeste,
        IUnitOfWork unitOfWork,
        ILogger<QuestaoAppService> logger
    )
    {
        this.repositorioQuestao = repositorioQuestao;
        this.repositorioMateria = repositorioMateria;
        this.repositorioTeste = repositorioTeste;

        this.unitOfWork = unitOfWork;
        this.logger = logger;
    }

    public Result Cadastrar(Questao questao)
    {
        var registros = repositorioQuestao.SelecionarRegistros();
        if (registros.Any(i => i.Enunciado.Equals(questao.Enunciado)))
            return Result.Fail(ResultadosErro.RegistroDuplicadoErro("Já existe uma questão registrada com este enunciado."));
        try
        {
            repositorioQuestao.Cadastrar(questao);
            unitOfWork.Commit();
            return Result.Ok();
        }
        catch (Exception ex)
        {
            unitOfWork.Rollback();
            logger.LogError(
                ex,
                "Ocorreu um erro durante o registro de {@Registro}.",
                questao
            );
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }

    public Result Editar(Guid id, Questao questao)
    {
        var registros = repositorioQuestao.SelecionarRegistros();
        if (registros.Any(i => i.Enunciado.Equals(questao.Enunciado) && i.Id != questao.Id))
            return Result.Fail(ResultadosErro.RegistroDuplicadoErro("Já existe uma questão registrada com este enunciado."));
        try
        {
            repositorioQuestao.Editar(id, questao);
            unitOfWork.Commit();
            return Result.Ok();
        }
        catch (Exception ex)
        {
            unitOfWork.Rollback();
            logger.LogError(
                ex,
                "Ocorreu um erro durante a atualização de {@Registro}.",
                questao
            );
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }

    public Result Excluir(Guid id)
    {
        var questao = repositorioQuestao.SelecionarRegistroPorId(id);
        try
        {
            var testes = repositorioTeste.SelecionarRegistros();

            if (testes.Any(t => t.Questoes.Any(q => q.Id.Equals(id))))
            {
                var erro = ResultadosErro
                    .ExclusaoBloqueadaErro("A questão não pôde ser excluída pois está em um ou mais testes ativos.");
                return Result.Fail(erro);
            }

            repositorioQuestao.Excluir(id);

            unitOfWork.Commit();

            return Result.Ok();
        }
        catch (Exception ex)
        {
            unitOfWork.Rollback();
            logger.LogError(
                ex,
                "Ocorreu um erro durante a exclusão de {Id}.",
                id
            );
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }

    public Result<Questao> SelecionarRegistroPorId(Guid id)
    {
        try
        {
            var registroSelecionado = repositorioQuestao.SelecionarRegistroPorId(id);

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

    public Result<List<Questao>> SelecionarRegistros()
    {
        try
        {
            var registros = repositorioQuestao.SelecionarRegistros();

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