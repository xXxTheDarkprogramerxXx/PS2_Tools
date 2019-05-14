using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PS2_Tools.BinCue
{

    public class BinChunk
    {
        public const int SectorLength = 2352;
        public static bool Verbose;
        private const string CueExtension = ".cue";

        static string outFileNameBase;
        static string cueFileName;

        public void Convert_To_ISO(string file,string outfile)
        {
            string cueFileName = file;
            string outFileNameBase = outfile;

            //Track.TruncatePsx = true;

            CueFile cueFile;

            try
            {
                cueFileName = Path.ChangeExtension(cueFileName, CueExtension);
                cueFile = new CueFile(cueFileName);
            }
            catch (Exception e)
            {
                throw new ApplicationException($"Could not read CUE {cueFileName}:\n{e.Message}");
            }

            Stream binStream;
            try
            {
                binStream = File.OpenRead(cueFile.BinFileName);
            }
            catch (Exception e)
            {
                throw new ApplicationException($"Could not open BIN {cueFile.BinFileName}: {e.Message}");
            }

            Console.WriteLine(Environment.NewLine + "Writing tracks:");
            foreach (Track curTrack in cueFile.TrackList)
            {
                // Include track number when more than 1 track.
                string outFileName;
                if (cueFile.TrackList.Count > 1)
                    outFileName =
                        $"{outFileNameBase}{curTrack.TrackNumber:00}.{curTrack.FileExtension.ToString().ToLower()}";
                else
                    outFileName = $"{outFileNameBase}.{curTrack.FileExtension.ToString().ToLower()}";
                curTrack.Write(binStream, outFileName);
            }
        }
    }
}
