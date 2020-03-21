using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();
            int actualNumber = random.Next(0, 100);

            List<string> ip = new List<string>();

            List<string> username = new List<string>();

            List<int> score = new List<int>();

            List<int> guesses = new List<int>();

            List<int> previousGuesses = new List<int>();

            List<int> scoreboard = new List<int>();

            UdpClient client = null;

            int servPort = 5000;

            try
            {
                client = new UdpClient(servPort);
                Console.WriteLine("Server Started...");
                Console.WriteLine("Number is " + actualNumber);
            }
            catch (SocketException se)
            {
                Console.WriteLine(se.ErrorCode + ": " + se.Message);
                Environment.Exit(se.ErrorCode);
            }

            IPEndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);

            for (; ; )
            {
                try
                {
                    
                    byte[] byteBuffer = client.Receive(ref remoteIPEndPoint);
                    Console.Write("Handling client at " + remoteIPEndPoint + " - ");

                    String recvText = Encoding.ASCII.GetString(byteBuffer, 0, byteBuffer.Length);
                    Console.WriteLine(recvText);
                    
                    if (recvText == "User is quitting client application")
                    {
                        for (int i = 0; i < ip.Count; i++)
                            {
                                if (ip[i] == remoteIPEndPoint.ToString())
                                {
                                    ip.RemoveAt(i);
                                    username.RemoveAt(i);
                                    score.RemoveAt(i);
                                    guesses.RemoveAt(i);
                                    UpdateLeaderboard();
                                }
                            }
                    }
                    else if (recvText.Length > 3)
                    {
                        ip.Add(remoteIPEndPoint.ToString());
                        username.Add(recvText);
                        score.Add(0);
                        guesses.Add(0);
                        UpdateLeaderboard();
                    }
                    else
                    {
                        int guessNumber = Convert.ToInt16(recvText);
                        String responseMessage = "";

                        for (int i = 0; i < ip.Count; i++)
                        {
                            if (ip[i] == remoteIPEndPoint.ToString())
                            {
                                guesses[i]++;
                                UpdateLeaderboard();
                            }
                        }

                        if (guessNumber > actualNumber)
                        {
                            responseMessage = guessNumber + " is too high";
                        }

                        if (guessNumber < actualNumber)
                        {
                            responseMessage = guessNumber + " is too low";
                        }

                        if (guessNumber == actualNumber)
                        {
                            for (int i = 0; i < ip.Count; i++)
                            {
                            if (ip[i] == remoteIPEndPoint.ToString())
                                {
                                    score[i]++;
                                    for (int x = 0; x < ip.Count; x++)
                                    {
                                        IPEndPoint remoteIPEndPoint2 = IPEndPoint.Parse(ip[x]);;
                                        responseMessage = "Congratulations " + username[i] + "!\nPrevious number was " + actualNumber;
                                        Byte[] responseBytes2 = Encoding.ASCII.GetBytes(responseMessage);
                                        client.Send(responseBytes2, responseBytes2.Length, remoteIPEndPoint2);
                                    }
                                }
                            }
                            UpdateLeaderboard();
                            actualNumber = random.Next(0, 100);
                            Console.WriteLine("New number is " + actualNumber);
                        }

                        Byte[] responseBytes = Encoding.ASCII.GetBytes(responseMessage);
                        client.Send(responseBytes, responseBytes.Length, remoteIPEndPoint);
                    }
                }
                catch (SocketException se)
                {
                    Console.WriteLine(se.ErrorCode + ": " + se.Message);
                }
            }

            void UpdateLeaderboard()
            {
                List<int> scoreDescending = new List<int>();
                List<int> scoreReference = new List<int>();
                List<int> guessesReference = new List<int>();
                int currentJ = -1;

                for (int i = 0; i < score.Count; i++)
                {
                    scoreDescending.Add(score[i]);
                    scoreReference.Add(score[i]);
                    guessesReference.Add(guesses[i]);
                }
                scoreDescending.Sort((a, b) => b.CompareTo(a));
                scoreboard = new List<int>();

                for (int i = 0; i < ip.Count; i++)
                {
                    currentJ = -1;
                    for (int j = 0; j < ip.Count; j++)
                    {
                        if (scoreReference[j] >= 0)
                        {
                            if (scoreDescending[i] == scoreReference[j])
                            {
                                if (currentJ < 0)
                                {
                                    currentJ = j;
                                }
                                else
                                {
                                    if (guessesReference[j] >= 0 && guessesReference[j] < guessesReference[currentJ])
                                    {
                                        currentJ = j;
                                    }
                                }
                            }
                        }
                    }
                    if (scoreboard.Count == i && currentJ >= 0)
                    {
                        scoreboard.Add(currentJ);
                        scoreReference[currentJ] = -1;
                        guessesReference[currentJ] = -1;
                    }
                }
                String responseMessage = "                                                          \nLeaderboard\n\nPlace : Name : Score : Guesses\n";
                for (int i = 0; i < ip.Count; i++)
                {
                    responseMessage += (i+1) + ". " + username[scoreboard[i]] + " : " + score[scoreboard[i]] + " : " + guesses[scoreboard[i]] + "\n";
                }
                for (int x = 0; x < ip.Count; x++)
                {
                    IPEndPoint remoteIPEndPoint3 = IPEndPoint.Parse(ip[x]);;
                    Byte[] responseBytes3 = Encoding.ASCII.GetBytes(responseMessage);
                    client.Send(responseBytes3, responseBytes3.Length, remoteIPEndPoint3);
                }
            }
        }
    }
}