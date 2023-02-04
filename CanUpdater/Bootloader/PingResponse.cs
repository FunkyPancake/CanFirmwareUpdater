namespace CanUpdater.Bootloader;

public record PingResponse(byte Major, byte Minor, byte Bugfix, byte ProtocolName, ushort Options, ushort Crc);