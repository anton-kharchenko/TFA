namespace TFA.Forums.Domain.Exceptions;

public class TopicNotFoundException(Guid topicId) : DomainException(410, $"Topic with id {topicId} not found.");