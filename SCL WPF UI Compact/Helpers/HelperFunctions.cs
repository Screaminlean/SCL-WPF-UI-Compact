// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
using System.Reflection;

namespace SCL_WPF_UI_Compact.Helpers
{
    public static class HelperFunctions
    {
        public static string AppName = "SCL WPF UI Compact";

        public static string GetAssemblyVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version?.ToString()
                ?? String.Empty;
        }
    }
}
