// See https://aka.ms/new-console-template for more information
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;
using Org.BouncyCastle.Utilities;

System.Console.WriteLine("Email reader from Giulio Sorrentino versione 0.1, This program is under GNU GPL 3.0 or in your humble opinion any later version");
if (args.Length != 3) {
    Console.WriteLine("Passare come parametri l'indirizzo imap, la login e la password.");
    Environment.Exit(1);
}
using (ImapClient client = new ImapClient())
{
    using (CancellationTokenSource cancel = new CancellationTokenSource())
    {
        MimeMessage message=null;
        IMailFolder inbox=null;
        string s;
        int i;

        try
        {      
            client.Connect(args[0], 993, true, cancel.Token);
            client.Authenticate(args[1], args[2], cancel.Token);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Environment.Exit(1);
        }
        inbox = client.Inbox;

        try
        {
            inbox.Open(FolderAccess.ReadOnly, cancel.Token);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Environment.Exit(1);
        }

        for (i = 0; i < inbox.Count; i++)
        {
            message = inbox.GetMessage(i, cancel.Token);
            Console.WriteLine("Soggetto {0}: {1}",i, message.Subject);
        }


        Console.Write("Inserire il numero del messaggio da leggere: ");
        s = Console.ReadLine();
      try
        {
            i = Convert.ToInt32(s);
      }
        catch (FormatException ex)
        {
            Console.WriteLine("Non è un numero. Il programma termina.");
            Environment.Exit(1);
        }
        try
        {
            message = inbox.GetMessage(i, cancel.Token);
        } catch (ArgumentOutOfRangeException ex)
        {
            Console.WriteLine("Indice non esistente. Il programma termina.");
            Environment.Exit(1);

        }
        Console.WriteLine(message.TextBody);


        client.Disconnect(true, cancel.Token);
    }
}