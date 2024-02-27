using Booking.Models;
using System.Net.Mail;

namespace Booking.Data
{
    public class MailingService
    {
        private readonly IConfiguration _config;
        private readonly IConfigurationSection _smtpConfig;

        public MailingService()
        {
            _config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            _smtpConfig = _config.GetSection("SMTPMailConfig");
        }

        public string FormatMailContent(string MailContent)
        {
            string SendMailContent = string.Empty;
            string LoginURL = _smtpConfig["LoginURL"].ToString();
            string ContactMailId = _smtpConfig["ContactUsMailId"].ToString();

            SendMailContent = @"<section style='border: 1px solid #ccc;padding:20px;max-width:800px;margin:0 auto'>

            <header style='border-bottom: 1px solid #ccc;margin-bottom:30px;'>
	            <img src='" + LoginURL + @"/Images/optioclogo_email.jpg' width='100%' height='auto' alt='OptionC'/>
            </header>";
            MailContent = MailContent.Replace("URL", @"<a title='Click here to login'  target='_blank' style='border-radius: 3px; font-size: 12px; line-height: 1.5; padding: 5px 10px; background-color: #1ca59e; border-color: #1ca59e; color: #ffffff; text-decoration: none; '
                                                        href='" + LoginURL + "/find-school'>Login Now</a>");

            SendMailContent += MailContent + @"<p style='margin-top:50px;border-top:solid 1px #808080'><span style='text-transform: uppercase; font-family: Verdana,sans-serif; color: black; font-size: 10pt; '>DISCLAIMER</span><br/><span style='color:

               black; line-height: 12.26px; font-family: Verdana,sans-serif; font-size: 8pt'>Please do not respond directly to this e-mail. The originating e-mail account is not monitored.<br /> If you have any questions please feel free to

               contact us via <a href='mailto:" + ContactMailId + @"' target='_blank'><span><b><span style='font-size:9.0pt;font-family:&quot;Calibri&quot;,sans-

               serif;color:#604a7b'>" + ContactMailId + @"</span></b></span><span></span></a></span></p>" + "</section>";

            return SendMailContent;
        }

        public bool SendEmail(string MailSubject, string MailContent, string ToAddress, string CCAddress = null, string AttachmentFile = null)
        {
            string SendMailFlag = _smtpConfig["SendMailFlag"];

            bool IsSuccess = false;
            if (SendMailFlag.Equals("1"))
            {
                string SmtpServer = _smtpConfig["SMTPServer"];
                int SmtpPort = Convert.ToInt16(_smtpConfig["SMTPPort"]);
                string DisplayName = _smtpConfig["DisplayName"];
                string Username = _smtpConfig["MUserName"];
                string Password = _smtpConfig["MPassword"];
                string SSL = _smtpConfig["IsSSLEnabled"];
                string contactUsMailId = _smtpConfig["ContactUsMailId"];
                string ccMailId = _smtpConfig["CCMailId"];
                string bccMailId = _smtpConfig["BCCMailId"];

                string SendMailContent = string.Empty;
                try
                {
                    // MailMessage mail = new MailMessage(Username, ToAddress, MailSubject, MailContent);
                    MailMessage mail = new MailMessage();
                    if (DisplayName != "")
                        mail.From = new MailAddress(Username, DisplayName);
                    else
                        mail.From = new MailAddress(Username);

                    mail.To.Add(ToAddress);
                    mail.Subject = MailSubject;
                    mail.BodyEncoding = System.Text.Encoding.GetEncoding("utf-8");
                    mail.IsBodyHtml = true;
                    if (MailContent != null)
                        mail.Body = FormatMailContent(MailContent);

                    if (!string.IsNullOrEmpty(CCAddress))
                    {
                        string[] ccMailAddress = CCAddress.Split(';');
                        foreach (string strccaddress in ccMailAddress)
                        {
                            mail.CC.Add(strccaddress);
                        }
                    }
                    if (!string.IsNullOrEmpty(AttachmentFile))
                    {
                        string[] AttachmentFilesList = AttachmentFile.Split(',');

                        foreach (string _file in AttachmentFilesList)
                        {
                            try
                            {
                                System.Net.Mail.Attachment aAttachment = new System.Net.Mail.Attachment(_file);
                                mail.Attachments.Add(aAttachment);
                            }
                            catch (Exception ex)
                            {
                                new ErrorLog().WriteLog(ex);
                            }
                        }
                    }
                    //Send the message.
                    SmtpClient smtpClient = new SmtpClient(SmtpServer, SmtpPort);
                    System.Net.NetworkCredential netCred = new System.Net.NetworkCredential(Username, Password);
                    smtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    smtpClient.Credentials = netCred;
                    smtpClient.EnableSsl = false;
                    if (SSL == "0")//0 when the mail server is gmail and others ; 1 when the mail server is intranet service
                        smtpClient.EnableSsl = true;
                    smtpClient.Send(mail);
                    IsSuccess = true;
                }
                catch (Exception ex)
                {
                    new ErrorLog().WriteLog(ex);
                }
            }
            return IsSuccess;

            // Use the extracted values here to send emails
        }
    }
}