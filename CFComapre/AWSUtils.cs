using Amazon.CloudFormation;
using Amazon.CloudFormation.Model;
using Amazon.EC2;
using Amazon.EC2.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CFComapre
{
    class AWSUtils
    {
        static bool log = false;
        static string logFile = "C:\\CFCompareLog.txt";
        
        
        /// <summary>
        /// Checks if the CloudFormation template is valid using the AWS API
        /// </summary>
        /// <param name="template"></param>
        /// <returns>bool</returns>
        public static bool IsTemplateValid(string jsonString, ref AmazonCloudFormationClient cfClient)
        {
            bool result = false;
            if (jsonString != null || jsonString != "")
            {
                ValidateTemplateRequest validateRequest = new ValidateTemplateRequest();
                validateRequest.TemplateBody = jsonString;
                ValidateTemplateResponse validateResponse = cfClient.ValidateTemplate(validateRequest);
                if (validateResponse.HttpStatusCode == HttpStatusCode.OK) { result = true; }
            }
            return result;
        }

        
        // -----------------------------------------------------------------------
        // Template

        public static void ProcessTemplateResources(dynamic input, CFStack stack)
        {

            foreach (var resource in input)
            {
                string logicalId = resource.Name;

                switch ((string)resource.Value["Type"])
                {
                    case "AWS::IAM::User":
                        //ProcessIAMUserFromTemplate(resource, stack);
                        break;
                    case "AWS::IAM::AccessKey":
                        break;
                    case "AWS::EC2::SecurityGroup":
                        ProcessEC2SecurityGroupFromTemplate(resource, stack);
                        break;
                    case "AWS::EC2::SecurityGroupIngress":
                        ProcessEC2SecurityGroupIngressFromTemplate(resource, stack);
                        break;
                    case "AWS::EC2::NetworkAcl":
                        ProcessNetworkAclFromTemplate(resource, stack);
                        break;
                    case "AWS::EC2::NetworkAclEntry":
                        ProcessNetworkAclEntryFromTemplate(resource, stack);
                        break;
                    default:
                        break;
                }
            }

        }
        
        static void ProcessNetworkAclEntryFromTemplate(dynamic input, CFStack stack)
        {            
            NetworkAclEntry ne = new NetworkAclEntry();

            var rule = input.Value["Properties"];

            ne.Protocol = rule.Protocol;            
            ne.CidrBlock = rule.CidrBlock;
            ne.Egress = (bool)rule.Egress;
            ne.RuleNumber = rule.RuleNumber;
            ne.RuleAction = rule.RuleAction;


            if (rule.NetworkAclId != null)
            {
                var a = rule.NetworkAclId;
                foreach (var item in a)
                {
                    ne.NetworkAclId = item.Value;
                }
            }

            if (rule.PortRange != null)
            {
                var range = rule.PortRange;
                foreach (var item in range)
                {
                    if (item.Name == "To")
                    {
                        if (item.Value == "-1")
                        {
                            ne.ToPort = "ALL";
                        }
                        else
                        {
                            ne.ToPort = item.Value; 
                        }
                    }
                    if (item.Name == "From") 
                    {
                        if (item.Value == "-1")
                        {
                            ne.ToPort = "ALL";
                        }
                        else
                        {
                            ne.FromPort = item.Value;
                        }
                    }
                }
            }
            else //If port range is not specified in the template then AWS sets it to ALL
            {
                ne.FromPort = "ALL";
                ne.ToPort = "ALL";
            }

            //Format PortRange
            string from = "";
            string to = "";
            FormatPortRange(ne.FromPort, ne.ToPort, out from, out to);
            ne.FromPort = from;
            ne.ToPort = to;

            if (rule.Icmp != null)
            {
                //May not be needed    
            }

            if (ne.NetworkAclId != null)
            {
                NetworkAcl x = stack.Resources.Find(n => n != null && n.LogicalId == ne.NetworkAclId);
                if (x != null)
                {
                    x.Properties.NetworkAclEntry.Add(ne);
                }
                else
                {
//TODO
//Either remember and process orphaned ingress rule
//Or write out to error log.
                    MessageBox.Show("Error", "Did not find NACL " + ne.NetworkAclId + " to add entry to", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                }
            }
                       
        }
        
        static void ProcessNetworkAclFromTemplate(dynamic input, CFStack stack)
        {   
            NetworkAcl nacl = new NetworkAcl();

            nacl.LogicalId = input.Name;
            nacl.Type = "AWS::EC2::NetworkAcl";

            var props = input.Value["Properties"];
            foreach (var prop in props)
            {
                switch ((string)prop.Name)
                {                    
                    case "VpcId":
                        var a = prop.Value;
                        foreach (var item in a)
                        {
                            nacl.Properties.VpcId = item.Value;
                        }                        
                        break;
                    case "Tags":

                        break;                    
                }
            }

            stack.Resources.Add(nacl);
        }
        
        static void ProcessEC2SecurityGroupIngressFromTemplate(dynamic input, CFStack stack)
        {
            EC2SecurityGroupIngress sgi = new EC2SecurityGroupIngress();

            var rule = input.Value["Properties"];

            //FormatProtocol - Protocol could be a number or text (e.g. 6 or tcp)                         
            sgi.IpProtocol = FormatProtocol(rule.IpProtocol.ToString());
            //-------------------------------------------------------------------

            //FormatPortRange - Port range could be 0-0 -1-1 0-65535
            string from = "";
            string to = "";
            FormatPortRange(rule.FromPort.ToString(), rule.ToPort.ToString(), out from, out to);
            sgi.FromPort = from;
            sgi.ToPort = to;
            //-------------------------------------------------------------------
            
            sgi.CidrIp = rule.CidrIp;
            if (rule.SourceSecurityGroupId != null)
            {
                if (rule.SourceSecurityGroupId != null)
                {
                    var Ids = rule.SourceSecurityGroupId;
                    foreach (var Id in Ids)
                    {
                        sgi.SourceSecurityGroupId = Id.Value;
                    }
                }
            }

            if (rule.GroupId != null)
            {
                sgi.GroupName = rule.GroupId["Ref"].Value;
                EC2SecurityGroup x = stack.Resources.Find(n => n != null && n.LogicalId == rule.GroupId["Ref"].Value);
                if (x != null)
                {
                    x.Properties.SecurityGroupIngress.Add(sgi);
                }
                else
                {
//TODO
//Either remember and process orphaned ingress rule
//Or write out to error log.
                    MessageBox.Show("Error", "Did not find security group " + sgi.GroupName + " to add ingress rule to", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        
        static void ProcessEC2SecurityGroupFromTemplate(dynamic input, CFStack stack)
        {
            EC2SecurityGroup sg = new EC2SecurityGroup();

            sg.LogicalId = input.Name;
            sg.Type = "AWS::EC2::SecurityGroup";

            var props = input.Value["Properties"];
            foreach (var prop in props)
            {
                switch ((string)prop.Name)
                {
                    case "GroupDescription":
                        sg.Properties.GroupDescription = prop.Value;
                        break;
                    case "VpcId":
                        var a = prop.Value;
                        sg.Properties.VpcId = a.Value;
                        break;
                    case "Tags":

                        break;
                    case "SecurityGroupIngress":
                        var ingressRules = prop.Value;
                        
                        foreach (var rule in ingressRules)
                        {
                            EC2SecurityGroupIngress sgi = new EC2SecurityGroupIngress();

                            //FormatProtocol - Protocol could be a number or text (e.g. 6 or tcp)                         
                            sgi.IpProtocol = FormatProtocol(rule.IpProtocol.ToString());
                            //-------------------------------------------------------------------

                            //FormatPortRange - Port range could be 0-0 -1-1 0-65535
                            string from = "";
                            string to = "";
                            FormatPortRange(rule.FromPort.ToString(), rule.ToPort.ToString(), out from, out to);
                            sgi.FromPort = from;
                            sgi.ToPort = to;
                            //-------------------------------------------------------

                            sgi.CidrIp = rule.CidrIp;
                            if (rule.SourceSecurityGroupId != null)
                            {
                                var Ids = rule.SourceSecurityGroupId;
                                foreach (var Id in Ids)
                                {
                                    sgi.SourceSecurityGroupId = Id.Value;
                                }
                            }
                            sg.Properties.SecurityGroupIngress.Add(sgi);
                        }
                        break;
                }
            }

            stack.Resources.Add(sg);
        }
        
        static void ProcessIAMUserFromTemplate(dynamic input, CFStack stack)
        {
            IAMUser u = new IAMUser();

            u.LogicalId = input.Name;
            u.Type = "AWS::IAM::User";

            var props = input.Value["Properties"];
            foreach (var prop in props)
            {
                switch ((string)prop.Name)
                {
                    case "Path":
                        u.Properties.Path = prop.Value;
                        break;
                    case "Policies":
                        var policies = prop.Value;
                        foreach (var policy in policies)
                        {
                            IAMUserPolicy pol = new IAMUserPolicy();
                            pol.PolicyName = (string)policy["PolicyName"];
                            var policyDoc = policy["PolicyDocument"];
                            foreach (var statementItems in policyDoc)
                            {
                                foreach (var statement in statementItems.Value)
                                {
                                    IAMUserStatement us = new IAMUserStatement();
                                    us.Action = statement.Action;
                                    us.Effect = statement.Effect;
                                    us.Resource = statement.Resource;
                                    pol.PolicyDocument.Statement.Add(us);
                                }
                            }
                            u.Properties.Policies.Add(pol);
                        }
                        break;
                    default:
                        break;
                }
            }
            stack.Resources.Add(u);
        }


        // -----------------------------------------------------------------------
        // Live Stack

        public static void ProcessNetworkAclFromAWS(StackResource resource, CFStack stack, AmazonEC2Client ec2Client)
        {
            DescribeNetworkAclsRequest naclRequest = new DescribeNetworkAclsRequest();
            naclRequest.NetworkAclIds = new List<string> { resource.PhysicalResourceId };
            
            DescribeNetworkAclsResponse response = ec2Client.DescribeNetworkAcls(naclRequest);
            foreach (Amazon.EC2.Model.NetworkAcl nacl in response.NetworkAcls)
            {
                NetworkAcl n = new NetworkAcl();
                n.LogicalId = resource.LogicalResourceId;
                if (log) { Utils.WriteToFile(logFile, "AWS NACL: " + n.LogicalId.ToString(), true); }
                n.Type = "AWS::EC2::NetworkAcl";
                n.Properties.VpcId = nacl.VpcId;

                foreach (Amazon.EC2.Model.NetworkAclEntry e in nacl.Entries)
                {
                    NetworkAclEntry ne = new NetworkAclEntry();
                    ne.RuleNumber = e.RuleNumber.ToString();
                    ne.CidrBlock = e.CidrBlock;
                    ne.Egress = e.Egress;
                    if (e.PortRange == null)
                    {
                        ne.FromPort = "ALL"; ne.ToPort = "ALL";
                    }
                    else
                    {
                        //FormatPortRange - Port range could be 0-0 -1-1 0-65535
                        string from = "";
                        string to = "";
                        FormatPortRange(e.PortRange.From.ToString(), e.PortRange.To.ToString(), out from, out to);
                        ne.FromPort = from;
                        ne.ToPort = to;
                        //------------------------------------------------------
                    }

                    //FormatProtocol - Protocol could be a number or text (e.g. 6 or tcp) 
                    ne.Protocol = FormatProtocol(e.Protocol);
                    //-------------------------------------------------------------------

                    ne.RuleAction = e.RuleAction;
                    //ICMP not included.

                    n.Properties.NetworkAclEntry.Add(ne);

                    if (e.PortRange == null)
                    {
                        if (log) { Utils.WriteToFile(logFile, ne.RuleNumber + " Protocol: " + e.Protocol + " | From: " + "null" + " To: " + "null", true); }
                    }
                    else
                    {
                        if (log) { Utils.WriteToFile(logFile, ne.RuleNumber + " Protocol: " + e.Protocol + " | From: " + e.PortRange.From.ToString() + " To: " + e.PortRange.To.ToString(), true); }
                    }
                }

                stack.Resources.Add(n);
            }

            
        }


        static DescribeSecurityGroupsResponse GetSecurityGroup(AmazonEC2Client ec2Client, DescribeSecurityGroupsRequest request)
        {
            DescribeSecurityGroupsResponse sg = null;
            try
            {
                sg = ec2Client.DescribeSecurityGroups(request);
            }
            catch (Exception)
            {

            }
            return sg;
        }


        public static void ProcessEC2SecurityGroupFromAWS(StackResource resource, CFStack stack, AmazonEC2Client ec2Client, Dictionary<string, string> secGroupMap)
        {
            DescribeSecurityGroupsRequest secGroupRequest = new DescribeSecurityGroupsRequest();
            //Set request to use Phisical Id
            secGroupRequest.GroupIds = new List<string> { resource.PhysicalResourceId };

            //Attempt to get security group using physical Id
            DescribeSecurityGroupsResponse response = GetSecurityGroup(ec2Client, secGroupRequest);
            
            if (response == null)
            {
                //Set request to use Logical Id and Stack Name Tags
                secGroupRequest.GroupIds.Clear();
                List<Filter> f = new List<Filter>();
                f.Add(new Filter { Name = "tag:aws:cloudformation:logical-id", Values = new List<string>() {resource.LogicalResourceId}});
                f.Add(new Filter { Name = "tag:aws:cloudformation:stack-name", Values = new List<string>() { resource.StackName }});                
                secGroupRequest.Filters = f;
                //Attempt to get security group using logical Id
                response = GetSecurityGroup(ec2Client, secGroupRequest);
            }

            if (response == null | response.SecurityGroups.Count == 0)
            {
                return;
            }

            foreach (SecurityGroup group in response.SecurityGroups)
            {       
                EC2SecurityGroup sg = new EC2SecurityGroup();                
                sg.LogicalId = resource.LogicalResourceId;
                if (log) { Utils.WriteToFile(logFile, "AWS SG: " + sg.LogicalId.ToString(), true); }
                sg.Type = "AWS::EC2::SecurityGroup";
                sg.Properties.GroupDescription = group.Description;
                sg.Properties.VpcId = group.VpcId;

                foreach (IpPermission perms in group.IpPermissions)
                {

                    for (int i = 0; i < perms.IpRanges.Count; i++)
                    {
                        EC2SecurityGroupIngress sgi = new EC2SecurityGroupIngress();

                        //FormatProtocol - Protocol could be a number or text (e.g. 6 or tcp)                         
                        sgi.IpProtocol = FormatProtocol(perms.IpProtocol);
                        //--------------------------------------------------------------------

                        //FormatPortRange - Port range could be 0-0 -1-1 0-65535                        
                        string from = "";
                        string to = "";
                        FormatPortRange(perms.FromPort.ToString(), perms.ToPort.ToString(), out from, out to);
                        sgi.FromPort = from;
                        sgi.ToPort = to;
                        //------------------------------------------------------

                        sgi.CidrIp = perms.IpRanges[i];
                        sg.Properties.SecurityGroupIngress.Add(sgi);

                        if (log) { Utils.WriteToFile(logFile, " Protocol: " + perms.IpProtocol + " | From: " + perms.FromPort.ToString() + " To: " + perms.ToPort.ToString(), true); }
                    }
                    for (int i = 0; i < perms.UserIdGroupPairs.Count; i++)
                    {
                        EC2SecurityGroupIngress sgi = new EC2SecurityGroupIngress();
                        //FormatProtocol - Protocol could be a number or text (e.g. 6 or tcp)                         
                        sgi.IpProtocol = FormatProtocol(perms.IpProtocol);
                        //--------------------------------------------------------------------

                        //FormatPortRange - Port range could be 0-0 -1-1 0-65535                        
                        string from = "";
                        string to = "";
                        FormatPortRange(perms.FromPort.ToString(), perms.ToPort.ToString(), out from, out to);
                        sgi.FromPort = from;
                        sgi.ToPort = to;
                        //-------------------------------------------------------

                        sg.Properties.SecurityGroupIngress.Add(sgi);
                        string groupName;
                        if (secGroupMap.TryGetValue(perms.UserIdGroupPairs[i].GroupId, out groupName))
                        {
                            sgi.SourceSecurityGroupId = groupName;
                        }
                        else
                        {
                            sgi.SourceSecurityGroupId = perms.UserIdGroupPairs[i].GroupId;
                        }

                        if (log) { Utils.WriteToFile(logFile, " Protocol: " + perms.IpProtocol + " | From: " + perms.FromPort.ToString() + " To: " + perms.ToPort.ToString(), true); }
                    }

                }
                stack.Resources.Add(sg);
            }
        }


        //Protocol could be a number or text (e.g. 6 or tcp) 
        //Convert to string number using Protocols Dictionary
        static string FormatProtocol(string input)
        {
            string result = input;
            
            foreach (string key in App.Protocols.Keys)
            {
                //Compare case insensitive
                if (String.Compare(input, App.Protocols[key], true) == 0)
                {
                    result = key;
                    break;
                }
            }

            return result;
        }


        static void FormatPortRange(string from, string to, out string r1, out string r2)
        {
            r1 = "";
            r2 = "";

            if ((from == "0" && to == "0") || (from == "-1" && to == "-1") || (from == "0" && to == "65535"))
            {
                r1 = "ALL";
                r2 = "ALL";
            }
            else
            {
                r1 = from;
                r2 = to;
            }
                        
        }
    }
}
