using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Threading;
using System.Diagnostics;

namespace Discord_Token_Checker
{
    public class Program
    {
        static int Valid, Invalid, Total;

        static void Main(string[] args)
        {
            Console.Title = "Discord Token Checker";
            Console.SetWindowSize(80, 20);


            string Watermark = @"
  ████████╗░█████╗░██╗░░██╗███████╗███╗░░██╗
  ╚══██╔══╝██╔══██╗██║░██╔╝██╔════╝████╗░██║
  ░░░██║░░░██║░░██║█████═╝░█████╗░░██╔██╗██║
  ░░░██║░░░██║░░██║██╔═██╗░██╔══╝░░██║╚████║
  ░░░██║░░░╚█████╔╝██║░╚██╗███████╗██║░╚███║
  ░░░╚═╝░░░░╚════╝░╚═╝░░╚═╝╚══════╝╚═╝░░╚══╝";

            Console.WriteLine($"{Watermark}\n");

            if (args.Length == 0) /* Check If Args (Dropped File) == None/0 */
            {
                Console.WriteLine("\n  Drag And Drop A Text File With Tokens To Begin");
                Thread.Sleep(5000);
                Process.GetCurrentProcess().Kill();
            }

            foreach (string Tokens in File.ReadAllLines(args[0])) /* Read Each Line Of Dropped File */
            {
                try
                {
                    using (var Client = new WebClient { Proxy = null })
                    {
                        Client.Headers.Add("Content-Type", "application/json");
                        Client.Headers.Add(HttpRequestHeader.Authorization, Tokens); /* Add Auth Headers */
                        string Result = Client.DownloadString("https://discord.com/api/v8/users/@me"); /* Download Result As String */

                        if (!Result.Contains("Unauthorized")) /* Result Dosen't = Unauthorized */
                        {
                            Valid++; /* Add 1 To Valid And Total */
                            Total++; 

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"  {Tokens}"); 
                            File.AppendAllText("./ValidTokens.txt", $"{Tokens}\n"); /* Append Working Token To File */
                        }
                    }
                }
                catch
                {
                    Invalid++; /* Add 1 To Invalid Amount And Total */
                    Total++;

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"  {Tokens}");
                }
                Console.Title = $"Discord Token Checker - (Valid : {Valid}, Invalid : {Invalid}, Total : {Total}) | ImmuneLion318#0001"; /* Set Console Title */
            }

            Console.ReadLine();
        }
    }
}
