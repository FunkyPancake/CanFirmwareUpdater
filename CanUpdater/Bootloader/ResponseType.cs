namespace CanUpdater.Bootloader;

public enum ResponseType
{
    Generic = 0xA0,
    ReadMemory = 0xA3,
    GetProperty = 0xA7
}