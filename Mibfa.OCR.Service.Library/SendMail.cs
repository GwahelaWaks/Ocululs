using System;
//using System.Web.Mail;
using System.Net.Mail;
//using System.Configuration;

public class MailHelper
{
    private string _Server = "";
    private string _Port = "";
    private string _Subject = "";
    private string _Body = "";
    private string _Attachment = "";
    private string _SendTo = "";
    private string _From = "";
    private bool _IsHtml = false;

    public bool IsHTML
    {
        get { return _IsHtml; }
        set { _IsHtml = value; }
    }
    public string SMTP_Server
    {
        get { return _Server; }
        set { _Server = value; }
    }
    public string SMTP_Port
    {
        get { return _Port; }
        set { _Port = value; }
    }
    public string Subject
    {
        get { return _Subject; }
        set { _Subject = value; }
    }
    public string Body
    {
        get { return _Body; }
        set { _Body = value; }
    }
    public string Attachment
    {
        get { return _Attachment; }
        set { _Attachment = value; }
    }
    public string SendTo
    {
        get { return _SendTo; }
        set { _SendTo = value; }
    }
    public string From
    {
        get { return _From; }
        set { _From = value; }
    }

    public void MailMessage(string sendUsername, string sendPassword)
    {
        MailMessage objMail = new MailMessage();
        
        objMail.IsBodyHtml = IsHTML;

        //MailAttachment Attach = null; //System.Web.Mail
        Attachment Attach = null; //System.Net.Mail
        try
        {
            string[] attachments = _Attachment.Split(';');
            if (!string.IsNullOrEmpty(_Attachment))
            {
                foreach (string attachment in attachments)
                {
                    //Attach = new MailAttachment(attachment); //System.Web.Mail
                    Attach = new Attachment(attachment); //System.Net.Mail
                    objMail.Attachments.Add(Attach);
                    Attach = null;
                }
            }
            //SmtpMail.SmtpServer = _Server; //System.Web.Mail
            SmtpClient mailClient = new SmtpClient(SMTP_Server, Convert.ToInt32(SMTP_Port));  //System.Net.Mail
            //this needs to be the Smtp mail server, a proxy will not work, get this from your administrator, or Exchange server name in outlook
            //these Private variables can be set from the class you are using them in
            //objMail.From = _From; //System.Web.Mail
            objMail.From = new MailAddress(From); //System.Net.Mail
            //objMail.To = _SendTo; //System.Web.Mail
            objMail.To.Add(SendTo.Replace(';', ','));
            objMail.Subject = _Subject;
            objMail.Body = _Body;
            //objMail.BodyFormat = MailFormat.Html; //System.Web.Mail
            objMail.IsBodyHtml = true; //System.Net.Mail

            //if (sendUsername != string.Empty) //System.Web.Mail
            //{
            //    objMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1");	//basic authentication
            //    objMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", sendUsername); //set your username here
            //    objMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", sendPassword);	//set your password here
            //}
            mailClient.Credentials = new System.Net.NetworkCredential(sendUsername, sendPassword); //System.Net.Mail

            //SmtpMail.Send(objMail); //System.Web.Mail
            mailClient.Send(objMail);
            Attach = null;
            objMail = null;
        }
        catch (Exception ex)
        {
            Attach = null;
            objMail = null;
            throw ex;
        }
    }
    /// <summary>
    /// Sends an mail message
    /// </summary>
    /// <param name="from">Sender address</param>
    /// <param name="to">Recepient address</param>
    /// <param name="bcc">Bcc recepient</param>
    /// <param name="cc">Cc recepient</param>
    /// <param name="subject">Subject of mail message</param>
    /// <param name="body">Body of mail message</param>
    /// 
    public void SendMailMessage(string sendUsername, string sendPassword)
    {
        //try
        //{
        //    // Instantiate a new instance of MailMessage
        //    MailMessage mMailMessage = new MailMessage();

        //    Attachment Attach = null;
        //    if (!string.IsNullOrEmpty(_Attachment))
        //    {
        //        Attach = new Attachment(_Attachment);
        //        mMailMessage.Attachments.Add(Attach);
        //    }

        //    mMailMessage.From = new MailAddress(_From);
        //    mMailMessage.To.Add(new MailAddress(_SendTo));
        //    // Check if the bcc value is null or an empty string
        //    //if ((bcc != null) && (bcc != string.Empty))
        //    //{
        //    //    // Set the Bcc address of the mail message
        //    //    mMailMessage.Bcc.Add(new MailAddress(bcc));
        //    //}

        //    // Check if the cc value is null or an empty value
        //    //if ((cc != null) && (cc != string.Empty))
        //    //{
        //    //    // Set the CC address of the mail message
        //    //    mMailMessage.CC.Add(new MailAddress(cc));
        //    //}

        //    mMailMessage.Subject = _Subject;
        //    mMailMessage.Body = _Body;
        //    mMailMessage.IsBodyHtml = true;
        //    mMailMessage.Priority = MailPriority.Normal;

        //    SmtpClient mSmtpClient = new SmtpClient();
        //    mSmtpClient.Host = _Server;
        //    mSmtpClient.Port = int.Parse(_Port);
        //    System.Net.NetworkCredential SMTPUserInfo = new System.Net.NetworkCredential(sendUsername, sendPassword);
        //    mSmtpClient.UseDefaultCredentials = false;
        //    mSmtpClient.Credentials = SMTPUserInfo;

        //    // Send the mail message
        //    mSmtpClient.Send(mMailMessage);
        //}
        //catch (System.Exception ex)
        //{
        //    throw ex;
        //}
    }
}
