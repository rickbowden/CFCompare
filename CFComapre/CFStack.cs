using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFComapre
{
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
    class EC2SecurityGroupProperties
    {
        public string GroupDescription { get; set; }
        public List<EC2SecurityGroupIngress> SecurityGroupIngress { get; set; }

        public EC2SecurityGroupProperties()
        {
            SecurityGroupIngress = new List<EC2SecurityGroupIngress>();
        }
    }
    public class EC2SecurityGroupIngress
    {
        public string IpProtocol { get; set; }
        public string FromPort { get; set; }
        public string ToPort { get; set; }
        public string CidrIp { get; set; }
        public string SourceSecurityGroupId { get; set; }
        public string SourceSecurityGroupName { get; set; }
        public string GroupName { get; set; }
    }

    //------------------------------------------------------------------------------------

    class EC2Instance
    {

    }
    class EC2InstanceProperties
    {

    }

    //------------------------------------------------------------------------------------

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
    class IAMUserProperties
    {
        public string Path { get; set; }
        public List<IAMUserPolicy> Policies { get; set; }

        public IAMUserProperties()
        {
            Policies = new List<IAMUserPolicy>();
        }
    }
    public class IAMUserPolicy
    {
        public string PolicyName { get; set; }
        public IAMUserPolicyDocument PolicyDocument { get; set; }

        public IAMUserPolicy()
        {
            PolicyDocument = new IAMUserPolicyDocument();
        }
    }
    public class IAMUserPolicyDocument
    {
        public List<IAMUserStatement> Statement { get; set; }

        public IAMUserPolicyDocument()
        {
            Statement = new List<IAMUserStatement>();
        }
    }
    public class IAMUserStatement
    {
        public string Effect { get; set; }
        public string Action { get; set; }
        public string Resource { get; set; }
    }

    //------------------------------------------------------------------------------------
}
