﻿using JT808.Protocol.Extensions;
using JT808.Protocol.MessageBody;
using JT808.Protocol.MessageBody;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.Protocol.JT808Formatters.MessageBodyFormatters
{
    public class JT808_0x0201Formatter : IJT808Formatter<JT808_0x0201>
    {
        public JT808_0x0201 Deserialize(ReadOnlySpan<byte> bytes, int offset, IJT808FormatterResolver formatterResolver, out int readSize)
        {
            offset = 0;
            JT808_0x0201 jT808_0X0201 = new JT808_0x0201();
            jT808_0X0201.MsgNum=JT808BinaryExtensions.ReadUInt16Little(bytes, ref offset);
            jT808_0X0201.Position=formatterResolver.GetFormatter<JT808_0x0200>().Deserialize(bytes.Slice(offset), offset, formatterResolver, out readSize);
            readSize = offset;
            return jT808_0X0201;
        }

        public int Serialize(ref byte[] bytes, int offset, JT808_0x0201 value, IJT808FormatterResolver formatterResolver)
        {
            offset += JT808BinaryExtensions.WriteUInt16Little(ref bytes, offset, value.MsgNum);
            offset += formatterResolver.GetFormatter<JT808_0x0200>().Serialize(ref bytes, offset, value.Position, formatterResolver);
            return offset;
        }
    }
}
