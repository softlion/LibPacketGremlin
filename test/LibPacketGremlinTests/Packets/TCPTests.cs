﻿namespace LibPacketGremlinTests.Packets
{
    using System.IO;
    using System.Linq;

    using FluentAssertions;

    using OutbreakLabs.LibPacketGremlin.Packets;
    using OutbreakLabs.LibPacketGremlin.Extensions;

    using Xunit;
    using OutbreakLabs.LibPacketGremlin.PacketFactories;
    public class TCPTests
    {
        [Fact]
        public void ParsesBasicFields()
        {
            byte[] rawBytes = {0x04, 0x0a
                , 0x14, 0x46, 0x45, 0x0e, 0x6e, 0xc8, 0xfb, 0xcd, 0x4f, 0xcc, 0x50, 0x18
                , 0xfa, 0x4e, 0x1b, 0x5e, 0x00, 0x00, 0x17, 0x03, 0x01, 0x00, 0x20, 0x0f
                , 0x52, 0xba, 0x8d, 0xda, 0x74, 0x2a, 0xd7, 0x3d, 0x59, 0x2f, 0x22, 0x5e
                , 0xcb, 0xde, 0xa7, 0xcf, 0xeb, 0x46, 0xd4, 0x88, 0x48, 0x48, 0x91, 0xda
                , 0xb9, 0xd8, 0xd7, 0x52, 0xef, 0x36, 0x2a};

            TCP packet;
            var parseResult = TCPFactory.Instance.TryParse(rawBytes, out packet);

            parseResult.Should().BeTrue();

            packet.SourcePort.Should().Be(1034);
            packet.DestPort.Should().Be(5190);
            packet.SeqNumber.Should().Be(1158573768U);
            packet.AckNumber.Should().Be(4224536524U);
            ((int)packet.DataOffset).Should().Be(5);

            packet.CongestionWindowReduced.Should().BeFalse();
            packet.ECN_Echo.Should().BeFalse();
            packet.Urgent.Should().BeFalse();
            packet.Ack.Should().BeTrue();
            packet.Push.Should().BeTrue();
            packet.Reset.Should().BeFalse();
            packet.Syn.Should().BeFalse();
            packet.Fin.Should().BeFalse();
            packet.WindowSize.Should().Be(64078);
            packet.Checksum.Should().Be(0x1b5e);
            packet.UrgentPointer.Should().Be(0);
        }

        [Fact]
        public void SerializesCorrectly()
        {
            TCP packet;
            TCPFactory.Instance.TryParse(new byte[] {0x04, 0x0a
                , 0x14, 0x46, 0x45, 0x0e, 0x6e, 0xc8, 0xfb, 0xcd, 0x4f, 0xcc, 0x50, 0x18
                , 0xfa, 0x4e, 0x1b, 0x5e, 0x00, 0x00, 0x17, 0x03, 0x01, 0x00, 0x20, 0x0f
                , 0x52, 0xba, 0x8d, 0xda, 0x74, 0x2a, 0xd7, 0x3d, 0x59, 0x2f, 0x22, 0x5e
                , 0xcb, 0xde, 0xa7, 0xcf, 0xeb, 0x46, 0xd4, 0x88, 0x48, 0x48, 0x91, 0xda
                , 0xb9, 0xd8, 0xd7, 0x52, 0xef, 0x36, 0x2a}, out packet).Should().BeTrue();


            using (var ms = new MemoryStream())
            {
                packet.WriteToStream(ms);
                ms.ToArray()
                    .SequenceEqual(
                        new byte[]
                            {0x04, 0x0a
                , 0x14, 0x46, 0x45, 0x0e, 0x6e, 0xc8, 0xfb, 0xcd, 0x4f, 0xcc, 0x50, 0x18
                , 0xfa, 0x4e, 0x1b, 0x5e, 0x00, 0x00, 0x17, 0x03, 0x01, 0x00, 0x20, 0x0f
                , 0x52, 0xba, 0x8d, 0xda, 0x74, 0x2a, 0xd7, 0x3d, 0x59, 0x2f, 0x22, 0x5e
                , 0xcb, 0xde, 0xa7, 0xcf, 0xeb, 0x46, 0xd4, 0x88, 0x48, 0x48, 0x91, 0xda
                , 0xb9, 0xd8, 0xd7, 0x52, 0xef, 0x36, 0x2a})
                    .Should()
                    .BeTrue();
            }
        }

        [Fact]
        public void CalculatesChecksum()
        {            
            IPv4 packet;
            IPv4Factory.Instance.TryParse(new byte[]{ 0x45, 0x00,0x00, 0x29,0x10, 0xe9,
0x40, 0x00, 0x80, 0x06, 0x00, 0x00, 0x7f, 0x00,
0x00, 0x01, 0x7f, 0x00, 0x00, 0x01, 0xd4, 0x68,
0xd4, 0x69, 0xb6, 0x08, 0xff, 0x50, 0x01, 0x1e,
0x37, 0xd9, 0x50, 0x18, 0x01, 0x00, 0xf8, 0xa5,
0x00, 0x00, 0x21 }, out packet).Should().BeTrue();

            var e2 = EthernetIIFactory.Instance.Default(packet);
            var tcp = packet.Layer<TCP>();
            var correctChecksum = tcp.Checksum;
            e2.CorrectFields();
            tcp.Checksum.Should().Be(correctChecksum);
        }

        [Fact]
        public void CalculatesLength()
        {
            byte[] rawBytes = {0x04, 0x0a
                , 0x14, 0x46, 0x45, 0x0e, 0x6e, 0xc8, 0xfb, 0xcd, 0x4f, 0xcc, 0x50, 0x18
                , 0xfa, 0x4e, 0x1b, 0x5e, 0x00, 0x00, 0x17, 0x03, 0x01, 0x00, 0x20, 0x0f
                , 0x52, 0xba, 0x8d, 0xda, 0x74, 0x2a, 0xd7, 0x3d, 0x59, 0x2f, 0x22, 0x5e
                , 0xcb, 0xde, 0xa7, 0xcf, 0xeb, 0x46, 0xd4, 0x88, 0x48, 0x48, 0x91, 0xda
                , 0xb9, 0xd8, 0xd7, 0x52, 0xef, 0x36, 0x2a};

            TCP packet;
            var parseResult = TCPFactory.Instance.TryParse(rawBytes, out packet);

            parseResult.Should().BeTrue();

            packet.Length().Should().Be(packet.ToArray().Length);
        }
    }
}
