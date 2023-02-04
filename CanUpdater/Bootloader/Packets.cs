using System.Runtime.Intrinsics.Arm;

namespace CanUpdater.Bootloader;

public class Packets
{
    private ITransportProtocol _tp;

    private bool Ping()
    {
        var cmd = new byte[] {0x5A, 0xA6};
        _tp.Send(cmd);
        byte[] bytes = _tp.GetBytes(10);
        if (!(bytes[0] == 0x5a && bytes[1] == 0xA7))
        {
            return false;
        }

        var x = new PingResponse(Major: bytes[4],
                Minor: bytes[3],
                Bugfix: bytes[2],
                ProtocolName: bytes[5],
                Options: (ushort) (bytes[6] + (bytes[7] << 8)),
                Crc: (ushort) (bytes[8] + (bytes[9] << 8)));
        CheckCrc(x.Crc,bytes[2..7]);
    }

    private void FramingPacket(byte[] command)
    {
        var cmd = new byte[] {0x5A};
    }

    private bool CheckCrc(ushort crc, byte[] data)
    {
        return crc == CalcCrc(data);
    }

    private short CalcCrc(byte[] data)
    {
        throw new NotImplementedException();
    }
}

