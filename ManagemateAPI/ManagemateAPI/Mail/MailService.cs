using MailKit.Net.Smtp;
using ManagemateAPI.Encryption;
using ManagemateAPI.Information;
using Microsoft.Extensions.Options;
using MimeKit;

namespace ManagemateAPI.Mail
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;

        public MailService(IOptions<MailSettings> mailSettingsOptions)
        {
            _mailSettings = mailSettingsOptions.Value;

        }



        public async Task<string> SendMailAsync(MailData mailData)
        {

                using (MimeMessage emailMessage = new MimeMessage())
                {
                    MailboxAddress emailFrom = new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail);
                    emailMessage.From.Add(emailFrom);
                    MailboxAddress emailTo = new MailboxAddress(mailData.EmailToName, mailData.EmailToid);
                    emailMessage.To.Add(emailTo);

                    // you can add the CCs and BCCs here.
                    //emailMessage.Cc.Add(new MailboxAddress("Cc Receiver", "cc@example.com"));
                    //emailMessage.Bcc.Add(new MailboxAddress("Bcc Receiver", "bcc@example.com"));

                    emailMessage.Subject = Mail_Vars.ADD_USER_SUBJECT;

                    string emailTemplateText = string.Empty;

                    try
                    {

                        emailTemplateText = Mail_Vars.GetTemplate1();


                        switch (mailData.EmailTemplate)
                        {
                            case 1:

                                emailMessage.Subject = Mail_Vars.TWO_STEP_LOGIN_SUBJECT;
                                emailTemplateText = Mail_Vars.GetTemplate2();
                                break;

                            default:
                                break;
                        }

                    }
                    catch (Exception)
                    {

                        throw new Exception("15");//_15_CONFIRM_CODE_READ_TEMPLATE_ERROR

                }


                    emailTemplateText = string.Format(emailTemplateText, mailData.EmailToName, mailData.EmailCode, DateTime.Today.Date.ToShortDateString().Replace("/", "."));

                    BodyBuilder emailBodyBuilder = new BodyBuilder();
                    emailBodyBuilder.HtmlBody = emailTemplateText;
                    emailBodyBuilder.TextBody = Mail_Vars.MAIL_SidE_TEXT;


                    emailMessage.Body = emailBodyBuilder.ToMessageBody();
                    //this is the SmtpClient from the Mailkit.Net.Smtp namespace, not the System.Net.Mail one
                    using (SmtpClient mailClient = new SmtpClient())
                    {
                        await mailClient.ConnectAsync(_mailSettings.Server, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                        await mailClient.AuthenticateAsync(_mailSettings.SenderEmail, Crypto.GetAppCode());
                        await mailClient.SendAsync(emailMessage);
                        await mailClient.DisconnectAsync(true);
                    }

                    return Info.CONFIRM_CODE_SENT;
                }


        }




    }

}
