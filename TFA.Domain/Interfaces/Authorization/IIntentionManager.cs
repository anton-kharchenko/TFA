namespace TFA.Domain.Interfaces.Authorization;

public interface IIntentionManager
{
    bool IsAllowed<TIntention>(TIntention intention) where TIntention : struct;

    bool IsAllowed<TIntention, TTarget>(TIntention intention, TTarget target) where TIntention : struct;
}