using SudokuApp.Core.Enums;

namespace SudokuApp.Core.Helpers
{
  public class ShowMessage
    {
        public static void ShowResultMb(ResultObject result)
        {
            string msg = $"Mesaj:{Environment.NewLine}{result.Message}{Environment.NewLine}";
            if (!string.IsNullOrEmpty(result.Description))
                msg += $"{Environment.NewLine}Ayrıntı:{Environment.NewLine}{result.Description}{Environment.NewLine}";
            if (!string.IsNullOrEmpty(result.OriginProcess))
                msg += $"{Environment.NewLine}Kaynak:{Environment.NewLine}{result.OriginProcess}{Environment.NewLine}";

            switch (result.ResultStatus)
            {
                case ResultStatus.Success:
                case ResultStatus.Info:
                    MessageBox.Show(msg, string.IsNullOrEmpty(result.Title) ? "Bilgi" : result.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case ResultStatus.Warning:
                    MessageBox.Show(msg, string.IsNullOrEmpty(result.Title) ? "Uyarı" : result.Title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case ResultStatus.Error:
                    MessageBox.Show(msg, string.IsNullOrEmpty(result.Title) ? "Hata" : result.Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                default:
                    break;
            }
        }
    }
} // EOF
