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
    public class IEEE802_1xTests
    {
        [Fact]
        public void ParsesBasicFields()
        {
            byte[] rawBytes = { 0x01, 0x03, 0x00, 0x75, 0x02, 0x01, 0x0a, 0x00, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x59, 0x16, 0x8b, 0xc3, 0xa5, 0xdf, 0x18, 0xd7, 0x1e, 0xfb, 0x64, 0x23, 0xf3, 0x40, 0x08, 0x8d, 0xab, 0x9e, 0x1b, 0xa2, 0xbb, 0xc5, 0x86, 0x59, 0xe0, 0x7b, 0x37, 0x64, 0xb0, 0xde, 0x85, 0x70, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xd5, 0x35, 0x53, 0x82, 0xb8, 0xa9, 0xb8, 0x06, 0xdc, 0xaf, 0x99, 0xcd, 0xaf, 0x56, 0x4e, 0xb6, 0x00, 0x16, 0x30, 0x14, 0x01, 0x00, 0x00, 0x0f, 0xac, 0x04, 0x01, 0x00, 0x00, 0x0f, 0xac, 0x04, 0x01, 0x00, 0x00, 0x0f, 0xac, 0x02, 0x01, 0x00 };
            IEEE802_1x packet;
            var parseResult = IEEE802_1xFactory.Instance.TryParse(rawBytes, out packet);

            parseResult.Should().BeTrue();     
        }

        [Fact]
        public void SerializesCorrectly()
        {
            var packet =
                IEEE802_1xFactory.Instance.ParseAs(
                    new byte[]
                        { 0x01, 0x03, 0x00, 0x75, 0x02, 0x01, 0x0a, 0x00, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x59, 0x16, 0x8b, 0xc3, 0xa5, 0xdf, 0x18, 0xd7, 0x1e, 0xfb, 0x64, 0x23, 0xf3, 0x40, 0x08, 0x8d, 0xab, 0x9e, 0x1b, 0xa2, 0xbb, 0xc5, 0x86, 0x59, 0xe0, 0x7b, 0x37, 0x64, 0xb0, 0xde, 0x85, 0x70, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xd5, 0x35, 0x53, 0x82, 0xb8, 0xa9, 0xb8, 0x06, 0xdc, 0xaf, 0x99, 0xcd, 0xaf, 0x56, 0x4e, 0xb6, 0x00, 0x16, 0x30, 0x14, 0x01, 0x00, 0x00, 0x0f, 0xac, 0x04, 0x01, 0x00, 0x00, 0x0f, 0xac, 0x04, 0x01, 0x00, 0x00, 0x0f, 0xac, 0x02, 0x01, 0x00 });

            using (var ms = new MemoryStream())
            {
                packet.WriteToStream(ms);
                ms.ToArray()
                    .SequenceEqual(
                        new byte[]
                            { 0x01, 0x03, 0x00, 0x75, 0x02, 0x01, 0x0a, 0x00, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x59, 0x16, 0x8b, 0xc3, 0xa5, 0xdf, 0x18, 0xd7, 0x1e, 0xfb, 0x64, 0x23, 0xf3, 0x40, 0x08, 0x8d, 0xab, 0x9e, 0x1b, 0xa2, 0xbb, 0xc5, 0x86, 0x59, 0xe0, 0x7b, 0x37, 0x64, 0xb0, 0xde, 0x85, 0x70, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xd5, 0x35, 0x53, 0x82, 0xb8, 0xa9, 0xb8, 0x06, 0xdc, 0xaf, 0x99, 0xcd, 0xaf, 0x56, 0x4e, 0xb6, 0x00, 0x16, 0x30, 0x14, 0x01, 0x00, 0x00, 0x0f, 0xac, 0x04, 0x01, 0x00, 0x00, 0x0f, 0xac, 0x04, 0x01, 0x00, 0x00, 0x0f, 0xac, 0x02, 0x01, 0x00 })
                    .Should()
                    .BeTrue();
            }
        }

        [Fact]
        public void CalculatesLength()
        {
            byte[] rawBytes = { 0x01, 0x03, 0x00, 0x75, 0x02, 0x01, 0x0a, 0x00, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x59, 0x16, 0x8b, 0xc3, 0xa5, 0xdf, 0x18, 0xd7, 0x1e, 0xfb, 0x64, 0x23, 0xf3, 0x40, 0x08, 0x8d, 0xab, 0x9e, 0x1b, 0xa2, 0xbb, 0xc5, 0x86, 0x59, 0xe0, 0x7b, 0x37, 0x64, 0xb0, 0xde, 0x85, 0x70, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xd5, 0x35, 0x53, 0x82, 0xb8, 0xa9, 0xb8, 0x06, 0xdc, 0xaf, 0x99, 0xcd, 0xaf, 0x56, 0x4e, 0xb6, 0x00, 0x16, 0x30, 0x14, 0x01, 0x00, 0x00, 0x0f, 0xac, 0x04, 0x01, 0x00, 0x00, 0x0f, 0xac, 0x04, 0x01, 0x00, 0x00, 0x0f, 0xac, 0x02, 0x01, 0x00 };
            IEEE802_1x packet;
            var parseResult = IEEE802_1xFactory.Instance.TryParse(rawBytes, out packet);

            parseResult.Should().BeTrue();

            packet.Length().Should().Be(packet.ToArray().Length);
        }
    }
}
