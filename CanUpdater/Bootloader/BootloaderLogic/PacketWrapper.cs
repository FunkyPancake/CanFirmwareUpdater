using Serilog;

namespace CanUpdater.Bootloader.BootloaderLogic;

internal class PacketWrapper {
    private readonly ILogger _logger;
    private const byte StartByte = 0x5A;

    public PacketWrapper(ILogger logger) {
        _logger = logger;
    }

    public static byte[] BuildFramingPacket(PacketType packet, byte[]? payload = null) {
        var x = new List<byte> {
            StartByte, (byte) packet
        };
        if (payload is null) {
            return x.ToArray();
        }

        var len = payload.Length;
        x.AddRange(new byte[] {(byte) (len & 0xff), (byte) ((len >> 8) & 0xff), 0, 0});
        x.AddRange(payload);
        var crc = CalcCrc(x);
        x[4] = (byte) (crc & 0xff);
        x[5] = (byte) ((crc >> 8) & 0xff);

        return x.ToArray();
    }

    public static bool ParseResponse(byte[] bytes, out byte[] payload) {
        payload = Array.Empty<byte>();
        if (bytes[0] != StartByte) {
            // _logger.Error("Start byte not equal to {}, value: {}", StartByte, bytes[0]);
            return false;
        }

        switch ((ResponseType) bytes[1]) {
            case ResponseType.Generic:
                break;
            case ResponseType.ReadMemory:
                break;
            case ResponseType.GetProperty:
                break;
            default:
                throw new ArgumentOutOfRangeException("");
        }

        return false;
    }

    public static bool ParsePingResponse(byte[] bytes, out byte[] payload) {
        payload = Array.Empty<byte>();
        if (bytes.Length != 10 || bytes[0] != StartByte || bytes[1] != (byte)PacketType.PingResponse) {
            return false;
        }

        payload = bytes[2..8];
        var crc = bytes[8] + (bytes[9] << 8);
        return crc == CalcCrc(bytes[..8]);
    }
    


    private static ushort CalcCrc(IReadOnlyList<byte> packet) {
        uint crc = 0;
        uint j;
        for (j = 0; j < packet.Count; ++j) {
            uint i;
            uint b = packet[(int) j];
            crc ^= b << 8;
            for (i = 0; i < 8; ++i) {
                var temp = crc << 1;
                if ((crc & 0x8000) == 0x8000) {
                    temp ^= 0x1021;
                }

                crc = temp;
            }
        }

        return (ushort) crc;
    }
}