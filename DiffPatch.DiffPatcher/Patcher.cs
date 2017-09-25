﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DiffPatch.Core;
using DiffPatch.Data;

namespace DiffPatch.DiffPatcher
{
    public class Patcher
    {
        public static String Patch(String src, IEnumerable<Chunk> chunks, string lineEnding = "\n")
        {
            IEnumerable<String> srcLines = StringHelper.SplitLines(src, lineEnding);
            IList<string> dstLines = new List<string>(srcLines);

            foreach (Chunk chunk in chunks)
            {
                int lineIndex = chunk.RangeInfo.NewRange.StartLine - 1; // zero-index the start line 
                if (lineIndex < 0)
                    lineIndex = 0;

                foreach (LineDiff lineDiff in chunk.Changes)
                {
                    if (lineDiff.Add)
                    {
                        dstLines.Insert(lineIndex, lineDiff.Content.Substring(2));
                        lineIndex++;
                    }
                    else if (lineDiff.Delete)
                    {
                        dstLines.RemoveAt(lineIndex);
                    }
                    else if (lineDiff.Normal)
                    {
                        lineIndex++;
                    }
                }

            }

            string patchString = string.Join(lineEnding, dstLines.ToArray());
            return patchString;
        }
    }
}
