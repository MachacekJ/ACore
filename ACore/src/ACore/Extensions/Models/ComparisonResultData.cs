namespace ACore.Extensions.Models;
public record ComparisonResultData(string Name, Type Type, bool IsChange, object? LeftValue, object? RightValue);