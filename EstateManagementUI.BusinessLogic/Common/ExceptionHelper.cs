using System.Text;

namespace EstateManagementUI.BusinessLogic.Common;

public static class ExceptionHelper
{
    public static string GetCombinedExceptionMessages(this Exception ex)
    {
        StringBuilder sb = new StringBuilder();
        AppendExceptionMessages(ex, sb);
        return sb.ToString();
    }

    private static void AppendExceptionMessages(Exception ex, StringBuilder sb)
    {
        if (ex == null) return;

        sb.AppendLine(ex.Message);

        if (ex.InnerException != null)
        {
            AppendExceptionMessages(ex.InnerException, sb);
        }
    }
}