using System.ComponentModel.DataAnnotations;

namespace SudokuApp.Core.Enums;

public enum SolutionStatuses : byte
{
  [Display(Description = "Başarısız", Name = "", GroupName = "", ShortName = "", Prompt = "")]
  UnSuccesful = 0,
  [Display(Description = "Başarılı", Name = "", GroupName = "", ShortName = "", Prompt = "")]
  Succesful = 1,
    [Display(Description = "Hatalı", Name = "", GroupName = "", ShortName = "", Prompt = "")]
    False = 3,
    [Display(Description = "Aynı", Name = "", GroupName = "", ShortName = "", Prompt = "")]
    Same = 2,
}
