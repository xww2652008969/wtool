using Lumina.Excel;
using Lumina.Excel.GeneratedSheets;
using Lumina.Text;
using OmenTools;

namespace whook;

public class ffxivdata
{
    public static ExcelSheet<World>? Word = DService.Data.GetExcelSheet<World>();


    public static  SeString Getwordname(uint id)
    {
        return Word.GetRow(id).Name;
    }
} 
