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

            var dialog = await _dialog.ShowAsync<DeleteDialog>("Confirm action", parameters);
            var result = await dialog.Result;
            
            return !result.Canceled;
        }
    }
}
