using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTest.Views
{
  /// <summary>Interface for block manager types.</summary>
  public interface IBlockManager
  {
    /// <summary>The set of block types supported by this manager.</summary>
    IBlock[] BlockTypes { get; }
    /// <summary>Creates a new block of the same type.</summary>
    IBlock NewBlock(IBlock block);
    /// <summary>Creates a new block group.</summary>
    IBlockGroup NewBlockGroup();
    /// <summary>Creates a new block that matches the line.</summary>
    IBlock NewBlock(string line);
    /// <summary>Creates an array of new blocks that match the lines.</summary>
    IBlock[] NewBlocks(string buffer);
    /// <summary>Produces a string buffer from an array of blocks.</summary>
    string ToString(IBlock[] blocks);

    /// <summary>Creates an array of new blocks from a text file.</summary>
    bool ReadFile(string path, out IBlock[] blocks);
    /// <summary>Writes a text file from an array of blocks.</summary>
    bool WriteFile(string path, IBlock[] blocks);
  }
}
