﻿using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using TFA.Search.API.Grpc;
using TFA.Search.Domain.Commands;
using TFA.Search.Domain.Queries;

namespace TFA.Search.API.Services;

internal class SearchEngineGrpcService(ISender mediator) : SearchEngine.SearchEngineBase
{
    public override async Task<Empty> Index(IndexRequest request, ServerCallContext context)
    {
        var command = new IndexCommand(Guid.Parse(request.Id),
            request.Type switch
            {
                SearchEntityType.Unknown => Domain.Enums.SearchEntityType.ForumTopic,
                SearchEntityType.ForumTopic => Domain.Enums.SearchEntityType.ForumTopic,
                SearchEntityType.ForumComment => Domain.Enums.SearchEntityType.ForumComment,
                _ => throw new ArgumentOutOfRangeException()
            },
            request.Title,
            request.Text
        );
        
        await mediator.Send(command);
        
        return new Empty();
    }

    public override async Task<SearchResponse> Search(SearchRequest request, ServerCallContext context)
    {
        var query = new SearchQuery(request.Query);

        var (searchResponses, totalCount) = await mediator.Send(query);

        return new SearchResponse
        {
            Total = totalCount,
            Entities =
            {
                searchResponses.Select(r => new SearchResponse.Types.SearchResultEntity
                {
                    Id = r.EntityId.ToString(),
                    Type = r.SearchEntityType switch {
                        Domain.Enums.SearchEntityType.ForumTopic => SearchEntityType.ForumTopic,
                        Domain.Enums.SearchEntityType.ForumComment => SearchEntityType.ForumComment,
                        _ => SearchEntityType.Unknown
                    }
                })
            }
        };
    }
}