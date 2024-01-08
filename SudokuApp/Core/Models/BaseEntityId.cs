using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SudokuApp.Core.Models;

public class BaseEntityId
{
    [Description("K.No")]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [ScaffoldColumn(false)]
    public int Id { get; set; }
}




