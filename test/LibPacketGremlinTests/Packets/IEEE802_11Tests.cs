﻿namespace LibPacketGremlinTests.Packets
{
    using System.IO;
    using System.Linq;

    using FluentAssertions;

    using OutbreakLabs.LibPacketGremlin.Packets;
    using OutbreakLabs.LibPacketGremlin.Packets.IEEE802_11Support;
    using OutbreakLabs.LibPacketGremlin.PacketFactories;

    using Xunit;
    using OutbreakLabs.LibPacketGremlin.Extensions;
    public class IEEE802_11Tests
    {
        [Fact]
        public void ParsesBasicFields()
        {
            byte[] rawBytes = {0x88,0x02,0x3a,0x01,0x00,0x22,0xfa,0xa2,0x35,0x46,0x00,0x25,0x9c,0xdc,0x6e,0xb2,0x00,0x25,0x9c,0xdc,0x6e,0xb2,0x00,0x00,0x00,0x00
            ,0xaa,0xaa,0x03,0x00,0x00,0x00,0x88,0x8e,
            0x01,0x03,0x00,0x5f,0xfe,0x00,0x89,0x00,0x20,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x02,0x56,0xd2,0x74,0x0a,0x5f,0x6c,0x02,0xa9,0xa8,0xf2,0x0b,0xdf,0xfe,0xe6,0xfc,0x16,0x96,0xa3,0x90,0x27,0x73,0x35,0x7f,0xbc,0x00,0x5c,0x42,0x25,0x1b,0xc0,0xf8,0x66,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00};
            IEEE802_11 packet;
            var parseResult = IEEE802_11Factory.Instance.TryParse(rawBytes, out packet);

            parseResult.Should().BeTrue();
            packet.FrameType.Should().Be((int)FrameTypes.Data);
            packet.SubType.Should().Be((int)DataSubTypes.QoS);
            packet.ProtocolVersion.Should().Be(0);
            packet.ToDS.Should().BeFalse();
            packet.FromDS.Should().BeTrue();
            packet.MoreFrag.Should().BeFalse();
            packet.Retry.Should().BeFalse();
            packet.PowerMgt.Should().BeFalse();
            packet.MoreData.Should().BeFalse();
            packet.Protected.Should().BeFalse();
            packet.Order.Should().BeFalse();
            packet.DurationID.Should().Be(314);
            packet.Destination.SequenceEqual(new byte[] { 0x00, 0x22, 0xfa, 0xa2, 0x35, 0x46 }).Should().BeTrue();
            packet.BSSID.SequenceEqual(new byte[] { 0x00, 0x25, 0x9c, 0xdc, 0x6e, 0xb2 }).Should().BeTrue();
            packet.Source.SequenceEqual(new byte[] { 0x00, 0x25, 0x9c, 0xdc, 0x6e, 0xb2 }).Should().BeTrue();

        }

        [Fact]
        public void SerializesCorrectly()
        {
            var packet =
                IEEE802_11Factory.Instance.ParseAs(
                    new byte[]
                        {
                            0x88, 0x02, 0x3a, 0x01, 0x00, 0x22, 0xfa, 0xa2, 0x35, 0x46, 0x00, 0x25, 0x9c, 0xdc, 0x6e, 0xb2,
                            0x00, 0x25, 0x9c, 0xdc, 0x6e, 0xb2, 0x00, 0x00, 0x00, 0x00, 0xaa, 0xaa, 0x03, 0x00, 0x00, 0x00,
                            0x88, 0x8e, 0x01, 0x03, 0x00, 0x5f, 0xfe, 0x00, 0x89, 0x00, 0x20, 0x00, 0x00, 0x00, 0x00, 0x00,
                            0x00, 0x00, 0x02, 0x56, 0xd2, 0x74, 0x0a, 0x5f, 0x6c, 0x02, 0xa9, 0xa8, 0xf2, 0x0b, 0xdf, 0xfe,
                            0xe6, 0xfc, 0x16, 0x96, 0xa3, 0x90, 0x27, 0x73, 0x35, 0x7f, 0xbc, 0x00, 0x5c, 0x42, 0x25, 0x1b,
                            0xc0, 0xf8, 0x66, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x00, 0x00
                        });

            using (var ms = new MemoryStream())
            {
                packet.WriteToStream(ms);
                ms.ToArray()
                    .SequenceEqual(
                        new byte[]
                            {0x88,0x02,0x3a,0x01,0x00,0x22,0xfa,0xa2,0x35,0x46,0x00,0x25,0x9c,0xdc,0x6e,0xb2,0x00,0x25,0x9c,0xdc,0x6e,0xb2,0x00,0x00,0x00,0x00
            ,0xaa,0xaa,0x03,0x00,0x00,0x00,0x88,0x8e,
            0x01,0x03,0x00,0x5f,0xfe,0x00,0x89,0x00,0x20,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x02,0x56,0xd2,0x74,0x0a,0x5f,0x6c,0x02,0xa9,0xa8,0xf2,0x0b,0xdf,0xfe,0xe6,0xfc,0x16,0x96,0xa3,0x90,0x27,0x73,0x35,0x7f,0xbc,0x00,0x5c,0x42,0x25,0x1b,0xc0,0xf8,0x66,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00})
                    .Should()
                    .BeTrue();
            }
        }
    }
}