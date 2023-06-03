namespace UcabGo.App;

public partial class App : Application
{
    public App()
    {
        try
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
        catch (Exception ex)
        {

        }
    }
}
