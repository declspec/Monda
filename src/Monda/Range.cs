namespace Monda {
    public struct Range {
        public static readonly Range Failure = new Range(-1, -1);

        public readonly int Start;
        public readonly int Length;

        public bool Success => Start >= 0 & Length >= 0;

        public Range(int start, int length) {
            Start = start;
            Length = length;
        }
    }
}
