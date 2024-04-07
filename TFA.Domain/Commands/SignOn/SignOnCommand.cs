using System.Security.Principal;
using MediatR;

namespace TFA.Domain.Commands.SignOn;

public record SignOnCommand(string Login, string Password) : IRequest<IIdentity>, IRequest<Interfaces.Authentication.IIdentity>;