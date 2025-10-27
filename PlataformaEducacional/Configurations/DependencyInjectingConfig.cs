using MediatR;
using PlataformaEducacional.Api.Extensions;
using PlataformaEducacional.Core.Communication.Mediator;
using PlataformaEducacional.Core.DomainObjects;
using PlataformaEducacional.Core.Messages.CommonMessages.IntegrationEvents;
using PlataformaEducacional.Core.Messages.CommonMessages.Notifications;
using PlataformaEducacional.GestaoAluno.Application.Commands.AdicionarAluno;
using PlataformaEducacional.GestaoAluno.Application.Commands.CancelarMatricula;
using PlataformaEducacional.GestaoAluno.Application.Commands.FinalizarCurso;
using PlataformaEducacional.GestaoAluno.Application.Commands.FinalizarMatricula;
using PlataformaEducacional.GestaoAluno.Application.Commands.GerarCertificado;
using PlataformaEducacional.GestaoAluno.Application.Commands.MatricularAlunoCurso;
using PlataformaEducacional.GestaoAluno.Application.Commands.PagamentoMatricula;
using PlataformaEducacional.GestaoAluno.Application.Commands.RealizarAula;
using PlataformaEducacional.GestaoAluno.Application.Events;
using PlataformaEducacional.GestaoAluno.Application.Queries;
using PlataformaEducacional.GestaoAluno.Data.Repository;
using PlataformaEducacional.GestaoAluno.Domain.Repositories;
using PlataformaEducacional.GestaoConteudo.Application.Commands;
using PlataformaEducacional.GestaoConteudo.Application.Queries;
using PlataformaEducacional.GestaoConteudo.Data.Repository;
using PlataformaEducacional.GestaoConteudo.Domain;
using PlataformaEducacional.GestaoFinanceira.AntiCorruption;
using PlataformaEducacional.GestaoFinanceira.Business;
using PlataformaEducacional.GestaoFinanceira.Business.Events;
using PlataformaEducacional.GestaoFinanceira.Data;
using PlataformaEducacional.GestaoFinanceira.Data.Repository;
using Financeira = PlataformaEducacional.GestaoFinanceira;

namespace PlataformaEducacional.Configurations
{
    public static class DependencyInjectingConfig
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            // Mediator
            services.AddMediatR(
               typeof(AdicionarAulaCommand).Assembly,
               typeof(CursoCommandHandler).Assembly
           );

            services.AddScoped<IMediatorHandler, MediatorHandler>();

            // Notifications
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

            // Gestão de Conteúdos
            services.AddScoped<ICursoRepository, CursoRepository>();
            services.AddScoped<ICursoQueries, CursoQueries>();

            services.AddScoped<IRequestHandler<AdicionarAulaCommand, bool>, CursoCommandHandler>();
            services.AddScoped<IRequestHandler<AdicionarCursoCommand, bool>, CursoCommandHandler>();
            services.AddScoped<IRequestHandler<AtualizarCursoCommand, bool>, CursoCommandHandler>();

            // Gestão de Alunos
            services.AddScoped<IAlunoRepository, AlunoRepository>();
            services.AddScoped<IAlunoQueries, AlunoQueries>();

            services.AddScoped<IRequestHandler<AdicionarAlunoCommand, bool>, AdicionarAlunoCommadHandler>();
            services.AddScoped<IRequestHandler<FinalizarMatriculaCommand, bool>, FinalizarMatriculaHandler>();
            services.AddScoped<IRequestHandler<MatricularAlunoCursoCommand, bool>, MatricularAlunoCursoHandler>();
            services.AddScoped<IRequestHandler<PagamentoMatriculaCommand, bool>, PagamentoMatriculaHandler>();
            services.AddScoped<IRequestHandler<CancelarMatriculaCommand, bool>, CancelarMatriculaCommandHandler>();
            services.AddScoped<IRequestHandler<RealizarAulaCommand, bool>, RealizarAulaCommandHandler>();
            services.AddScoped<IRequestHandler<FinalizarCursoCommand, bool>, FinalizarCursoCommandHandler>();
            services.AddScoped<IRequestHandler<GerarCertificadoCommand, bool>, GerarCertificadoCommandHandler>();

            services.AddScoped<INotificationHandler<MatriculaPagamentoRealizadoEvent>, MatriculaEventHandler>();
            services.AddScoped<INotificationHandler<MatriculaPagamentoRecusadoEvent>, MatriculaEventHandler>();
            services.AddScoped<INotificationHandler<CursoFinalizadoEvent>, MatriculaEventHandler>();

            // Pagamento
            services.AddScoped<IPagamentoRepository, PagamentoRepository>();
            services.AddScoped<IPagamentoService, PagamentoService>();
            services.AddScoped<IPagamentoCartaoCreditoFacade, PagamentoCartaoCreditoFacade>();
            services.AddScoped<IPayPalGateway, PayPalGateway>();
            services.AddScoped<Financeira.IConfigurationManager, Financeira.ConfigurationManager>();
            services.AddScoped<INotificationHandler<IniciaPagamentoEvent>, PagamentoEventHandler>();
            services.AddScoped<PagamentoContext>();

            services.AddScoped<IAppIdentityUser, AppIdentityUser>();
            return services;
        }
    }
}
