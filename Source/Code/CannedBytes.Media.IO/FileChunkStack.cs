﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace CannedBytes.Media.IO
{
    /// <summary>
    /// Manages a stack of <see cref="FileChunk"/> instances.
    /// </summary>
    /// <remarks>Parent-child relation between chunks is also maintained.</remarks>
    public class FileChunkStack
    {
        private Stack<FileChunk> chunkStack = new Stack<FileChunk>();

        /// <summary>
        /// Removes all chunks.
        /// </summary>
        public void Clear()
        {
            this.chunkStack.Clear();
        }

        /// <summary>
        /// Root chunk of the file.
        /// </summary>
        public FileChunk RootChunk
        {
            get
            {
                if (this.chunkStack.Count >= 1)
                {
                    return this.chunkStack.ToArray()[0];
                }

                // no root set yet
                return null;
            }
            set
            {
                if (this.chunkStack.Count != 0)
                    throw new InvalidOperationException("Root Chunk has already been set.");

                PushChunk(value);
            }
        }

        /// <summary>
        /// Current chunk being processed.
        /// </summary>
        public FileChunk CurrentChunk
        {
            get
            {
                if (this.chunkStack.Count == 0) return null;

                return this.chunkStack.Peek();
            }
        }

        /// <summary>
        /// Pushes a new chunk up the stack.
        /// </summary>
        /// <param name="chunk">Must not be null.</param>
        /// <remarks>The <paramref name="chunk"/> is added as a child to the current top chunk.</remarks>
        public void PushChunk(FileChunk chunk)
        {
            Contract.Requires(chunk != null);
            Throw.IfArgumentNull(chunk, "chunk");

            var parentChunk = CurrentChunk;

            // maintain child-chunks on parent.
            if (parentChunk != null)
            {
                parentChunk.SubChunks.Add(chunk);
            }

            this.chunkStack.Push(chunk);
        }

        /// <summary>
        /// Removes the top chunk from the stack.
        /// </summary>
        /// <returns>Returns null if only the root chunk remains or the stack is empty.</returns>
        public FileChunk PopChunk()
        {
            // never pop the root.
            if (this.chunkStack.Count > 1)
            {
                return this.chunkStack.Pop();
            }

            return null;
        }
    }
}