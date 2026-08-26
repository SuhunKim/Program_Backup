using System.Text;

namespace PC_BackUp;

/// <summary>
/// 외부 패키지 없이 문자 단위 LCS(최장 공통 부분열)로 두 문자열의 차이를 구하는 작은 유틸리티.
/// 그리드에서 "현재"/"백업" 값 중 실제로 다른 부분만 색으로 강조하는 데 쓴다.
/// </summary>
internal static class TextDiff
{
    internal enum SegmentKind { Equal, RemovedFromLeft, AddedInRight }

    internal readonly record struct Segment(string Text, SegmentKind Kind);

    /// <summary>left="현재" 값, right="백업" 값 기준으로 구간을 나눈다.</summary>
    internal static IReadOnlyList<Segment> Compute(string? left, string? right)
    {
        left ??= string.Empty;
        right ??= string.Empty;

        // lengths[i, j] = left[i..]와 right[j..]의 최장 공통 부분열 길이 (뒤에서부터 채운다).
        var lengths = new int[left.Length + 1, right.Length + 1];
        for (var i = left.Length - 1; i >= 0; i--)
        {
            for (var j = right.Length - 1; j >= 0; j--)
            {
                lengths[i, j] = left[i] == right[j]
                    ? lengths[i + 1, j + 1] + 1
                    : Math.Max(lengths[i + 1, j], lengths[i, j + 1]);
            }
        }

        var segments = new List<Segment>();
        var buffer = new StringBuilder();
        var bufferKind = SegmentKind.Equal;

        void Append(SegmentKind kind, char character)
        {
            if (buffer.Length > 0 && bufferKind != kind)
                Flush();
            bufferKind = kind;
            buffer.Append(character);
        }

        void Flush()
        {
            if (buffer.Length == 0) return;
            segments.Add(new Segment(buffer.ToString(), bufferKind));
            buffer.Clear();
        }

        var x = 0;
        var y = 0;
        while (x < left.Length && y < right.Length)
        {
            if (left[x] == right[y])
            {
                Append(SegmentKind.Equal, left[x]);
                x++; y++;
            }
            else if (lengths[x + 1, y] >= lengths[x, y + 1])
            {
                Append(SegmentKind.RemovedFromLeft, left[x]);
                x++;
            }
            else
            {
                Append(SegmentKind.AddedInRight, right[y]);
                y++;
            }
        }
        while (x < left.Length) { Append(SegmentKind.RemovedFromLeft, left[x]); x++; }
        while (y < right.Length) { Append(SegmentKind.AddedInRight, right[y]); y++; }
        Flush();

        return segments;
    }
}
