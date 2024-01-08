using SudokuApp.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace SudokuApp.Core.Models;

public class SolutionHead : BaseEntityId
{
  [Description("Toplam Çözüm Süresi Sn")]
  [Required]
  public double TotalTime { get; set; } // saniye

  [Description("Başlangıç Zamanı")]
  [Required]
  public DateTime StartTime { get; set; } // başlama zamanı

  [Description("Durum")]
  [Required]
  [ScaffoldColumn(false)]
  public SolutionStatuses Status { get; set; }

  [Description("Durum")]
  [NotMapped]
  public string? StatusDescription { get; set; }

  [Description("Deneme Döngüsü")]
  [Required]
  public uint LoopTryCount { get; set; }

  [Description("Toplam Döngü")]
  [Required]
  public uint LoopTotalCount { get; set; }

  [Description("Sayaç")]
  [Required]
  public uint FinishTotalCount { get; set; }

    [Description("Key")]
    [Required]
    public string Key { get; set; }

    [Description("Description")]
    public string? Description { get; set; }


    [Description("Veriler")]
  [NotMapped]
  [ScaffoldColumn(false)]
  public virtual List<SolutionItem> SolutionItems { get; set; }
}

public class SolutionItem : BaseEntityId
{
  // satırno, sütunno, hücreno, değer, sırano, HeadId

  [Description("Çözüm Id")]
  [Required]
  [ScaffoldColumn(false)]
  public int SolutionHeadId { get; set; }

  [Description("Çözüm")]
  [NotMapped]
  [ScaffoldColumn(false)]
  public virtual SolutionHead SolutionHead { get; set; }

  [Description("Hücre No")]
  [Required]
  public byte CellNumber { get; set; }

  [Description("Satır No")]
  [Required]
  public byte RowNumber { get; set; }

  [Description("Sütun No")]
  [Required]
  public byte ColumnNumber { get; set; }

  [Description("Grup No")]
  [Required]
  public byte GroupNumber { get; set; }

  [Description("Değer")]
  [Required]
  public byte Value { get; set; }
}