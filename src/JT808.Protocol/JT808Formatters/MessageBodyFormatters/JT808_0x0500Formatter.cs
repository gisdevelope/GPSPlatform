using JT808.Protocol.Extensions;
using JT808.Protocol.MessageBody;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.Protocol.JT808Formatters.MessageBodyFormatters
{
    public class JT808_0x0500Formatter : IJT808Formatter<JT808_0x0500>
    {
        public JT808_0x0500 Deserialize(ReadOnlySpan<byte> bytes, int offset, IJT808FormatterResolver formatterResolver, out int readSize)
        {
            offset = 0;
            JT808_0x0500 jT808_0X0500 = new JT808_0x0500();
            jT808_0X0500.MsgNum = JT808BinaryExtensions.ReadUInt16Little(bytes, ref offset);
            jT808_0X0500.JT808_0x0200= formatterResolver.GetFormatter<JT808_0x0200>().Deserialize(bytes.Slice(offset), offset, formatterResolver, out readSize);
            readSize = offset;
            return jT808_0X0500;
        }

        public int Serialize(ref byte[] bytes, int offset, JT808_0x0500 value, IJT808FormatterResolver formatterResolver)
        {
            offset += JT808BinaryExtensions.WriteUInt16Little(ref bytes, offset, value.MsgNum);
            offset += formatterResolver.GetFormatter<JT808_0x0200>().Serialize(ref bytes, offset, value.JT808_0x0200, formatterResolver);
            return offset;
        }
    }
}
