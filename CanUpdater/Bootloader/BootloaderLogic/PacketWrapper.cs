using System.Data;

namespace CanUpdater.Bootloader.BootloaderLogic;

internal static class PacketWrapper {
    private const byte StartByte = 0x5A;

    public static byte[] BuildFramingPacket(PacketType packetType, byte[]? payload = null) {
        var header = new List<byte> {
            StartByte, (byte) packetType
        };
        if (payload is null) {
            return header.ToArray();
        }

        var len = payload.Length;
        header.AddRange(new[] {(byte) (len & 0xff), (byte) ((len >> 8) & 0xff)});
        var dataForCrc = header.Concat(payload).ToArray();
        var crc = CalcCrc(dataForCrc);
        var packet = new byte[6 + payload.Length];
        packet[4] = (byte) (crc & 0xff);
        packet[5] = (byte) ((crc >> 8) & 0xff);
        header.CopyTo(packet, 0);
        payload.CopyTo(packet, 6);
        return packet.ToArray();
    }

    public static byte[] BuildCommandPacket(Command commandType, byte flag, uint[] parameters) {
        var len = parameters.Length;
        if (len > 7)
            throw new ArgumentException("Parameters array larger than 7");
        var commandPacket = new byte[4 + 4 * len];
        var header = new byte[] {(byte) commandType, flag, 0, (byte) len};
        header.CopyTo(commandPacket, 0);
        for (var i = 0; i < len; i++) {
            var bytes = BitConverter.GetBytes(parameters[i]);
            bytes.CopyTo(commandPacket, 4 * i + 4);
        }

        return BuildFramingPacket(PacketType.Command, commandPacket);
    }


    public static bool ParseResponse(byte[] bytes, out byte[] payload) {
        payload = Array.Empty<byte>();
        if (bytes[0] != StartByte) {
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
                throw new ArgumentOutOfRangeException();
        }

        return false;
    }

    public static bool ParsePingResponse(byte[] bytes, out byte[] response) {
        response = Array.Empty<byte>();
        if (bytes.Length != 10 || bytes[0] != StartByte || bytes[1] != (byte) PacketType.PingResponse) {
            return false;
        }

        response = bytes[2..8];
        var crc = bytes[8] + (bytes[9] << 8);
        return crc == CalcCrc(bytes[..8]);
    }

    public static bool ParseAck(byte[] bytes) {
        return bytes[0] == StartByte && bytes[1] == (byte) PacketType.Ack;
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