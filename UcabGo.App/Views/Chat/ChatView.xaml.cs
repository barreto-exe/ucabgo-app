using UcabGo.App.ViewModel;

namespace UcabGo.App.Views;

public partial class ChatView : ContentPage
{
    public ChatView(ChatViewModel viewModel)
    {
        InitializeComponent();

        viewModel.CollectionView = collectionView;

        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as ChatViewModel)?.OnAppearing();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        (BindingContext as ChatViewModel)?.OnDisappearing();
    }
}