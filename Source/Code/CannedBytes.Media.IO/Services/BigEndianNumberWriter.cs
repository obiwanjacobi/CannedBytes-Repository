using System;
using System.ComponentModel.Composition;
using System.IO;

namespace CannedBytes.Media.IO.Services
{
    /// <summary>
    /// Implements the <see cref="INumberWriter"/> interface for a big-endian encoding.
    /// </summary>
    [Export(typeof(INumberWriter))]
    public class BigEndianNumberWriter : INumberWriter
    {
        /// <inheritdocs/>
        public void WriteInt16(short value, Stream stream)
        {
            Throw.IfArgumentNull(stream, "stream");

            WriteUInt16((ushort)value, stream);
        }

        /// <inheritdocs/>
        public void WriteInt16(int value, Stream stream)
        {
            Throw.IfArgumentNull(stream, "stream");

            WriteUInt16((ushort)value, stream);
        }

        /// <inheritdocs/>
        public void WriteInt32(int value, Stream stream)
        {
            Throw.IfArgumentNull(stream, "stream");

            WriteUInt32((uint)value, stream);
        }

        /// <inheritdocs/>
        public void WriteInt32(long value, Stream stream)
        {
            Throw.IfArgumentNull(stream, "stream");

            WriteUInt32((uint)value, stream);
        }

        /// <inheritdocs/>
        public void WriteInt64(long value, Stream stream)
        {
            Throw.IfArgumentNull(stream, "stream");

            byte[] buffer = BitConverter.GetBytes(value);
            Array.Reverse(buffer);

            stream.Write(buffer, 0, buffer.Length);
        }

        /// <inheritdocs/>
        private void WriteUInt16(UInt16 value, Stream stream)
        {
            Throw.IfArgumentNull(stream, "stream");

            byte[] buffer = BitConverter.GetBytes(value);
            Array.Reverse(buffer);

            stream.Write(buffer, 0, buffer.Length);
        }

        /// <inheritdocs/>
        private void WriteUInt32(UInt32 value, Stream stream)
        {
            Throw.IfArgumentNull(stream, "stream");

            byte[] buffer = BitConverter.GetBytes(value);
            Array.Reverse(buffer);

            stream.Write(buffer, 0, buffer.Length);
        }
    }
}