using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace maui_empty_datatemplate_problem;

public class CustomViewItem
{
	public string Name { get; set; }
}

public partial class MainPage : ContentPage, INotifyPropertyChanged
{
	private int _index = 0;
	
	private bool _canShow = false;
	private bool _isLoading = true;
	private bool _hideCollectionWhileLoading = false;
	public bool CanShow
	{
		get => _canShow;
		set => SetProperty(ref this._canShow, value);
	}

	public bool IsLoading
	{
		get => _isLoading;
		set => SetProperty(ref this._isLoading, value);
	}
	public bool HideCollectionWhileLoading
	{
		get => _hideCollectionWhileLoading;
		set => SetProperty(ref this._hideCollectionWhileLoading, value);
	}

	public ObservableCollection<CustomViewItem> FilteredItems
	{
		get;
		init;
	} = new ObservableCollection<CustomViewItem>();

	public Command RefreshCommand
	{
		get;
		init;
	}
	
	public bool AddEmptyNameFirst
	{
		get;
		set;
	}

	public MainPage()
	{
		this.RefreshCommand = new Command(() => this.RefreshCommandHandler());
		this.BindingContext = this;
		InitializeComponent();
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		_ = FillList();
	}

	private async Task RefreshCommandHandler()
	{
		await this.FillList();
	}

	private async Task FillList()
	{
		if (this.HideCollectionWhileLoading)
		{
			this.CanShow = false;
			this.IsLoading = true;
		}

		this.FilteredItems.Clear();
		await Task.Delay(250);

		if (this.AddEmptyNameFirst)
		{
			this.FilteredItems.Add(new CustomViewItem { Name = "" });
		}

		// The Index is to give impression of refresh
		var firstIndex = this._index;
		while(this._index < firstIndex + 5)
		{
			this.FilteredItems.Add(new CustomViewItem { Name = $"Item {this._index}" });
			this._index++;
		}

		this.CanShow = true;
		this.IsLoading = false;
	}

	private bool SetProperty<T>([NotNullIfNotNull("newValue")] ref T field, T newValue, [CallerMemberName] string? propertyName = null)
	{
		if (EqualityComparer<T>.Default.Equals(field, newValue))
		{
			return false;
		}

		OnPropertyChanging(propertyName);
		field = newValue;
		OnPropertyChanged(propertyName);
		return true;
	}
}

