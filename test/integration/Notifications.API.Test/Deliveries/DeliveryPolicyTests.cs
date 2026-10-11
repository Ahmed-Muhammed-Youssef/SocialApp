using Notifications.API.Deliveries;
using Notifications.API.Entities;

namespace Notifications.API.Test.Deliveries;

public sealed class DeliveryPolicyTests
{
    private static readonly ChannelOptions Options = new()
    {
        MaxAttempts = 3,
        AckTimeout = TimeSpan.FromSeconds(30),
        BackoffBase = TimeSpan.FromSeconds(5),
        BackoffCap = TimeSpan.FromMinutes(10),
    };

    private static readonly DateTimeOffset Now = new(2026, 1, 1, 12, 0, 0, TimeSpan.Zero);

    [Theory]
    [InlineData(1, 5)]
    [InlineData(2, 10)]
    [InlineData(3, 20)]
    [InlineData(20, 600)] // capped
    public void ComputeDelay_ShouldBackOffExponentially_WithoutJitter(int attempt, int expectedSeconds)
    {
        // jitterSample 0.5 is the midpoint of the ±20% band, i.e. no jitter
        TimeSpan delay = RetryPolicy.ComputeDelay(attempt, Options.BackoffBase, Options.BackoffCap, retryAfter: null, jitterSample: 0.5);

        Assert.Equal(TimeSpan.FromSeconds(expectedSeconds), delay);
    }

    [Theory]
    [InlineData(0.0, 8)]
    [InlineData(0.999999, 12)]
    public void ComputeDelay_ShouldStayWithinJitterBand(double jitterSample, int expectedSeconds)
    {
        TimeSpan delay = RetryPolicy.ComputeDelay(2, Options.BackoffBase, Options.BackoffCap, retryAfter: null, jitterSample);

        Assert.Equal(expectedSeconds, delay.TotalSeconds, precision: 3);
    }

    [Fact]
    public void ComputeDelay_ShouldHonorLongerRetryAfter()
    {
        TimeSpan delay = RetryPolicy.ComputeDelay(1, Options.BackoffBase, Options.BackoffCap, TimeSpan.FromMinutes(2), jitterSample: 0.5);

        Assert.Equal(TimeSpan.FromMinutes(2), delay);
    }

    [Fact]
    public void FromResult_ShouldParkWithoutConsumingAnAttempt_WhenRecipientUnavailable()
    {
        DeliveryTransition transition = DeliveryTransition.FromResult(ChannelResult.RecipientUnavailable, Claimed(attempts: 2), Options, Now);

        Assert.Equal(DeliveryStatus.Parked, transition.Status);
        Assert.Equal(1, transition.Attempts);
    }

    [Fact]
    public void FromResult_ShouldWaitForAck_UntilAckTimeout()
    {
        DeliveryTransition transition = DeliveryTransition.FromResult(ChannelResult.AwaitingAck, Claimed(attempts: 1), Options, Now);

        Assert.Equal(DeliveryStatus.AwaitingAck, transition.Status);
        Assert.Equal(Now + Options.AckTimeout, transition.NextAttemptAt);
    }

    [Fact]
    public void FromResult_ShouldScheduleRetry_OnTransientFailure()
    {
        DeliveryTransition transition = DeliveryTransition.FromResult(ChannelResult.Transient("timeout"), Claimed(attempts: 1), Options, Now);

        Assert.Equal(DeliveryStatus.Pending, transition.Status);
        Assert.InRange(transition.NextAttemptAt, Now + TimeSpan.FromSeconds(4), Now + TimeSpan.FromSeconds(6));
        Assert.Equal("timeout", transition.Error);
    }

    [Fact]
    public void FromResult_ShouldFail_OnTransientFailureAtMaxAttempts()
    {
        DeliveryTransition transition = DeliveryTransition.FromResult(ChannelResult.Transient("timeout"), Claimed(attempts: Options.MaxAttempts), Options, Now);

        Assert.Equal(DeliveryStatus.Failed, transition.Status);
    }

    [Fact]
    public void FromResult_ShouldFailImmediately_OnPermanentFailure()
    {
        DeliveryTransition transition = DeliveryTransition.FromResult(ChannelResult.Permanent("invalid token"), Claimed(attempts: 1), Options, Now);

        Assert.Equal(DeliveryStatus.Failed, transition.Status);
        Assert.Equal("invalid token", transition.Error);
    }

    private static Delivery Claimed(int attempts) => new()
    {
        EventId = "event",
        RecipientId = "user",
        Channel = "signalr",
        Status = DeliveryStatus.Processing,
        Attempts = attempts,
    };
}
