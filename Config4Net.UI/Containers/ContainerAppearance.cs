namespace Config4Net.UI.Containers
{
    /// <summary>
    /// Container appearance.
    /// </summary>
    public class ContainerAppearance : AppearanceBase
    {
    }

    /// <summary>
    /// Create <see cref="ContainerAppearance"/> instance.
    /// </summary>
    public interface IContainerAppearanceFactory
    {
        /// <summary>
        /// Create <see cref="ContainerAppearance"/> instance.
        /// </summary>
        ContainerAppearance Create();
    }

    /// <inheritdoc />
    public class DefaultContainerAppearanceFactory : IContainerAppearanceFactory
    {
        /// <inheritdoc />
        public ContainerAppearance Create()
        {
            return new ContainerAppearance
            {
                Width = 350
            };
        }
    }
}