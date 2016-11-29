﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFComapre
{
    [Serializable]
    class CFStack
    {
        public string Description { get; set; }
        //public Resources Resources { get; set; }
        public List<dynamic> Resources { get; set; }

        public CFStack()
        {
            Resources = new List<dynamic>();
        }
    }

    //------------------------------------------------------------------------------------

    [Serializable]
    class EC2SecurityGroup
    {
        public object LogicalId { get; set; }
        public object Type { get; set; }
        public EC2SecurityGroupProperties Properties { get; set; }

        public EC2SecurityGroup()
        {
            Properties = new EC2SecurityGroupProperties();
        }
    }
    [Serializable]
    class EC2SecurityGroupProperties
    {
        public string GroupDescription { get; set; }
        public string VpcId { get; set; }
        public List<EC2SecurityGroupIngress> SecurityGroupIngress { get; set; }

        public EC2SecurityGroupProperties()
        {
            SecurityGroupIngress = new List<EC2SecurityGroupIngress>();
        }
    }
    [Serializable]
    public class EC2SecurityGroupIngress
    {
        public string IpProtocol { get; set; }
        public string FromPort { get; set; }
        public string ToPort { get; set; }
        public string CidrIp { get; set; }
        public string SourceSecurityGroupId { get; set; }
        public string SourceSecurityGroupName { get; set; }
        public string GroupName { get; set; }                           //Not AWS property
        public string State { get; set; }                               //Not AWS property
    }

    //------------------------------------------------------------------------------------
    [Serializable]
    class EC2Instance
    {

    }
    [Serializable]
    class EC2InstanceProperties
    {

    }

    //------------------------------------------------------------------------------------

    [Serializable]
    class IAMUser
    {
        public object LogicalId { get; set; }
        public object Type { get; set; }
        public IAMUserProperties Properties { get; set; }

        public IAMUser()
        {
            Properties = new IAMUserProperties();
        }
    }
    [Serializable]
    class IAMUserProperties
    {
        public string Path { get; set; }
        public List<IAMUserPolicy> Policies { get; set; }

        public IAMUserProperties()
        {
            Policies = new List<IAMUserPolicy>();
        }
    }
    [Serializable]
    public class IAMUserPolicy
    {
        public string PolicyName { get; set; }
        public IAMUserPolicyDocument PolicyDocument { get; set; }

        public IAMUserPolicy()
        {
            PolicyDocument = new IAMUserPolicyDocument();
        }
    }
    [Serializable]
    public class IAMUserPolicyDocument
    {
        public List<IAMUserStatement> Statement { get; set; }

        public IAMUserPolicyDocument()
        {
            Statement = new List<IAMUserStatement>();
        }
    }
    [Serializable]
    public class IAMUserStatement
    {
        public string Effect { get; set; }
        public string Action { get; set; }
        public string Resource { get; set; }
    }

    //------------------------------------------------------------------------------------
}
