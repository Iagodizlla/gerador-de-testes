using FluentResults;
using Microsoft.Extensions.Logging;
using TesteFacil.Aplicacao.Compartilhado;
using TesteFacil.Dominio.Compartilhado;
using TesteFacil.Dominio.ModuloDisciplina;
using TesteFacil.Dominio.ModuloMateria;
using TesteFacil.Dominio.ModuloQuestao;
using TesteFacil.Dominio.ModuloTeste;

namespace TesteFacil.Aplicacao.ModuloMateria;

public class MateriaAppService
{
    private readonly IRepositorioMateria repositorioMateria;
    private readonly IRepositorioDisciplina repositorioDisciplina;
    private readonly IRepositorioQuestao repositorioQuestao;
    private readonly IRepositorioTeste repositorioTeste;
    private readonly IUnitOfWork unitOfWork;
    private readonly ILogger<MateriaAppService> logger;

    public MateriaAppService(
        IRepositorioDisciplina repositorioDisciplina,
        IRepositorioMateria repositorioMateria,
        IRepositorioQuestao repositorioQuestao,
        IRepositorioTeste repositorioTeste,
        IUnitOfWork unitOfWork,
        ILogger<MateriaAppService> logger
)
    {
        this.repositorioDisciplina = repositorioDisciplina;
        this.repositorioMateria = repositorioMateria;

        this.repositorioQuestao = repositorioQuestao;
        this.repositorioTeste = repositorioTeste;

        this.unitOfWork = unitOfWork;

        this.logger = logger;
    }

    public Result Cadastrar(Materia materia)
    {
        var registros = repositorioMateria.SelecionarRegistros();

        if (registros.Any(i => i.Nome.Equals(materia.Nome)))
            return Result.Fail(ResultadosErro.RegistroDuplicadoErro("Já existe uma matéria registrada com este nome."));

        try
        {
            repositorioMateria.Cadastrar(materia);

            unitOfWork.Commit();

            return Result.Ok();
        }
        catch (Exception ex)
        {
            unitOfWork.Rollback();

            logger.LogError(
                ex,
                "Ocorreu um erro durante o registro de {@Registro}.",
                materia
            );

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }

    public Result Editar(Guid id, Materia materiaEditada)
    {
        var registros = repositorioMateria.SelecionarRegistros();

        if (registros.Any(i => !i.Id.Equals(id) && i.Nome.Equals(materiaEditada.Nome)))
            return Result.Fail(ResultadosErro.RegistroDuplicadoErro("Já existe uma matéria registrada com este nome."));

        try
        {
            repositorioMateria.Editar(id, materiaEditada);

            unitOfWork.Commit();

            return Result.Ok();
        }
        catch (Exception ex)
        {
            unitOfWork.Rollback();

            logger.LogError(
                ex,
                "Ocorreu um erro durante a edição do registro {@Registro}.",
                materiaEditada
            );

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }

    public Result Excluir(Guid id)
    {
        try
        {
            var questoes = repositorioQuestao.SelecionarRegistros();

            if (questoes.Any(m => m.Materia.Id.Equals(id)))
            {
                var erro = ResultadosErro
                    .ExclusaoBloqueadaErro("A matéria não pôde ser excluída pois está em uma ou mais questões ativas.");

                return Result.Fail(erro);
            }

            var testes = repositorioTeste.SelecionarRegistros();

            if (testes.Any(t => t.Materia?.Id == id))
            {
                var erro = ResultadosErro
                    .ExclusaoBloqueadaErro("A matéria não pôde ser excluída pois está em um ou mais testes ativo.");

                return Result.Fail(erro);
            }

            repositorioDisciplina.Excluir(id);

            unitOfWork.Commit();

            return Result.Ok();

        }
        catch (Exception ex)
        {
            unitOfWork.Rollback();

            logger.LogError(
                ex,
                "Ocorreu um erro durante a exclusão do registro {Id}.",
                id
            );

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }

    public Result<Materia> SelecionarPorId(Guid id)
    {
        try
        {
            var registroSelecionado = repositorioMateria.SelecionarRegistroPorId(id);

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

    public Result<List<Materia>> SelecionarTodos()
    {
        try
        {
            var registros = repositorioMateria.SelecionarRegistros();

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