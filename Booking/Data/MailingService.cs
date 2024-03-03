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

                //if(ToAddress==null)
                ToAddress = _smtpConfig["AdminEmail"];

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
                        mail.Body = MailContent;

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