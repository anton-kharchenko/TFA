using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using TFA.Search.API.Grpc;

namespace TFA.Search.API.Services;

internal class SearchEngineGrpcService() : SearchEngine.SearchEngineBase
{
    public override Task<Empty> Index(IndexRequest request, ServerCallContext context)
    {
        return base.Index(request, context);
    }

    public override Task<SearchResponse> Search(SearchRequest request, ServerCallContext context)
    {
        return base.Search(request, context);
    }
}