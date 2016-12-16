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

namespace CFComapre
{
    class Utils
    {
        public static Dictionary<int, string> Protocol()
        {
            Dictionary<int, string>  dic = new Dictionary<int, string>();

            dic.Add(-1, "ALL");
            dic.Add(1, "ICMP");
            dic.Add(6, "TCP");
            //UDP = 17,
            //SSH = 22,
            //telnet = 23,
            //SMTP = 25,
            //nameserver = 42,
            //DNS = 53,
            //HTTP = 80,
            //POP3 = 110,
            //LDAP = 389,
            //SMTPS = 465,
            //HTTPS = 443,
            //IMAPS = 993,
            //POPS3S = 995,
            //[System.ComponentModel.Description("My SQL")]
            //MYSQL = 1493,
            //HTTP2 = 8080,
            //RDP = 3389,

            return dic;
        }
    }
}
