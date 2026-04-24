using FluentValidation;
using Klc.Mutabix.Application.Common.Interfaces;
using Klc.Mutabix.Application.Erp.Dtos;
using Klc.Mutabix.Domain.Entities;
using MediatR;

namespace Klc.Mutabix.Application.Erp.Commands;

public record CreateErpConnectionCommand(CreateErpConnectionDto Dto) : IRequest<ErpConnectionDto>;

public class CreateErpConnectionCommandValidator : AbstractValidator<CreateErpConnectionCommand>
{
    public CreateErpConnectionCommandValidator()
    {
        RuleFor(x => x.Dto.CompanyId).GreaterThan(0);
        RuleFor(x => x.Dto.Provider).IsInEnum();
        RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(200);
    }
}

public class CreateErpConnectionCommandHandler(IApplicationDbContext context)
    : IRequestHandler<CreateErpConnectionCommand, ErpConnectionDto>
{
    public async Task<ErpConnectionDto> Handle(
        CreateErpConnectionCommand request, CancellationToken cancellationToken)
    {
        var entity = new ErpConnection
        {
            CompanyId = request.Dto.CompanyId,
            Provider = request.Dto.Provider,
            Name = request.Dto.Name,
            ConnectionString = request.Dto.ConnectionString,
            ApiUrl = request.Dto.ApiUrl,
            ApiKey = request.Dto.ApiKey,
            Username = request.Dto.Username,
            Password = request.Dto.Password,
            ExtraSettings = request.Dto.ExtraSettings
        };

        context.ErpConnections.Add(entity);
        await context.SaveChangesAsync(cancellationToken);

        return new ErpConnectionDto(
            entity.Id, entity.CompanyId, entity.Provider, entity.Name,
            entity.ApiUrl, !string.IsNullOrEmpty(entity.ApiKey),
            entity.Username, entity.LastSyncAt, entity.IsActive, entity.CreatedAt);
    }
}
