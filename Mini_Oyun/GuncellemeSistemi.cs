using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public class GuncellemeSistemi
{
    private const string MevcutVersiyon = "v1.0.0";
    private const string GithubKullaniciAdi = "DevOgz-07"; 
    private const string RepoAdi = "Console_Mini_Rpg_Game"; 
    private const string UygulamaAdi = "Mini_Oyun.exe";

    public static async Task GuncellemeDenetle()
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", "C#-Game-Updater");
                string url = $"https://api.github.com/repos/{GithubKullaniciAdi}/{RepoAdi}/releases/latest";
                var response = await client.GetStringAsync(url);

                using (JsonDocument doc = JsonDocument.Parse(response))
                {
                    string enSonVersiyon = doc.RootElement.GetProperty("tag_name").GetString();

                    if (enSonVersiyon != MevcutVersiyon)
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("╔═════════════════════════════════════════════════════════╗");
                        Console.WriteLine("║            🚀 YENİ SÜRÜM OTOMATİK YÜKLENİYOR            ║");
                        Console.WriteLine("╚═════════════════════════════════════════════════════════╝");
                        Console.WriteLine($"\n[!] Yeni versiyon tespit edildi: {enSonVersiyon}");
                        Console.WriteLine("[!] Dosyalar indiriliyor, lütfen beklemeyin...");

                        // Asset listesinden ilk exe linkini çekiyoruz
                        string indirmeUrl = doc.RootElement.GetProperty("assets")[0].GetProperty("browser_download_url").GetString();

                        await DosyaIndirVeKur(indirmeUrl);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // İnternet yoksa veya hata varsa oyunu başlat
            Console.WriteLine("\n[!] Güncelleme sunucusuna bağlanılamadı, oyun başlatılıyor...");
            await Task.Delay(1000);
        }
    }

    private static async Task DosyaIndirVeKur(string url)
    {
        string geciciDosya = "Mini_Oyun_Yeni.exe";
        string mevcutExe = UygulamaAdi;

        using (WebClient wc = new WebClient())
        {
            await wc.DownloadFileTaskAsync(new Uri(url), geciciDosya);
        }

        // Kendi kendini güncelleme sihirbazı (Batch Script)
        // Bu script eski exe'yi siler, yenisini isimlendirir ve oyunu tekrar açar
        string batchKomutları = $@"
@echo off
timeout /t 2 /nobreak > nul
del ""{mevcutExe}""
ren ""{geciciDosya}"" ""{mevcutExe}""
start """" ""{mevcutExe}""
del ""%~f0""
";
        File.WriteAllText("updater.bat", batchKomutları);

        ProcessStartInfo psi = new ProcessStartInfo("updater.bat") { CreateNoWindow = true, UseShellExecute = false };
        Process.Start(psi);

        Environment.Exit(0); // Eski sürümü hemen kapat
    }
}