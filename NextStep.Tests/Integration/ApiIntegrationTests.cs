using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using NextStep.Application.DTOs.Auth;
using NextStep.Application.DTOs.Chat;

namespace NextStep.Tests.Integration;

public class ApiIntegrationTests : IClassFixture<NextStepApiFactory>
{
    private readonly HttpClient _client;

    public ApiIntegrationTests(NextStepApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Register_ShouldReturnToken_WhenPayloadIsValid()
    {
        var response = await _client.PostAsJsonAsync("/api/v1/auth/register", new
        {
            email = $"tests+{Guid.NewGuid():N}@nextstep.com",
            password = "SenhaForte123!",
            name = "Tester"
        });

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var auth = await response.Content.ReadFromJsonAsync<AuthResponse>();
        auth.Should().NotBeNull();
        auth!.Token.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task JourneyFlow_ShouldCreateAndRetrieveActiveJourney()
    {
        var auth = await RegisterAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth.Token);

        var createResponse = await _client.PostAsJsonAsync("/api/v1/journeys", new
        {
            desiredJob = "AI Strategist",
            currentSkills = new[] { "C#", "Azure" },
            gaps = new[] { "AI Ethics" }
        });

        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = await JsonDocument.ParseAsync(await createResponse.Content.ReadAsStreamAsync());
        created.RootElement.GetProperty("data").GetProperty("desiredJob").GetString().Should().Be("AI Strategist");

        var activeResponse = await _client.GetAsync("/api/v1/journeys/active");
        activeResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var active = await JsonDocument.ParseAsync(await activeResponse.Content.ReadAsStreamAsync());
        active.RootElement.GetProperty("data").GetProperty("overallProgress").GetInt32().Should().Be(0);
    }

    [Fact]
    public async Task Chat_Send_ShouldAcceptToken_OnV2()
    {
        var auth = await RegisterAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth.Token);

        var response = await _client.PostAsJsonAsync("/api/v2/chat/send", new
        {
            conversationId = "aline-mentor-journey",
            message = "Preciso de dicas para migrar para AI Strategy"
        });

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var payload = await response.Content.ReadFromJsonAsync<ChatResponse>();
        payload.Should().NotBeNull();
        payload!.Reply.Message.Should().NotBeNullOrWhiteSpace();
    }

    private async Task<AuthResponse> RegisterAsync()
    {
        var response = await _client.PostAsJsonAsync("/api/v1/auth/register", new
        {
            email = $"integration+{Guid.NewGuid():N}@nextstep.com",
            password = "SenhaForte123!"
        });

        response.EnsureSuccessStatusCode();
        var auth = await response.Content.ReadFromJsonAsync<AuthResponse>();
        return auth!;
    }
}
