using System.Security.Principal;
using MediatR;

namespace TFA.Domain.Commands.SignIn;

public record SignInCommand(string Login, string Password) : IRequest<(Interfaces.Authentication.IIdentity identity, string token)>;