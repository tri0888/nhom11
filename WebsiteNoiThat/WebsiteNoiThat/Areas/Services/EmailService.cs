using System.Net;
using System.Net.Mail;

namespace WebsiteNoiThat.Areas.Services
{
    public class EmailService
    {
        private const string GmailUsername = "trandoanminhtri70@gmail.com";
        private const string GmailAppPassword = "jamp dcyy aowb rrzy";

        public static bool SendAutoResponse(string toEmail, string content)
        {
            try
            {
                using (var smtp = new SmtpClient())
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(GmailUsername, GmailAppPassword);

                    using (var message = new MailMessage())
                    {
                        message.From = new MailAddress(GmailUsername, "Thời Trang Cao Cấp");
                        message.To.Add(new MailAddress(toEmail));
                        message.Subject = "Phản hồi từ Thời Trang Cao Cấp";
                        message.IsBodyHtml = true;
                        message.Body = $@"
                            <h3>Kính gửi quý khách,</h3>
                            <p>Chúng tôi đã nhận được nội dung liên hệ của bạn:</p>
                            <p><i>{content}</i></p>
                            <p>Chúng tôi đã ghi nhận và xử lý yêu cầu của bạn.</p>
                            <p>Nếu cần thêm thông tin, vui lòng liên hệ với chúng tôi qua:</p>
                            <ul>
                                <li>Địa chỉ: Ngõ 34 - Phú Kiều - Kiều Mai</li>
                                <li>Điện thoại: 01649.629.629</li>
                            </ul>
                            <p>Trân trọng,<br/>Thời Trang Cao Cấp</p>
                        ";

                        smtp.Send(message);
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
    }
} 