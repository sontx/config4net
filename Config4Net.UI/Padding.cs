namespace Config4Net.UI
{
    public struct Padding
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public int Right { get; set; }
        public int Bottom { get; set; }
        public int All
        {
            get => (Left == Top) && (Right == Bottom) && (Left == Right) ? Left : -1;
            set
            {
                Left = value;
                Top = value;
                Right = value;
                Bottom = value;
            }
        }

        public Padding(int padding = 0)
        {
            Left = padding;
            Top = padding;
            Right = padding;
            Bottom = padding;
        }

        public Padding(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public override string ToString()
        {
            var value = (Left == Top) && (Right == Bottom) && (Left == Right) ? $"{Left}" : "-1";
            return $"{value}";
        }
    }
}