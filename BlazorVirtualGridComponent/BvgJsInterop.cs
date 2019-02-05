using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace BlazorVirtualGridComponent
{
    public class BvgJsInterop
    {
        public static Task<string> Prompt(string message)
        {
            // Implemented in exampleJsInterop.js
            return JSRuntime.Current.InvokeAsync<string>(
                "exampleJsFunctions.showPrompt",
                message);
        }
    }
}