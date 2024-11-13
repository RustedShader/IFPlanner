using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;


namespace IFPlanner.Components.Pages
{
    public partial class Index 
    {
        [Inject] protected NavigationManager navigationManager1 { get; set; }

        private void OnClick()
        {
             navigationManager1.NavigateTo("/create-flight-plan");
        }
    }
}