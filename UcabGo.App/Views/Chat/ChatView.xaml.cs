using UcabGo.App.ViewModel;

namespace UcabGo.App.Views;

public partial class ChatView : ContentPage
{
    public ChatView(ChatViewModel viewModel)
    {
        InitializeComponent();

        viewModel.ScrollView = scrollview;

        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as ChatViewModel)?.OnAppearing();

        //Scroll to the last message
        scrollview.ScrollToAsync(0, scrollview.Content.Height, false);
    }

}