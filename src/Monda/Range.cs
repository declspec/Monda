namespace Monda {
    /// <summary>
    /// Represents start and length properties that define a range in contiguous data
    /// </summary>
    public struct Range {
        public static readonly Range Failure = new Range(-1, -1);

        public int Start { get; }
        public int Length { get; }

        public Range(int start, int length) {
            Start = start;
            Length = length;
        }
    }
}
