using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.IO;

namespace DownloadsFolderOrganizer_cSharp
{
    public class downloadFolderOrganizer
    {
        private readonly Timer zaman;
        private string hataDosyasi;
        private string downloadDosyasi;
        private dfPath yol = new dfPath();
        private string[] dosyalar;
        private Dictionary<string, List<string>> uzantilar = new Dictionary<string, List<string>>();
        private List<string> resimlerDizisi = new List<string>();
        private List<string> yazitlarDizisi = new List<string>();
        private List<string> programlarDizisi = new List<string>();
        private List<string> rarlarDizisi = new List<string>();
        private string resimlerYOL = @"\Resimler";
        private string yazitlarYOL = @"\Yazıtlar";
        private string programlarYOL = @"\Programlar";
        private string rarlarYOL = @"\Sıkıştırılmışlar";
        private string digerYOL = @"\Diğerleri";
        public downloadFolderOrganizer()
        {
            try
            {
                hataDosyasi = yol.Main("Documents") + @"\dfoLog.txt";
                downloadDosyasi = yol.Main("Downloads");
                Directory.CreateDirectory(downloadDosyasi + resimlerYOL);
                Directory.CreateDirectory(downloadDosyasi + yazitlarYOL);
                Directory.CreateDirectory(downloadDosyasi + programlarYOL);
                Directory.CreateDirectory(downloadDosyasi + rarlarYOL);
                Directory.CreateDirectory(downloadDosyasi + digerYOL);
                try
                {
                    try
                    {
                        resimlerDizisi.AddRange(new string[] { "ai", "ico", "ps", "psd", "svg", "jpeg", "jpg", "png", "gif", "bmp", "dib", "jpe", "jfif", "tif", "tiff", "heic" });
                        yazitlarDizisi.AddRange(new string[] { "dat", "docx", "docm", "doc", "dotx", "dotm", "dot", "pdf", "xps", "rtf", "txt", "xml", "odt", "csv", "odp", "pptx", "pptm", "ppt", "potx", "pot", "potm", "ppsx", "ppsm", "pps", "xlsx", "xlsm", "xlsb", "xls", "xlt", "dif", "xla", "ods" });
                        programlarDizisi.AddRange(new string[] { "exe", "bat", "ex", "shs", "lnk", "app", "apk", "bin", "com", "jar", "msi", "wsf", "py" });
                        rarlarDizisi.AddRange(new string[] { "rar", "zip", "7z", "arj", "deb", "bzip2", "gzip", "tar", "wim", "xz", "z" });
                    }
                    catch { Stop(); return; }
                    uzantilar.Add(@"\Resimler", resimlerDizisi);
                    uzantilar.Add(@"\Yazıtlar", yazitlarDizisi);
                    uzantilar.Add(@"\Programlar", programlarDizisi);
                    uzantilar.Add(@"\Sıkıştırılmışlar", rarlarDizisi);
                }
                catch { Stop(); return; }
                //Console.WriteLine(uzantilar[3]);
                //uzantilar[2].ForEach(Console.WriteLine);
                //Console.WriteLine("asd:|" + uzantilar.FirstOrDefault(x => x.Value.Any(y => y == "11")).Key+"|");
                //Console.WriteLine(hataDosyasi);
                //Console.ReadLine();
                zaman = new Timer(1000) { AutoReset = true };
                zaman.Elapsed += ZamanDoldu;
            }
            catch { Stop(); return; }
        }
        private void ZamanDoldu(object sender, ElapsedEventArgs e)
        {
            //HataBildir(yol.Main("Downloads") + " 1ve1 " + yol.Main("SavedGames"));
            dosyalar = Directory.GetFiles(downloadDosyasi);
            //foreach (string dosya in dosyalar)

            foreach (string dosya in dosyalar)
            {
                string adi = Path.GetFileName(dosya).ToLower();
                try
                {
                    int nokta = adi.LastIndexOf('.');
                    //Console.WriteLine(adi + " ve " + nokta);
                    if (nokta == -1)
                    {
                        File.Move(dosya, (downloadDosyasi + digerYOL + @"\" + adi));
                        //Console.WriteLine(" asd ");
                    }
                    else
                    {
                        nokta++;
                        int boyut = adi.Length;
                        string uzantisi = adi.Substring(nokta, boyut - nokta);
                        string gidilecekDosya = uzantilar.FirstOrDefault(x => x.Value.Any(y => y == uzantisi)).Key;
                        //Console.WriteLine(adi + ", " + nokta + ", uzantisi: " + uzantisi);
                        if (gidilecekDosya == null && uzantisi != "tmp")
                        {
                            try
                            {
                                gidilecekDosya = downloadDosyasi + digerYOL;
                                //Console.WriteLine("\tD:" + dosya + "\tG:" + gidilecekDosya + "\tA:" + adi);
                                File.Move(dosya, gidilecekDosya + @"\" + adi);

                            }
                            catch (Exception hata1)
                            {
                                try
                                {
                                    HataBildir(hata1.ToString());
                                }
                                catch
                                {
                                    Stop();
                                    return;
                                }
                            }
                        }
                        else
                        {
                            try
                            {
                                gidilecekDosya = downloadDosyasi + gidilecekDosya;
                                //Console.WriteLine("\tD:" + dosya + "\tG:" + gidilecekDosya + "\tA:" + adi);
                                File.Move(dosya, gidilecekDosya + @"\" + adi);
                            }
                            catch (Exception hata1)
                            {
                                try
                                {
                                    HataBildir(hata1.ToString());
                                }
                                catch
                                {
                                    Stop();
                                    return;
                                }
                            }
                        }
                    }
                }
                catch(Exception hata1)
                {
                    //Console.WriteLine(hata1);
                    try
                    {
                        HataBildir(hata1.ToString());
                    }
                    catch
                    {
                        Stop();
                        return;
                    }
                }
            }
        }
        private void HataBildir(string hataYazisi) 
        {
            File.AppendAllText(hataDosyasi, DateTime.Now.ToString() + " ->\t" + hataYazisi + '\n');
        }
        public void Start()
        {
            zaman.Start();
        }
        public void Stop()
        {
            zaman.Stop();
        }
    }
}
