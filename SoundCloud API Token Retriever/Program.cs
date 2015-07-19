using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Security;
using System.Web;
namespace POSToken {
class SoundCloudAPITokenRetriever
    {
        static void Main(string[] args)
        {
            Console.Title = "Sound Cloud API Token Retriever";
            Console.WriteLine("SoundCloud API Token Retriever");
            Console.Write("Enter Client ID: ");
            string ClientId = Console.ReadLine();
            Console.Write("Enter Client Secret: ");
            string ClientSecret = Console.ReadLine(); 
            Console.Write("Enter your SoundCloud Username (email address): ");
            string username = Console.ReadLine(); 
            Console.Write("Enter your SoundCloud Password: ");
            SecureString password = new SecureString();
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);

               int top = Console.CursorTop;
    
               if (key.Key == ConsoleKey.Backspace)
               {
                    if (password.Length > 0) 
                    {
                        Console.SetCursorPosition(+password.Length+31, top);
                        Console.Write(' ');
                        Console.SetCursorPosition(password.Length+31, top);
                        password.RemoveAt(password.Length-1);
                    }
               }// Ignore any key out of range. 
               else if (((int)key.Key) >= 32 && ((int)key.Key <= 126)) 
                {
                    // Append the character to the password.
                    password.AppendChar(key.KeyChar);
                    Console.Write("*");
                }
            // Exit if Enter key is pressed.
            } while (key.Key != ConsoleKey.Enter);
            Console.WriteLine("");
            string token ="";
            string tokenInfo = "";

                //WebClient to communicate via http
                WebClient client = new WebClient();
                client.Encoding = System.Text.Encoding.UTF8;
                //Authentication data
                string postData = "client_id=" + ClientId
                    + "&client_secret=" + ClientSecret
                    + "&grant_type=client_credentials&username=" + username
                    + "&password=" + password;
                try
                {
                    //Authentication
                    tokenInfo = client.UploadString("https://api.soundcloud.com/oauth2/token", postData);
                    //Parse the token
                    tokenInfo = tokenInfo.Remove(0, tokenInfo.IndexOf("token\":\"") + 8);
                    token = tokenInfo.Remove(tokenInfo.IndexOf("\""));

                }
                catch (WebException e)
                {
                    if ((e.Response is System.Net.HttpWebResponse) && (e.Response as System.Net.HttpWebResponse).StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        Console.WriteLine("Incorrect Username and/or Password.");
                    }
                    else if ((e.Response as System.Net.HttpWebResponse).StatusCode == HttpStatusCode.NotFound)
                    {
                        Console.WriteLine("Can't connect to SoundCloud!");
                    }
                    token = "Unavailable";
                }
            //Print the Data
            Console.Write("Your SoundCloud API Token is: " + token);
            Console.ReadKey(true);
        }
    }
}
