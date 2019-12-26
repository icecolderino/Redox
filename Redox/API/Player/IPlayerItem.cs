
namespace Redox.API.Player
{
    public interface IPlayerItem
    {
        /// <summary>
        /// The name of the item
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The amount of the item
        /// </summary>
        int Amount { get; set; }

        /// <summary>
        /// The slot of the item
        /// </summary>
        int Slot { get; set; }

        /// <summary>
        /// Orignal item object
        /// </summary>
        object Object { get; }

    }
}
