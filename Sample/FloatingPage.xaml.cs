using Reactive.Bindings;

namespace Sample;

public partial class FloatingPage : ContentPage
{
    public ReactiveCommandSlim NextCommand { get; set; } = new ReactiveCommandSlim();
    public ReactiveCommandSlim FabCommand { get; set; } = new();
    public ReactivePropertySlim<bool> Visible { get; set; } = new ReactivePropertySlim<bool>(true);
    public ReactivePropertySlim<bool> Visible1 { get; set; } = new ReactivePropertySlim<bool>(false);
    public ReactivePropertySlim<bool> Visible2 { get; set; } = new ReactivePropertySlim<bool>(false);
    public ReactivePropertySlim<bool> Visible3 { get; set; } = new ReactivePropertySlim<bool>(false);
    public ReactivePropertySlim<bool> Visible4 { get; set; } = new ReactivePropertySlim<bool>(false);



    List<ReactivePropertySlim<bool>> _visibles = new List<ReactivePropertySlim<bool>>();

    public FloatingPage()
    {
        InitializeComponent();

        BindingContext = this;

        _visibles.Add(Visible);
        _visibles.Add(Visible1);
        _visibles.Add(Visible2);
        _visibles.Add(Visible3);
        _visibles.Add(Visible4);

        NextCommand.Subscribe(_ =>
        {
            var index = _visibles.FindIndex(x => x.Value == true);
            _visibles[index].Value = false;
            index++;
            if (index == _visibles.Count)
            {
                index = 0;
            }

            _visibles[index].Value = true;
        });

        FabCommand.Subscribe(_ =>
        {
            DisplayAlert("", "FabTap", "OK");
        });
    }

    void Handle_Clicked(object sender, System.EventArgs e)
    {
        DisplayAlert("", "BaseTap", "OK");
    }

    void GreenTap(object sender, System.EventArgs e)
    {
        DisplayAlert("", "GreenTap", "OK");
    }

    void BlueTap(object sender, System.EventArgs e)
    {
        DisplayAlert("", "BlueTap", "OK");
    }

    void RedTap(object sender, System.EventArgs e)
    {
        DisplayAlert("", "RedTap", "OK");
    }

    void LimeTap(object sender, System.EventArgs e)
    {
        DisplayAlert("", "LimeTap", "OK");
    }

    void NavyTap(object sender, System.EventArgs e)
    {
        DisplayAlert("", "NavyTap", "OK");
    }

    void PinkTap(object sender, System.EventArgs e)
    {
        DisplayAlert("", "PinkTap", "OK");
    }
}
