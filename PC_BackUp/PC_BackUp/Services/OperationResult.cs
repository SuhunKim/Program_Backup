namespace PC_BackUp.Services;

// [Codex - 2026.10.19] 취소와 부분 완료를 문자열 해석 없이 구별한다. 부분 완료는 완전 성공이 아니다.
public enum OperationStatus { Success, PartialSuccess, Canceled, Failed }

public sealed record OperationResult(bool Succeeded, string Message)
{
    public string OperationId { get; init; } = Guid.NewGuid().ToString("N");
    public IReadOnlyList<OperationFileError> Errors { get; init; } = Array.Empty<OperationFileError>();
    public OperationStatus Status { get; init; } = Succeeded ? OperationStatus.Success : OperationStatus.Failed;
    public static OperationResult Success(string message) => new(true, message);
    public static OperationResult Failure(string message) => new(false, message);
    public static OperationResult Failure(string message, Exception exception) => new(false, message)
    {
        Errors = (exception is AggregateException aggregate ? aggregate.Flatten().InnerExceptions : new[] { exception }.AsEnumerable())
            .Select(error => new OperationFileError(error is FileNotFoundException missing ? missing.FileName ?? string.Empty :
                error.Data["Path"]?.ToString() ?? string.Empty, error.GetType().FullName ?? error.GetType().Name, error.ToString())).ToList()
    };
    public static OperationResult Canceled(string message) => new(false, message) { Status = OperationStatus.Canceled };
    public static OperationResult Partial(string message) => new(false, message) { Status = OperationStatus.PartialSuccess };
}

public sealed record OperationFileError(string Path, string ExceptionType, string Detail);
