using System;
using System.Security.Cryptography;
using System.Text;

public static class VeriKoruma
{
    private const string GizliAnahtar = "Gumusisik_Sirlari_2026_Secure!";

    
    public static string HashHesapla(string veri)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(veri + GizliAnahtar));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
    }

    
    public static string Kodla(string veri) => Convert.ToBase64String(Encoding.UTF8.GetBytes(veri));

    
    public static string Coz(string kodlanmisVeri) => Encoding.UTF8.GetString(Convert.FromBase64String(kodlanmisVeri));
}