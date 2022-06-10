using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace XMLLinearize
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length < 2) throw new Exception(GetUsageMsg());

                string src_file = args[0];
                string dst_file = args[1];

                int indent = 0;
                string currentLine = "";
                using (StreamReader reader = new StreamReader(src_file))
                {
                    using (StreamWriter writer = new StreamWriter(dst_file))
                    {
                        while (!reader.EndOfStream)
                        {
                            int currentCharInt = reader.Read();
                            if (currentCharInt < 0) throw new Exception("Invalid character at position " + reader.BaseStream.Position.ToString());

                            char currentChar = (char)currentCharInt;

                            if (currentChar == '<')
                            {
                                //decrease indent before end tag
                                if (currentLine.StartsWith("</"))
                                {
                                    indent--;
                                }

                                //write current line (if any)
                                if (!String.IsNullOrEmpty(currentLine))
                                {
                                    writer.WriteLine(GetIndent(indent) + currentLine);
                                }

                                //increase indent after start tag
                                if (currentLine.StartsWith("<") && !currentLine.StartsWith("</"))
                                {
                                    indent++;
                                }

                                //reset line
                                currentLine = "";
                            }

                            currentLine += (char)currentChar;

                            if (currentChar == '>')
                            {
                                //decrease indent before end tag
                                if (currentLine.StartsWith("</"))
                                {
                                    indent--;
                                }

                                //write current line (if any)
                                if (!String.IsNullOrEmpty(currentLine))
                                {
                                    writer.WriteLine(GetIndent(indent) + currentLine);
                                }

                                //increase indent after start tag
                                if (currentLine.StartsWith("<") && !currentLine.StartsWith("</"))
                                {
                                    indent++;
                                }

                                //reset line
                                currentLine = "";
                            }
                        }

                        if (!String.IsNullOrEmpty(currentLine)) writer.WriteLine(GetIndent(indent) + currentLine);
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            Console.WriteLine("Press <ENTER> to terminate...");
            Console.ReadLine();
        }

        private static string GetUsageMsg()
        {
            return "Invalid parameters. Usage: $ XMLLinearize <src_file> <dst_file>";
        }

        private static string GetIndent(int indent)
        {
            return new String(' ', indent * 2);
        }
    }
}
