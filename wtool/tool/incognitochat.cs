using System.Diagnostics;
using System.Runtime.InteropServices;

namespace whook.tool;

public static class incognitochat
{
    [DllImport("kernel32.dll")]
    private static extern IntPtr OpenProcess(uint processAccess, bool bInheritHandle, int processId);

    [DllImport("kernel32.dll")]
    private static extern bool ReadProcessMemory(
        IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint dwSize, out int lpNumberOfBytesRead);

    [DllImport("kernel32.dll")]
    private static extern bool CloseHandle(IntPtr hObject);

    private static IntPtr getbase()
    {
        var processNameToSearch = "ffxiv_dx11";
        byte[] textdata =
            [0x48, 0x83, 0xEC, 0x38, 0x48, 0x8D, 0x4C, 0x24, 0x20, 0xE8, 0x62, 0x03, 0x00, 0x00, 0x89, 0x44];
        var processes = Process.GetProcessesByName(processNameToSearch);
        foreach (var p in processes)
        foreach (ProcessModule m in p.Modules)
            if (m.ModuleName == "ffxiv_dx11.exe")
            {
                var processHandle = OpenProcess(0x0010, false, p.Id); //获取句柄

                var buffer = new byte[16];
                for (var i = 0;; i++)
                    if (ReadProcessMemory(processHandle, m.BaseAddress + i, buffer, 16, out var bytesRead))
                        if (buffer.SequenceEqual(textdata))
                            return m.BaseAddress + i;
            }

        return IntPtr.Zero;
    }
}
