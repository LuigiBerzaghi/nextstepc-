using FluentAssertions;
using NextStep.Domain.Entities;
using NextStep.Domain.Enums;

namespace NextStep.Tests.Unit;

public class JourneyDomainTests
{
    [Fact]
    public void RecalculateProgress_ShouldAverageSteps_WhenStepsExist()
    {
        var journey = new Journey
        {
            Steps = new List<JourneyStep>
            {
                new() { Id = 1, Order = 1 },
                new() { Id = 2, Order = 2 }
            }
        };

        journey.Steps.ElementAt(0).UpdateProgress(50);
        journey.Steps.ElementAt(1).UpdateProgress(100);

        journey.RecalculateProgress();

        journey.OverallProgress.Should().Be(75);
        journey.Status.Should().Be(JourneyStatus.Active);
    }

    [Fact]
    public void UpdateProgress_ShouldMarkAsCompleted_WhenProgressIsHundred()
    {
        var step = new JourneyStep();

        step.UpdateProgress(100);

        step.Progress.Should().Be(100);
        step.Status.Should().Be(JourneyStepStatus.Completed);
    }
}
