/* Copyright (c)  2019 TheDarkporgramer
*
*
* 
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#region << Disc Utils >>

using DiscUtils.Iso9660;
using System.IO;
using PS2_Tools.BinCue;
using System.Data;

#endregion << Disc Utils >>


namespace PS2_Tools
{

    /// <summary>
    /// PS2 Tools Backups helper for ISO/Cue/Bin 
    /// </summary>
    public class Backups
    {
        public class ISO
        {
            /// <summary>
            /// Path of ISO File
            /// </summary>
            public static string ISO_File { get; set; }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="ISOFile">Iso Path</param>
            public ISO(string ISOFile)
            {
                ISO.ISO_File = ISOFile;
            }

            /// <summary>
            /// Get a spesific file from iso path
            /// </summary>
            /// <param name="file"></param>
            /// <returns></returns>
            public Stream Get_File(string file)
            {
                Stream returnstrm = null;
                if(ISO_File == "")
                {
                    throw new Exception("ISO Path not set");
                }

                using (FileStream isoStream = File.OpenRead(ISO_File))
                {
                    //use disk utils to read iso file
                    CDReader cd = new CDReader(isoStream, true);
                    //look for the specific file naimly the system config file
                    Stream fileStream = cd.OpenFile(file, FileMode.Open);
                    returnstrm = fileStream;
                }

                return returnstrm;
            }

            /// <summary>
            /// Reads Iso into CDreader container
            /// </summary>
            /// <param name="file"></param>
            /// <returns></returns>
            public CDReader Read_ISO(string file)
            {
                using (FileStream isoStream = File.OpenRead(file))
                {
                    //use disk utils to read iso file
                    CDReader cd = new CDReader(isoStream, true);
                    return cd;
                }
            }

            /// <summary>
            /// Returns Content ID (PS2 Game ID)
            /// </summary>
            /// <param name="file">ISO Path</param>
            /// <returns></returns>
            public string Read_ContentID(string file)
            {
                using (FileStream isoStream = File.OpenRead(file))
                {
                    //use disk utils to read iso file
                    CDReader cd = new CDReader(isoStream, true);
                    //look for the specific file naimly the system config file
                    Stream fileStream = cd.OpenFile(@"SYSTEM.CNF", FileMode.Open);
                    // Use fileStream...
                    TextReader tr = new StreamReader(fileStream);
                    string fullstring = tr.ReadToEnd();//read string to end this will read all the info we need

                    //mine for info
                    string Is = @"\";
                    string Ie = ";";

                    //mine the start and end of the string
                    int start = fullstring.ToString().IndexOf(Is) + Is.Length;
                    int end = fullstring.ToString().IndexOf(Ie, start);
                    if (end > start)
                    {
                        string PS2Id = fullstring.ToString().Substring(start, end - start);

                        if (PS2Id != string.Empty)
                        {
                            return PS2Id.Replace(".", "").Replace("_", "-") ;
                            Console.WriteLine("PS2 ID Found" + PS2Id);
                        }
                        else
                        {
                            Console.WriteLine("Could not load PS2 ID");
                            throw new Exception("Could not load PS2 ID");
                        }
                    }
                }
                return "";
            }
        }

        public class Bin_Cue
        {
            public static void Convert_To_ISO(string CueFile,string OutputFile)
            {
                BinChunk binchunk = new BinChunk();
                binchunk.Convert_To_ISO(CueFile, OutputFile);
            }
        }

      
    }


    public class PS2_Content
    {
        #region << PS2 Items >>

        private static string GetNameFromID(string PS2ID, DataTable dt)
        {
            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["PS2ID"].ToString() == PS2ID)
                    {
                        return dt.Rows[i]["PS2Title"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return "";
        }

        private static string GetRegionFromID(string PS2ID, DataTable dt)
        {
            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["PS2ID"].ToString() == PS2ID)
                    {
                        return dt.Rows[i]["Region"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return "";
        }

        //PS2 Images
        private static string GetImageFromID(string PS2ID, DataTable dt)
        {
            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["PS2ID"].ToString() == PS2ID)
                    {
                        return dt.Rows[i]["Imagurl"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                return "";
            }
            return "";
        }

        private static string GetPS2ID(string isopath)
        {
            using (FileStream isoStream = System.IO.File.OpenRead(isopath))
            {
                //use disk utils to read iso file
                CDReader cd = new CDReader(isoStream, true);
                //look for the specific file naimly the system config file
                Stream fileStream = cd.OpenFile(@"SYSTEM.CNF", FileMode.Open);
                // Use fileStream...
                TextReader tr = new StreamReader(fileStream);
                string fullstring = tr.ReadToEnd();//read string to end this will read all the info we need

                //mine for info
                string Is = @"\";
                string Ie = ";";

                //mine the start and end of the string
                int start = fullstring.ToString().IndexOf(Is) + Is.Length;
                int end = fullstring.ToString().IndexOf(Ie, start);
                if (end > start)
                {
                    string PS2Id = fullstring.ToString().Substring(start, end - start);

                    if (PS2Id != string.Empty)
                    {
                        return PS2Id.Replace(".", "").Replace("_", "-");
                        Console.WriteLine("PS2 ID Found" + PS2Id);
                    }
                    else
                    {
                        Console.WriteLine("Could not load PS2 ID");
                        return "";
                    }
                }
            }
            return "";
        }

        private static DataTable ConvertToDataTable(StreamReader sr)
        {
            DataTable tbl = new DataTable();

            //add defualt columns 
            tbl.Columns.Add("PS2Title");
            tbl.Columns.Add("Region");
            tbl.Columns.Add("PS2ID");
            tbl.Columns.Add("Imagurl");

            string line;
            while ((line = sr.ReadLine()) != null)
            {


                var cols = line.Split(';');
                if (cols.Count() > 2)
                {
                    DataRow dr = tbl.NewRow();
                    dr[0] = cols[0];
                    dr[1] = cols[1];
                    dr[2] = cols[2];
                    if (cols.Count() > 3)
                    {
                        dr[3] = cols[3];
                    }
                    tbl.Rows.Add(dr);

                }
            }

            return tbl;
        }

        #endregion << PS2 Items >>

        public class PS2Item
        {
            public string PS2ID { get; set; }
            public string PS2_Title { get; set; }
            public string Region { get; set; }
            public string Picture { get; set; }
            public string Path { get; set; }
        }

        public static PS2Item GetPS2Item(string PS2ID)
        {
            PS2Item item = new PS2Item();

            StreamReader reader = new StreamReader(new MemoryStream(Encoding.ASCII.GetBytes(Properties.Resources.PS2DB)));

            DataTable dttemp = ConvertToDataTable(reader);

          
            var exmapleitem = new PS2Item();
            exmapleitem.PS2ID = PS2ID;
            //exmapleitem.Message = id;// (id,dttemp);
            exmapleitem.Path = "";
            exmapleitem.PS2_Title = GetNameFromID(PS2ID, dttemp);
            exmapleitem.Region = GetRegionFromID(PS2ID, dttemp);
            exmapleitem.Picture = GetImageFromID(PS2ID, dttemp);

            item = exmapleitem;
            return item;
        }
    }
}
