using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RuyaOptik.Business.Interfaces;

namespace RuyaOptik.Business.Services
{
  using System.Net;
using System.Net.Mail;
using System.Text;
using Microsoft.Extensions.Configuration;

public class MailService : IMailService
{
    readonly IConfiguration _configuration;

    public MailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendMailAsync(string to, string subject, string body, bool isBodyHtml = true)
    {
        await SendMailAsync(new[] { to }, subject, body, isBodyHtml);
    }

    public async Task SendMailAsync(string[] tos, string subject, string body, bool isBodyHtml = true)
    {
        var host = _configuration["Mail:Host"];
        var port = _configuration.GetValue<int>("Mail:Port", 587); 
        var username = _configuration["Mail:Username"];
        var password = _configuration["Mail:Password"];

        if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            throw new Exception("Mail ayarları (Host, Username veya Password) eksik! Lütfen docker-compose veya .env dosyasını kontrol edin.");
        }

        using MailMessage mail = new();
        mail.IsBodyHtml = isBodyHtml;

        foreach (var to in tos)
            mail.To.Add(to);

        mail.Subject = subject;
        mail.Body = body;
        mail.From = new MailAddress(username, "Rüya Optik", Encoding.UTF8);

        using SmtpClient smtp = new();
        smtp.Credentials = new NetworkCredential(username, password);
        smtp.Port = port;
        smtp.EnableSsl = true;
        smtp.Host = host;

        await smtp.SendMailAsync(mail);
    }

    public async Task SendPasswordResetMailAsync(string to, string userId, string resetToken)
    {
        string clientUrl = _configuration["ClientUrl"] ?? "https://localhost:8081";

        StringBuilder mail = new();
        mail.AppendLine("Merhaba<br>Eğer yeni şifre talebinde bulunduysanız aşağıdaki linkten şifrenizi yenileyebilirsiniz.<br><strong><a target=\"_blank\" href=\"");
        
        mail.Append($"{clientUrl}/password-reset/{userId}/{resetToken}");
        
        mail.AppendLine("\">Yeni şifre talebi için tıklayınız...</a></strong><br><br><span style=\"font-size:12px;\">NOT : Eğer ki bu talep tarafınızca gerçekleştirilmemişse lütfen bu maili ciddiye almayınız.</span><br>Saygılarımızla...<br><br><br>Rüya Optik");

        await SendMailAsync(to, "Şifre Yenileme Talebi", mail.ToString());
    }

    public async Task SendCompletedOrderMailAsync(string to, string orderCode, DateTime orderDate, string userName)
    {
        string mail = $"Sayın {userName} Merhaba<br>" +
            $"{orderDate} tarihinde vermiş olduğunuz {orderCode} kodlu siparişiniz tamamlanmış ve kargo firmasına verilmiştir.<br>Hayrını görünüz efendim...";

        await SendMailAsync(to, $"{orderCode} Sipariş Numaralı Siparişiniz Tamamlandı", mail);
    }
}
}
