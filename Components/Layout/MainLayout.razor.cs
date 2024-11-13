using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;

namespace IFPlanner.Components.Layout;

public partial class MainLayout
{
    private bool _sidebarExpanded;
    private bool _darkMode = true;
    private readonly string _donationUrl = "https://paypal.me/shubham10297";

    [Inject] protected IJSRuntime JSRuntime { get; set; }

    [Inject] protected NavigationManager NavigationManager { get; set; }

    [Inject] protected DialogService DialogService { get; set; }

    [Inject] protected TooltipService TooltipService { get; set; }

    [Inject] protected ContextMenuService ContextMenuService { get; set; }

    [Inject] protected NotificationService NotificationService { get; set; }

    private void SidebarToggleClick()
    {
        _sidebarExpanded = !_sidebarExpanded;
    }

    private void DarkModeToggleClick()
    {
        _darkMode = !_darkMode;
    }
    
    string theTime;    
    Timer aTimer;

    protected override void OnInitialized()
    {
        aTimer = new Timer(Tick, null, 0, 1000);        
    }

    private void Tick(object _)
    {
        theTime = DateTime.UtcNow.ToLongTimeString();
        InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
        aTimer?.Dispose();
    }
}