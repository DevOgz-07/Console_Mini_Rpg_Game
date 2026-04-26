using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public class GuncellemeSistemi
{
    public const string MevcutVersiyon = "v1.0.3";
    private const string GithubKullaniciAdi = "DevOgz-07";
    private const string RepoAdi = "Console_Mini_Rpg_Game";
    private const string UygulamaAdi = "Mini_Oyun.exe";

    // SADECE KONTROL EDER (Menüde seçenek göstermek için)
    public static async Task<bool> YeniGuncellemeVarMi()
    {
        var handler = new HttpClientHandler() { AllowAutoRedirect = true };
        try
        {
            using (HttpClient client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Add("User-Agent", "C#-Game-Updater");
                string url = $"https://api.github.com/repos/{GithubKullaniciAdi}/{RepoAdi}/releases/latest";
                var responseJson = await client.GetStringAsync(url);

                using (JsonDocument doc = JsonDocument.Parse(responseJson))
                {
                    string enSonVersiyon = doc.RootElement.GetProperty("tag_name").GetString();
                    return enSonVersiyon != MevcutVersiyon;
                }
            }
        }
        catch { return false; }
    }

    // GÜNCELLEMEYİ BAŞLATIR (Seçenek seçilince çalışır)
    public static async Task GuncellemeBaslat()
    {
        var handler = new HttpClientHandler() { AllowAutoRedirect = true };
        try
        {
            using (HttpClient client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Add("User-Agent", "C#-Game-Updater");
                string url = $"https://api.github.com/repos/{GithubKullaniciAdi}/{RepoAdi}/releases/latest";
                var responseJson = await client.GetStringAsync(url);

                using (JsonDocument doc = JsonDocument.Parse(responseJson))
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("╔═════════════════════════════════════════════════════════╗");
                    Console.WriteLine("║            🚀 GÜNCELLEME İŞLEMİ BAŞLADI                 ║");
                    Console.WriteLine("╚═════════════════════════════════════════════════════════╝");

                    string indirmeUrl = "";
                    var assets = doc.RootElement.GetProperty("assets");
                    foreach (var asset in assets.EnumerateArray())
                    {
                        if (asset.GetProperty("name").GetString() == UygulamaAdi)
                        {
                            indirmeUrl = asset.GetProperty("browser_download_url").GetString();
                            break;
                        }
                    }

                    if (!string.IsNullOrEmpty(indirmeUrl))
                    {
                        await DosyaIndirVeKur(indirmeUrl, client);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n[!] Hata: {ex.Message}");
            await Task.Delay(2000);
        }
    }

    private static async Task DosyaIndirVeKur(string url, HttpClient client)
    {
        string geciciDosya = "Mini_Oyun_Yeni.exe";
        string mevcutExe = UygulamaAdi;

        var response = await client.GetAsync(url);
        using (var fs = new FileStream(geciciDosya, FileMode.Create, FileAccess.Write, FileShare.None))
        {
            await response.Content.CopyToAsync(fs);
        }

        string batchKomutlari = $@"
@echo off
timeout /t 2 /nobreak > nul
taskkill /f /im ""{mevcutExe}"" > nul 2>&1
if exist ""{mevcutExe}"" del /f /q ""{mevcutExe}""
if exist ""{geciciDosya}"" ren ""{geciciDosya}"" ""{mevcutExe}""
start """" ""{mevcutExe}""
del ""%~f0""
";
        File.WriteAllText("updater.bat", batchKomutlari);

        ProcessStartInfo psi = new ProcessStartInfo("updater.bat")
        {
            CreateNoWindow = true,
            UseShellExecute = false
        };
        Process.Start(psi);
        Environment.Exit(0);
    }
}