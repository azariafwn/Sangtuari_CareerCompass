using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System.Threading.Tasks;

namespace SangtuariCareerCompass.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        // Tambahkan parameter assessmentType di sini
        public async Task SendResultEmailAsync(string toEmail, string studentName, string reportUrl, string assessmentType)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Sangtuari Consulting", "noreply@sangtuari.com"));
            message.To.Add(new MailboxAddress(studentName, toEmail));
            message.Subject = "Hasil Asesmen Psikologis - Sangtuari's Career Compass";

            // Logika dinamis untuk paragraf penjelasan
            string paragrafKhusus = "";

            if (assessmentType == "Exploration") // SMP
            {
                paragrafKhusus = @"
                    <p>Selamat! Kamu telah berhasil menyelesaikan seluruh rangkaian tes minat dan bakat tingkat SMP/Sederajat (Program Exploration).</p>
                    <p>Hasil tes ini mencakup analisis mengenai potensi kognitif, preferensi gaya belajar, serta arah minat karier kamu.</p>
                    <p>Laporan ini dirancang khusus untuk membantumu merencanakan metode belajar yang lebih efektif, serta memberikan panduan awal yang objektif dalam memilih penjurusan saat melangkah ke jenjang SMA/SMK nanti.</p>";
            }
            else if (assessmentType == "Discovery") // SMA
            {
                paragrafKhusus = @"
                    <p>Selamat! Kamu telah berhasil menyelesaikan seluruh rangkaian tes peminatan dan penjurusan tingkat SMA/Sederajat (Program Discovery).</p>
                    <p>Hasil tes ini mencakup analisis mendalam mengenai kapasitas intelektual, sikap kerja dan kepribadian, gaya belajar, serta rekomendasi pendidikan dan karier.</p>
                    <p>Laporan ini sangat berguna sebagai kompas untuk memantapkan pilihan program studi di perguruan tinggi, mengenali kekuatan dirimu, dan merencanakan langkah strategis untuk masa depan kariermu.</p>";
            }
            else // Advanced / Default
            {
                paragrafKhusus = @"
                    <p>Selamat! Kamu telah berhasil menyelesaikan seluruh rangkaian tes psikologi di platform kami.</p>
                    <p>Hasil tes ini mencakup gambaran komprehensif mengenai potensi kognitif, kepribadian, dan minat profesionalmu.</p>
                    <p>Kami berharap laporan ini dapat menjadi acuan yang bermanfaat untuk pengembangan karier dan pengenalan dirimu yang lebih baik ke depannya.</p>";
            }

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $@"
                <div style='font-family: Arial, sans-serif; color: #333; max-width: 600px; margin: auto; border: 1px solid #ddd; border-radius: 10px; overflow: hidden;'>
                    <div style='background-color: #4A2B50; padding: 20px; text-align: center; color: #FBE676;'>
                        <h2 style='margin:0;'>Sangtuari's Career Compass</h2>
                    </div>
                    <div style='padding: 20px; background-color: #FFFDF4;'>
                        <p>Halo <strong>{studentName}</strong>,</p>
                        
                        {paragrafKhusus}
                        
                        <div style='text-align: center; margin: 30px 0;'>
                            <a href='{reportUrl}' style='background-color: #9C7BB4; color: white; padding: 12px 25px; text-decoration: none; border-radius: 25px; font-weight: bold;'>Lihat & Unduh Laporan PDF</a>
                        </div>
                        <p style='font-size: 12px; color: #777;'>Jika tombol di atas tidak berfungsi, salin tautan berikut ke browser Anda: <br/> {reportUrl}</p>
                    </div>
                </div>"
            };

            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();

            var host = _config["SmtpSettings:Host"];
            var port = int.Parse(_config["SmtpSettings:Port"] ?? "2525");
            var user = _config["SmtpSettings:Username"];
            var pass = _config["SmtpSettings:Password"];

            await client.ConnectAsync(host, port, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(user, pass);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        public async Task SendCredentialEmailAsync(string toEmail, string fullName, string plainPassword, string role)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Sangtuari Consulting", "noreply@sangtuari.com"));
            message.To.Add(new MailboxAddress(fullName, toEmail));
            message.Subject = "Kredensial Akun Psikolog - Sangtuari's Career Compass";

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $@"
                <div style='font-family: Arial, sans-serif; color: #333; max-width: 600px; margin: auto; border: 1px solid #ddd; border-radius: 10px; overflow: hidden;'>
                    <div style='background-color: #4A2B50; padding: 20px; text-align: center; color: #FBE676;'>
                        <h2 style='margin:0;'>Akses Dasbor Psikolog</h2>
                    </div>
                    <div style='padding: 20px; background-color: #FFFDF4;'>
                        <p>Halo <strong>{fullName}</strong>,</p>
                        <p>Akun Anda sebagai <strong>{role}</strong> telah berhasil dibuat oleh Kepala Psikolog. Berikut adalah kredensial login Anda:</p>
                        
                        <div style='background-color: #eee; padding: 15px; border-radius: 5px; margin: 20px 0;'>
                            <p style='margin: 0;'><strong>Email:</strong> {toEmail}</p>
                            <p style='margin: 0;'><strong>Password:</strong> <span style='font-family: monospace; font-size: 16px;'>{plainPassword}</span></p>
                        </div>
                        
                        <h4 style='color: #4A2B50; margin-bottom: 10px;'>Langkah Selanjutnya (Wajib):</h4>
                        <ol style='margin-top: 0; padding-left: 20px; font-size: 14px;'>
                            <li style='margin-bottom: 8px;'>Segera login ke dasbor dan <strong>ubah password default Anda</strong> melalui menu yang tersedia.</li>
                            <li style='margin-bottom: 8px;'>Masuk ke menu <strong>Profil Psikolog</strong> untuk melengkapi data lisensi profesional Anda (Nomor SILP, masa berlaku, dan Nomor STR).</li>
                        </ol>

                        <div style='background-color: #ffe6e6; border-left: 4px solid #ff4d4d; padding: 12px 15px; margin: 25px 0; font-size: 13px; color: #b30000;'>
                            <strong><span style='font-size: 16px;'>⚠️</span> Peringatan Keamanan:</strong><br/>
                            Kredensial ini bersifat sangat rahasia. Anda dilarang keras membagikan informasi login ini kepada pihak mana pun. Segala aktivitas di dalam akun ini sepenuhnya menjadi tanggung jawab Anda guna menjaga kerahasiaan dan integritas data hasil tes peserta.
                        </div>
                        
                        <p style='margin-top: 30px;'>Salam hangat,<br/><strong>Tim Sangtuari</strong></p>
                    </div>
                </div>"
            };

            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            var host = _config["SmtpSettings:Host"];
            var port = int.Parse(_config["SmtpSettings:Port"] ?? "2525");
            var user = _config["SmtpSettings:Username"];
            var pass = _config["SmtpSettings:Password"];

            await client.ConnectAsync(host, port, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(user, pass);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}