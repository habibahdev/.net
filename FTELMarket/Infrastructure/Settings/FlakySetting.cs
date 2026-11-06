using System.ComponentModel.DataAnnotations;

namespace FTELMarket.Infrastructure.Settings;

public class FlakySetting
{
    [Required(AllowEmptyStrings =false)]
    public string BaseAddress {get;set;} = string.Empty;
}