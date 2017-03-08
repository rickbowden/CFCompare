using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Amazon;
using Amazon.EC2;
using Amazon.EC2.Model;
using Amazon.EC2.Util;
using Amazon.CloudFormation;
using Amazon.CloudFormation.Model;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;

namespace CFCompare
{
    class Utils
    {
        public static Dictionary<string, string> Protocol()
        {
            Dictionary<string, string>  dic = new Dictionary<string, string>();

            //Get list from Ports.xml file
            XDocument xdoc = XDocument.Load("Ports.xml");
            dic = xdoc.Descendants("ports").Elements().ToDictionary(n => (n.Attribute("number").Value), n => n.Value);

            //Check dictionary and fall back to hard coded list if empty
            if (dic.Count == 0)
            {
                dic.Add("-1", "ALL");
                dic.Add("1", "ICMP");
                dic.Add("6", "TCP");
                dic.Add("17", "UDP");
                dic.Add("22", "SSH");
                dic.Add("23", "telnet");
                dic.Add("25", "SMTP");
                dic.Add("42", "nameserver");
                dic.Add("53", "DNS");
                dic.Add("80", "HTTP");
                dic.Add("110", "POP3");
                dic.Add("389", "LDAP");
                dic.Add("465", "SMTPS");
                dic.Add("443", "HTTPS");
                dic.Add("993", "IMAPS");
                dic.Add("995", "POPS3S");
                dic.Add("1433", "My SQL");
                dic.Add("1521", "ORACLE");
                dic.Add("3306", "MySQL/Aurora");
                dic.Add("2049", "NSF");
                dic.Add("3389", "RDP");
                dic.Add("5432", "PostgeSQL");
                dic.Add("5439", "Redshift");
                dic.Add("5985","WinRM-HTTP");
                dic.Add("5986", "WinRM-HTTPS");
                dic.Add("8080", "HTTP*");
                dic.Add("8443", "HTTPS*");
            }            
            return dic;
        }

        public static void WriteToFile(string file, string inputString, bool appendToFile)
        {
            using (StreamWriter sw = new StreamWriter(file, appendToFile))// true to append
            {
                sw.WriteLine(inputString);
                sw.Flush();
            }
        }
    }
}
