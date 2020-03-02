
namespace Redox.API.Entity
{
    public interface IItem
    {
        /// <summary>
        /// The name of the item
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Amount of items
        /// </summary>
        int Amount { get; }

        /// <summary>
        /// The inventory slot
        /// </summary>
        int Slot { get; }

        /// <summary>
        /// Original redox game Item object ex, UnturnedItem
        /// </summary>
        object Object { get; }

        /// <summary>
        /// The inventory of this item
        /// </summary>
        IInventory Inventory { get; }
    }
}
