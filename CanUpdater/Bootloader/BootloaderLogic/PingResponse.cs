namespace CanUpdater.Bootloader.BootloaderLogic;

internal record PingResponse(byte Major, byte Minor, byte Bugfix, byte ProtocolName, ushort Options) {
    public PingResponse(IReadOnlyList<byte> bytes) : this(
        Bugfix: bytes[0],
        Minor: bytes[1],
        Major: bytes[2],
        ProtocolName: bytes[3],
        Options: (ushort) (bytes[4] + (bytes[5] << 8))
    ) {
    }
}