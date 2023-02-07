using Microsoft.AspNetCore.Components;
using MudBlazor;
using SQuiz.Client.Shared.Components.Dialogs;

namespace SQuiz.Client.Services
{
    public class CrudDialogService
    {
        private readonly IDialogService _dialog;

        public CrudDialogService(IDialogService dialog)
        {
            _dialog = dialog;
        }

        public async Task<bool> AskToDeleteItem(string itemName, string itemType = "item")
        {
            var parameters = new DialogParameters()
            {
                ["ItemType"] = itemType,
                ["ItemName"] = itemName
            };

            var dialog = await _dialog.ShowAsync<DeleteDialog>($"Add {itemType}", parameters);
            var result = await dialog.Result;
            
            return !result.Canceled;
        }

        public async Task AskToAddItem<TItem, TTemplate>(Func<TItem, Task> onAdded)
            where TItem: class
            where TTemplate: ComponentBase
        {   
            var dialog = await _dialog.ShowAsync<TTemplate>("Confirm action");
            var result = await dialog.Result;

            if (result.Data is TItem item)
            {
                await onAdded(item);
            }
        }
    }
}
