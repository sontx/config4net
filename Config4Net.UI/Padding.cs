namespace Config4Net.UI
{
    /// <summary>
    /// Padding between the component and children.
    /// </summary>
    public struct Padding
    {
        /// <summary>
        /// Padding left.
        /// </summary>
        public int Left { get; set; }

        /// <summary>
        /// Padding top.
        /// </summary>
        public int Top { get; set; }

        /// <summary>
        /// Padding right.
        /// </summary>
        public int Right { get; set; }

        /// <summary>
        /// Padding bottom.
        /// </summary>
        public int Bottom { get; set; }

        /// <summary>
        /// -1 if <see cref="Left"/>, <see cref="Top"/>, <see cref="Right"/> and <see cref="Bottom"/> are
        /// not equal. Otherwise, its's one of them.
        /// </summary>
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

        /// <summary>
        /// Creates new padding with default all values are equal.
        /// </summary>
        public Padding(int padding = 0)
        {
            Left = padding;
            Top = padding;
            Right = padding;
            Bottom = padding;
        }

        /// <summary>
        /// Creates new padding with specific <see cref="Left"/>, <see cref="Top"/>, <see cref="Right"/> and <see cref="Bottom"/>
        /// </summary>
        public Padding(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            var value = (Left == Top) && (Right == Bottom) && (Left == Right) ? $"{Left}" : "-1";
            return $"{value}";
        }
    }
}