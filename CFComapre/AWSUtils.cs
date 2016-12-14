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

namespace CFComapre
{
    class AWSUtils
    {
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
                    default:
                        break;
                }
            }

        }
        
        static void ProcessEC2SecurityGroupIngressFromTemplate(dynamic input, CFStack stack)
        {
            EC2SecurityGroupIngress sgi = new EC2SecurityGroupIngress();

            var rule = input.Value["Properties"];

            sgi.IpProtocol = rule.IpProtocol;
            sgi.ToPort = rule.ToPort;
            sgi.FromPort = rule.FromPort;
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
                            sgi.IpProtocol = rule.IpProtocol;
                            sgi.ToPort = rule.ToPort;
                            sgi.FromPort = rule.FromPort;
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


        public static void ProcessEC2SecurityGroupFromAWS(StackResource resource, CFStack stack, AmazonEC2Client ec2Client, Dictionary<string, string> secGroupMap)
        {
            DescribeSecurityGroupsRequest secGroupRequest = new DescribeSecurityGroupsRequest();
            secGroupRequest.GroupIds = new List<string> { resource.PhysicalResourceId };

            DescribeSecurityGroupsResponse response = ec2Client.DescribeSecurityGroups(secGroupRequest);
            foreach (SecurityGroup group in response.SecurityGroups)
            {       
                EC2SecurityGroup sg = new EC2SecurityGroup();                
                sg.LogicalId = resource.LogicalResourceId;
                sg.Type = "AWS::EC2::SecurityGroup";
                sg.Properties.GroupDescription = group.Description;
                sg.Properties.VpcId = group.VpcId;

                foreach (IpPermission perms in group.IpPermissions)
                {

                    for (int i = 0; i < perms.IpRanges.Count; i++)
                    {
                        EC2SecurityGroupIngress sgi = new EC2SecurityGroupIngress();
                        sgi.IpProtocol = perms.IpProtocol;
                        sgi.FromPort = perms.FromPort.ToString();
                        sgi.ToPort = perms.ToPort.ToString();
                        sgi.CidrIp = perms.IpRanges[i];
                        sg.Properties.SecurityGroupIngress.Add(sgi);
                    }
                    for (int i = 0; i < perms.UserIdGroupPairs.Count; i++)
                    {
                        EC2SecurityGroupIngress sgi = new EC2SecurityGroupIngress();
                        sgi.IpProtocol = perms.IpProtocol;
                        sgi.FromPort = perms.FromPort.ToString();
                        sgi.ToPort = perms.ToPort.ToString();

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
                    }

                }
                stack.Resources.Add(sg);
            }
        }

    }
}
