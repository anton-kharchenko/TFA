using MediatR;
using TFA.Domain.Models;

namespace TFA.Domain.Queries.GetForum;

public record GetForumQuery() : IRequest<IEnumerable<Forum>>;
