using SudokuApp.Core.Enums;

namespace SudokuApp.Core.Helpers
{
    public class ResultObject
    {
        public ResultStatus ResultStatus { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Description { get; set; }
        public string OriginProcess { get; set; }

        // public dynamic DynamicResultData { get; set; }
    }
}
