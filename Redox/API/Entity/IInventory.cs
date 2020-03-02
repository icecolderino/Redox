using System.Collections.Generic;


namespace Redox.API.Entity
{
    public interface IInventory
    {
        /// <summary>
        /// List of items
        /// </summary>
        HashSet<IItem> Items { get; }

        /// <summary>
        /// The original redox game inventory ex, UnturnedInventory
        /// </summary>
        object Object { get; }

        int FreeSlots { get; }

        void AddItem(string Name, int amount = 1);

        void AddItemTo(string name, int slot, int amount = 1);

        void RemoveItem(string name, int amount = 1);

        void RemoveItemAt(string name, int slot, int amount = 0);

        void ClearAll();
    }
}
