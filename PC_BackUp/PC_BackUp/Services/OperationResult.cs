namespace PC_BackUp.Services;

// [Codex - 2026.10.19] 취소와 부분 완료를 문자열 해석 없이 구별한다. 부분 완료는 완전 성공이 아니다.
public enum OperationStatus { Success, PartialSuccess, Canceled, Failed }

public sealed record OperationResult(bool Succeeded, string Message)
{
    public OperationStatus Status { get; init; } = Succeeded ? OperationStatus.Success : OperationStatus.Failed;
    public static OperationResult Success(string message) => new(true, message);
    public static OperationResult Failure(string message) => new(false, message);
    public static OperationResult Canceled(string message) => new(false, message) { Status = OperationStatus.Canceled };
    public static OperationResult Partial(string message) => new(false, message) { Status = OperationStatus.PartialSuccess };
}
