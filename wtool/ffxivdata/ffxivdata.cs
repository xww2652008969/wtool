using Lumina.Excel;
using Lumina.Excel.Sheets;
using Lumina.Text.ReadOnly;
using OmenTools;

namespace whook;

public static class Ffxivdata
{
    public static ExcelSheet<World>? Word = DService.Data.GetExcelSheet<World>();


    public static ReadOnlySeString?  Getwordname(uint id)
    {
        return Word?.GetRow(id).Name;
    }
}
