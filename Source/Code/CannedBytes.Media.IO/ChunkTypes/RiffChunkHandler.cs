﻿namespace CannedBytes.Media.IO.ChunkTypes
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using CannedBytes.Media.IO.SchemaAttributes;

    /// <summary>
    /// Handles serialization of a RIFF chunk.
    /// </summary>
    [FileChunkHandler("RIFF")]
    public class RiffChunkHandler : DefaultFileChunkHandler
    {
        /// <inheritdocs/>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Check is not recognized")]
        public override object Read(ChunkFileContext context)
        {
            // create instance and read type
            var riffChunk = base.Read(context) as RiffChunk;

            if (riffChunk == null)
            {
                throw new InvalidOperationException();
            }

            // read child chunk of 'type'
            var reader = context.CompositionContainer.GetService<FileChunkReader>();
            var stream = reader.CurrentStream;

            riffChunk.InnerChunk = reader.ReadRuntimeContainerChunkType(stream, riffChunk.FileType);

            // the 4CC FileType was not found as a runtime chunk type.
            if (riffChunk.InnerChunk == null)
            {
                // normally we'd skip an unknown chunk but at the root its no use.
                var msg = String.Format(
                          CultureInfo.InvariantCulture,
                          "The Four Character Code '{0}' specified in the RIFF (root) chunk could not be mapped to a CLR Type.",
                          riffChunk.FileType);

                throw new ChunkFileException(msg);
            }

            return riffChunk;
        }

        /// <summary>
        /// Indicates if the specified chunk <paramref name="instance"/> can be written.
        /// </summary>
        /// <param name="instance">The chunk object to write to the stream. Must be of type <see cref="RiffChunk"/>.</param>
        /// <returns>Returns true if there is a good chance <see cref="Write"/>
        /// will successfully write the chunk <paramref name="instance"/>.</returns>
        public override bool CanWrite(object instance)
        {
            return base.CanWrite(instance) && instance is RiffChunk;
        }

        /// <summary>
        /// Writes the <paramref name="instance"/> to the file stream.
        /// </summary>
        /// <param name="context">Must not be null.</param>
        /// <param name="instance">The chunk object to write to the stream. Must be of type <see cref="RiffChunk"/> and not null.</param>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Check is not recognized.")]
        public override void Write(ChunkFileContext context, object instance)
        {
            Check.IfArgumentNull(context, "context");
            Check.IfArgumentNull(instance, "instance");
            Check.IfArgumentNotOfType<RiffChunk>(instance, "instance");

            var chunk = (RiffChunk)instance;
            if (chunk.InnerChunk == null)
            {
                throw new ArgumentException("No RIFF chunk content found.", "instance");
            }

            // make sure the correct file type is set.
            chunk.FileType = new FourCharacterCode(ChunkAttribute.GetChunkId(chunk.InnerChunk));

            // write out RIFF file type
            base.Write(context, instance);

            var writer = context.CompositionContainer.GetService<FileChunkWriter>();

            writer.WriteRuntimeChunkType(chunk.InnerChunk);
        }
    }
}