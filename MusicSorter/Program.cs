using System;
using System.IO;
using M3U.NET;

namespace MusicSorter
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");            
            string inputFolder = @"C:\Users\d.egorychev\Music\unsorted";
            string outputFolder = @"C:\Users\d.egorychev\Music\sorted";

            if(args.Length == 2)
            {
                inputFolder = args[0];
                outputFolder = args[1];
            }
            string[] allfolders = Directory.GetDirectories(inputFolder);
            foreach (string folder in allfolders)
            {
                M3UFile playlist = new M3UFile(new FileInfo(outputFolder + "\\" + folder.Substring(folder.LastIndexOf('\\')) + ".m3u"));
                string[] AllFiles = Directory.GetFiles(folder, "*.flac", SearchOption.AllDirectories);
                foreach (string filename in AllFiles)
                {
                    var tfile = TagLib.File.Create(filename);
                    string artist = "noname_author";
                    if ((tfile.Tag.AlbumArtists[0] != null) || (tfile.Tag.AlbumArtists[0] != ""))
                        artist = String.Join('-', tfile.Tag.AlbumArtists);
                    string album = "noname_album";
                    if ((tfile.Tag.Album != null) || (tfile.Tag.Album != ""))
                        album = tfile.Tag.Album;

                    string PathDeb = artist + "\\" + album + filename.Substring(filename.LastIndexOf('\\'));
                    foreach (var ch in "~#%&*{}:<>?+|\"")
                        PathDeb = PathDeb.Replace(ch, '�');
                    foreach (var ch in new string[]{ "...", ".."})
                        PathDeb = PathDeb.Replace(ch, "�");
                    FileInfo newPath = new FileInfo(outputFolder + "\\" + PathDeb);

                    Directory.CreateDirectory(newPath.DirectoryName);
                    File.Copy(filename, newPath.FullName, true);

                    MediaItem MI = new MediaItem { Location = PathDeb };
                    playlist.Files.Add(MI);

                    Console.WriteLine(newPath);
                }
                playlist.Save();
            }

            //var tfile = TagLib.File.Create(@"C:\Users\d.egorychev\Music\Nathan Dawe - Flowers (feat. Jaykae).flac");
            //string title = tfile.Tag.Title;
            //TimeSpan duration = tfile.Properties.Duration;
            //Console.WriteLine("Title: {0}, duration: {1}", title, duration);
            Console.WriteLine("Done!");
            Console.ReadLine();
        }
    }
}
