namespace Monda {
    public ref struct Match {
        public readonly int Start;
        public readonly int Length;

        public bool Success => Start >= 0 & Length >= 0;

        public Match(int start, int length) {
            Start = start;
            Length = length;
        }
    }
}
