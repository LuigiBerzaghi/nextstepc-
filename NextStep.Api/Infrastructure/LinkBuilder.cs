using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace NextStep.Api.Infrastructure;

public interface ILinkBuilder
{
    IDictionary<string, string?> BuildPaginationLinks(HttpContext context, string actionName, string controllerName, int pageNumber, int pageSize, int totalPages, object? additionalRouteValues = null);
}

public class LinkBuilder : ILinkBuilder
{
    private readonly LinkGenerator _linkGenerator;

    public LinkBuilder(LinkGenerator linkGenerator)
    {
        _linkGenerator = linkGenerator;
    }

    public IDictionary<string, string?> BuildPaginationLinks(HttpContext context, string actionName, string controllerName, int pageNumber, int pageSize, int totalPages, object? additionalRouteValues = null)
    {
        var apiVersion = context.GetRequestedApiVersion()?.ToString() ?? "1.0";
        object MergeRouteValues(int targetPage)
        {
            var baseValues = new RouteValueDictionary(additionalRouteValues ?? new { });
            baseValues["version"] = apiVersion;
            baseValues["pageNumber"] = targetPage;
            baseValues["pageSize"] = pageSize;
            return baseValues;
        }

        var self = _linkGenerator.GetUriByAction(context, action: actionName, controller: controllerName, values: MergeRouteValues(pageNumber));
        var next = pageNumber < totalPages
            ? _linkGenerator.GetUriByAction(context, actionName, controllerName, MergeRouteValues(pageNumber + 1))
            : null;
        var prev = pageNumber > 1
            ? _linkGenerator.GetUriByAction(context, actionName, controllerName, MergeRouteValues(pageNumber - 1))
            : null;

        return new Dictionary<string, string?>
        {
            ["self"] = self,
            ["next"] = next,
            ["prev"] = prev
        };
    }
}
