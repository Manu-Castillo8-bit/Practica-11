using Todosucarnet.Data;
using Todosucarnet.Models;

namespace Todosucarnet.Views;

[QueryProperty("Item", "Item")]

public partial class TodoItemPage : ContentPage
{
	TodoItem item;

	public TodoItem Item
	{
		get => BindingContext as TodoItem;
		set => BindingContext = value;
	}

	TodoItemDatabase database;
	public TodoItemPage(TodoItemDatabase todoItemData)
	{
		InitializeComponent();
		database = todoItemData;
	}


async void OnSaveClicked(object sender, EventArgs e)
	{
		if (string.IsNullOrWhiteSpace(Item.Name))
		{
			await DisplayAlert("Nombre requerido", "Por favor ingresa un nombre para el todo item.", "OK");
			return;
		}
		await database.SaveItemAsync(Item);
		await Shell.Current.GoToAsync("..");
	}

	async void OnDeleteClicked(object sender, EventArgs e)
	{
		if (Item.ID == 0)
			return;
		await database.DeleteItemAsync(Item);
		await Shell.Current.GoToAsync("..");
    }

	async void OnCancelClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}
