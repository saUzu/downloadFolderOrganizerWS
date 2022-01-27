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
        private List<string> kullanilanDosyalar = new List<string>();
        public downloadFolderOrganizer()
        {
            while (!ilkAtamalar())
            {
                System.Threading.Thread.Sleep(10000);
            }
            zaman = new Timer(1000) { AutoReset = true };
            zaman.Elapsed += ZamanDoldu;
        }
        private void ZamanDoldu(object sender, ElapsedEventArgs e)
        {
            dosyalar = Directory.GetFiles(downloadDosyasi);
            if (dosyalar.Length !=0)
                dosyaIslemleri(dosyalar);
        }
        private void HataBildir(string hataYazisi) 
        {
            File.AppendAllText(hataDosyasi, "--------------------------------------------------------------------------------------------------------\n");
            File.AppendAllText(hataDosyasi, DateTime.Now.ToString() + " ->\t" + hataYazisi + '\n');
        }
        private void kullanılanlariBul(string[] dosyalar)
        {
            foreach (string dosya in dosyalar)
            {
                string dosyaninAdi = Path.GetFileName(dosya);
                int noktaKonumu = dosyaninAdi.LastIndexOf('.');
                //Console.WriteLine(dosyaninAdi);
                try
                {
                    using (var fs = new FileStream(dosya, FileMode.Open, FileAccess.Read))
                    {
                        //kullanilanDosyalar.ForEach(Console.WriteLine);
                    }
                }
                catch
                {
                    if (noktaKonumu != -1)
                        kullanilanDosyalar.Add(dosyaninAdi.Substring(0, noktaKonumu));
                    else
                        kullanilanDosyalar.Add(dosyaninAdi);
                }
            }
        }
        private void dosyaIslemleri(string[] dosyalar)
        {
            kullanılanlariBul(dosyalar);
            foreach (string dosya in dosyalar)
            {
                string dosyaninAdi = Path.GetFileName(dosya);
                try
                {
                    using (var fs = new FileStream(dosya, FileMode.Open, FileAccess.Read)) 
                    {
                    }
                    //Console.WriteLine("kullanilmayan: " + dosyaninAdi);
                    int noktaKonumu = dosyaninAdi.LastIndexOf('.');
                    if (noktaKonumu != -1)
                    {
                        int adUzunlugu = dosyaninAdi.Length;
                        string gidilecekDosya;
                        if (!kullanilanDosyalar.Contains(dosyaninAdi))
                        {
                            ++noktaKonumu;
                            try
                            {
                                string uzantisi = dosyaninAdi.Substring(noktaKonumu, adUzunlugu - noktaKonumu).ToLower();
                                gidilecekDosya = uzantilar.FirstOrDefault(x => x.Value.Any(y => y == uzantisi)).Key;
                                //Console.WriteLine("kullanılmayan yol:\t" + downloadDosyasi + gidilecekDosya + @"\" + dosyaninAdi);
                                File.Move(dosya, downloadDosyasi + gidilecekDosya + @"\" + dosyaninAdi);
                                if (gidilecekDosya == null)
                                {
                                    Console.WriteLine("diger yol:\t" + downloadDosyasi + digerYOL + @"\" + dosyaninAdi);
                                    File.Move(dosya, (downloadDosyasi + digerYOL + @"\" + dosyaninAdi));
                                }
                            }
                            catch (Exception hata)
                            {
                                HataBildir(hata.ToString());
                                //Console.WriteLine(dosyaninAdi);
                                //Console.WriteLine(hata.ToString());
                            }
                        }
                        else
                        {
                            HataBildir("Bu dosya hâlâ indiriliyor. -> " + dosyaninAdi);
                        }
                    }
                    else
                    {
                        Console.WriteLine("diger yol:\t" + downloadDosyasi + digerYOL + @"\" + dosyaninAdi);
                        File.Move(dosya, (downloadDosyasi + digerYOL + @"\" + dosyaninAdi));
                    }
                }
                catch (Exception hata)
                {
                    //Console.WriteLine(dosyaninAdi);
                    HataBildir(hata.ToString());
                }
            }
            kullanilanDosyalar.Clear();
        }
        private bool ilkAtamalar()
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
                    resimlerDizisi.AddRange(new string[] { "ai", "ico", "ps", "psd", "svg", "jpeg", "jpg", "png", "gif", "bmp", "dib", "jpe", "jfif", "tif", "tiff", "heic" });
                    yazitlarDizisi.AddRange(new string[] { "dat", "docx", "docm", "doc", "dotx", "dotm", "dot", "pdf", "xps", "rtf", "txt", "xml", "odt", "csv", "odp", "pptx", "pptm", "ppt", "potx", "pot", "potm", "ppsx", "ppsm", "pps", "xlsx", "xlsm", "xlsb", "xls", "xlt", "dif", "xla", "ods" });
                    programlarDizisi.AddRange(new string[] { "exe", "bat", "ex", "shs", "lnk", "app", "apk", "bin", "com", "jar", "msi", "wsf", "py", "iso" });
                    rarlarDizisi.AddRange(new string[] { "rar", "zip", "7z", "arj", "deb", "bzip2", "gzip", "tar", "wim", "xz", "z" });
                    try
                    {
                        uzantilar.Add(@"\Resimler", resimlerDizisi);
                        uzantilar.Add(@"\Yazıtlar", yazitlarDizisi);
                        uzantilar.Add(@"\Programlar", programlarDizisi);
                        uzantilar.Add(@"\Sıkıştırılmışlar", rarlarDizisi);

                    }
                    catch (Exception hata) { HataBildir(hata.ToString()); return false; }
                }
                catch (Exception hata) { HataBildir(hata.ToString()); return false; }
                //Console.WriteLine(uzantilar[3]);
                //uzantilar[2].ForEach(Console.WriteLine);
                //Console.WriteLine("asd:|" + uzantilar.FirstOrDefault(x => x.Value.Any(y => y == "11")).Key+"|");
                //Console.WriteLine(hataDosyasi);
                //Console.ReadLine();

            }
            catch (Exception hata){ HataBildir(hata.ToString()); return false; }
            return true;
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



/*
 
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
                        if (gidilecekDosya == null || uzantisi != "tmp")
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
 
 
 */
