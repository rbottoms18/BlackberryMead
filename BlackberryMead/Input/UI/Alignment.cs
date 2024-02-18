using System;

namespace BlackberryMead.Input.UI
{
    /// <summary>
    /// Allignment of objects.
    /// </summary>
    [Flags]
    public enum Alignment
    {
        Left = 0,
        Right = 1,
        Center = 2,
        Top = 3,
        Bottom = 4,
        None = 5,
        Vertical = Bottom | Center | Top | None,
        Horizontal = Left | Center | Top | None
    }
}
