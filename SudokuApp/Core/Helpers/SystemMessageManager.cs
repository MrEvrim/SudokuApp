using SudokuApp.Core.Enums;

namespace SudokuApp.Core.Helpers
{
    public class SystemMessageManager
    {
        public static ResultObject MethodExepcionResultObject(string title, string opc, Exception ex)
        {
            return new ResultObject()
            {
                ResultStatus = ResultStatus.Error,
                Title = title,
                OriginProcess = opc,
                Message = $"{title} - İşlem Hatası",
                Description = ex.Message
            };
        }
        public static ResultObject MethodSuccessResultObject(string title, string opc)
        {
            return new ResultObject()
            {
                ResultStatus = ResultStatus.Success,
                Title = title,
                OriginProcess = opc
            };
        }
        public static ResultObject MethodErrorResultObject(string title, string opc, string message, string descritption = null)
        {
            return new ResultObject()
            {
                ResultStatus = ResultStatus.Error,
                Title = title,
                OriginProcess = opc,
                Message = message,
                Description = descritption
            };
        }
        public static ResultObject MethodResultObjectFromSubMethod(string title, string opc, ResultObject result)
        {
            result.Title = title;
            result.OriginProcess = $"{opc}{Environment.NewLine}{result.OriginProcess}";
            return result;
        }

    }//EOF
}
