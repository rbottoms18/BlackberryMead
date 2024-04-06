namespace BlackberryMead.Framework
{
    /// <summary>
    /// Service that records information about the state of the application.
    /// </summary>
    public interface IGameApplicationService
    {
        /// <summary>
        /// Whether the mouse is visible or not.
        /// </summary>
        bool IsMouseVisible { get; set; }

        /// <summary>
        /// Size of the game window.
        /// </summary>
        Size DisplayDim { get; set; }
    }
}
