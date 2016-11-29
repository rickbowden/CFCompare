using Amazon;
using Amazon.CloudFormation;
using Amazon.CloudFormation.Model;
using Amazon.EC2;
using Amazon.EC2.Model;
using Amazon.Runtime;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CFComapre
{
    public partial class App : Form
    {
                    
        string templatePath1 = "";
        string profileName1 = "";
        string jasonString1 = "";
        string stackName1 = "";
        string templatePath2 = "";
        string profileName2 = "";
        string jasonString2 = "";
        string stackName2 = "";

        const string tab1 = "    ";
        const string tab2 = "            ";
        const string tab3 = "                    ";
        const string tab4 = "                            ";

        CFStack Stack1 = null;
        CFStack Stack2 = null;
        CFStack CompareStack1 = null;
        CFStack CompareStack2 = null;

        bool CompareRemoves = true;

        public App()
        {
            InitializeComponent();

            validation1_LB.Visible = false;
            validation2_LB.Visible = false;
            AWSConfigs.AWSRegion = "eu-west-1";
            PopulateProfileList(profile1_CB);
            PopulateProfileList(profile2_CB);
            
        }




        private void source1_CB_SelectedIndexChanged(object sender, EventArgs e)
        {
            sourceSelection(source1_CB);            
        }
        private void source2_CB_SelectedIndexChanged(object sender, EventArgs e)
        {
            sourceSelection(source2_CB);   
        }
        private void sourceSelection(ComboBox cb)
        {
            if (cb.Name.Equals(source1_CB.Name))
            {
                switch (cb.Text)
                {
                    case "Template":
                        templateOrStack1_TB.Enabled = true;
                        templateOrStack1_LB.Text = "Template:";
                        templatePath1 = templateOrStack1_TB.Text;
                        break;
                    case "AWS":
                        templateOrStack1_TB.Enabled = true;
                        templateOrStack1_LB.Text = "Stack:";
                        templateOrStack1_TB.Text = "";
                        break;
                }
            }
            else
            {
                switch (cb.Text)
                {
                    case "Template":
                        templateOrStack2_TB.Enabled = true;
                        templateOrStack2_LB.Text = "Template:";
                        templatePath2 = templateOrStack2_TB.Text;
                        break;
                    case "AWS":
                        templateOrStack2_TB.Enabled = true;
                        templateOrStack2_LB.Text = "Stack:";
                        templateOrStack2_TB.Text = "";
                        break;
                }
            }
        }
        private void PopulateProfileList(ComboBox cb)
        {
            cb.Items.Clear();
            foreach (var item in Amazon.Util.ProfileManager.ListProfiles())
            {
                cb.Items.Add(item.Name);
            }
        }




        ///GO
        private void Go1_BTN_Click(object sender, EventArgs e)
        {            
            Go_Window1();
            CompareStack1 = Stack1.DeepClone();
        }
        private void Go2_BTN_Click(object sender, EventArgs e)
        {
            Go_Window2();
            CompareStack2 = Stack2.DeepClone();
        }

        private void Go_Window1()
        {
            if (inputsAreValid(source1_CB, profile1_CB, templateOrStack1_TB, validation1_LB))
            {
                AmazonCloudFormationClient CFclient = null;
                AmazonEC2Client EC2client = null;

                Stack1 = new CFStack();
                CompareStack1 = new CFStack();

                try
                {
                    profileName1 = profile1_CB.Text;
                    if (validateTemplate_CB.Checked || source1_CB.Text == "AWS")
                    {
                        var creds = new StoredProfileAWSCredentials(profileName1);
                        CFclient = new AmazonCloudFormationClient(creds);
                        EC2client = new AmazonEC2Client(creds);
                    }

                    switch (source1_CB.Text)
                    {
                        case "Template":
                            templatePath1 = templateOrStack1_TB.Text.Trim();
                            using (StreamReader sr = new StreamReader(templatePath1))
                            {
                                jasonString1 = sr.ReadToEnd();
                                if (validateTemplate_CB.Checked)
                                {
                                    if (validateTemplate(jasonString1, CFclient))
                                    {
                                        ProcessTemplate(jasonString1, richTextBox1, templatePath1, Stack1);                                        
                                    }
                                }
                                else
                                {
                                    ProcessTemplate(jasonString1, richTextBox1, templatePath1, Stack1);
                                }
                            }
                            break;
                        case "AWS":
                            stackName1 = templateOrStack1_TB.Text.Trim();
                            ProcessLiveStack(stackName1, CFclient, EC2client, richTextBox1, Stack1);
                            break;
                    }
                    profileName1 = profile1_CB.Text.Trim();
                }
                catch (Exception ex)
                {
                    richTextBox1.Text = ex.Message;
                }
                finally
                {
                    if (CFclient != null) { CFclient.Dispose(); }
                    if (EC2client != null) { EC2client.Dispose(); }
                }
            }
        }

        private void Go_Window2()
        {
            if (inputsAreValid(source2_CB, profile2_CB, templateOrStack2_TB, validation2_LB))
            {
                AmazonCloudFormationClient CFclient = null;
                AmazonEC2Client EC2client = null;

                Stack2 = new CFStack();
                CompareStack2 = new CFStack();

                try
                {
                    profileName2 = profile2_CB.Text;
                    if (validateTemplate_CB.Checked || source2_CB.Text == "AWS")
                    {
                        var creds = new StoredProfileAWSCredentials(profileName2);
                        CFclient = new AmazonCloudFormationClient(creds);
                        EC2client = new AmazonEC2Client(creds);
                    }

                    switch (source2_CB.Text)
                    {
                        case "Template":
                            templatePath2 = templateOrStack2_TB.Text.Trim();
                            using (StreamReader sr = new StreamReader(templatePath2))
                            {
                                jasonString2 = sr.ReadToEnd();
                                if (validateTemplate_CB.Checked)
                                {
                                    if (validateTemplate(jasonString2, CFclient))
                                    {
                                        ProcessTemplate(jasonString2, richTextBox2, templatePath2, Stack2);
                                    }
                                }
                                else
                                {
                                    ProcessTemplate(jasonString2, richTextBox2, templatePath2, Stack2);
                                }
                            }
                            break;
                        case "AWS":
                            stackName2 = templateOrStack2_TB.Text.Trim();
                            ProcessLiveStack(stackName2, CFclient, EC2client, richTextBox2, Stack2);
                            break;
                    }
                    profileName2 = profile2_CB.Text.Trim();
                }
                catch (Exception ex)
                {
                    richTextBox2.Text = ex.Message;
                }
                finally
                {
                    if (CFclient != null) { CFclient.Dispose(); }
                    if (EC2client != null) { EC2client.Dispose(); }
                }
            }
        }

        private void compare_BTN_Click(object sender, EventArgs e)
        {            
        }

        private void ProcessTemplate(string jsonString, RichTextBox rtb, string path, CFStack stack)
        {
            //Stack From Template
            
            var template = JsonConvert.DeserializeObject<dynamic>(jsonString);
            stack.Description = template.Description;
            //Process StackResources            
            AWSUtils.ProcessTemplateResources(template.Resources, stack);
            stack.Resources.Sort((a, b) => a.LogicalId.CompareTo(b.LogicalId));
            foreach (var item in stack.Resources)
            {
                
            }
            WriteOutput(stack, rtb, "CF Template", path);
            
        }


        private void ProcessLiveStack(string stackName, AmazonCloudFormationClient cfClient, AmazonEC2Client ec2Client, RichTextBox rtb, CFStack stack)
        {
            
            //Get Live Stack
            DescribeStacksRequest cfRequest = new DescribeStacksRequest();
            cfRequest.StackName = stackName;
            DescribeStacksResponse liveStack = cfClient.DescribeStacks(cfRequest);
            stack.Description = liveStack.Stacks[0].Description;
            

            //Get Stack Resouces
            DescribeStackResourcesRequest cfResourcesRequest = new DescribeStackResourcesRequest();
            cfResourcesRequest.StackName = stackName;
            DescribeStackResourcesResponse liveStackResources = cfClient.DescribeStackResources(cfResourcesRequest);

            //Get SecurityGroups and map id to name
            Dictionary<string, string> secGroupMap = new Dictionary<string, string>();
            DescribeSecurityGroupsRequest secGroupRequestAll = new DescribeSecurityGroupsRequest();

            //Get all security group Id's and cf logicalId's (if any)
            DescribeSecurityGroupsResponse secGroupResponseAll = ec2Client.DescribeSecurityGroups(secGroupRequestAll);
            foreach (SecurityGroup sg in secGroupResponseAll.SecurityGroups)
            {
                string value = "none";
                foreach (Amazon.EC2.Model.Tag tag in sg.Tags)
                {
                    if (tag.Key.Contains("aws:cloudformation:logical-id")) { value = tag.Value; }
                }
                secGroupMap.Add(sg.GroupId, value);
            }

            foreach (StackResource liveStackResource in liveStackResources.StackResources)
            {
                switch (liveStackResource.ResourceType)
                {
                    case "AWS::EC2::SecurityGroup":
                        AWSUtils.ProcessEC2SecurityGroupFromAWS(liveStackResource, stack, ec2Client, secGroupMap);
                        break;
                    default:
                        break;
                }

            }
            stack.Resources.Sort((a, b) => a.LogicalId.CompareTo(b.LogicalId));

            WriteOutput(stack, rtb, "Live Stack", stackName);
        }



        private void WriteOutput(CFStack s, RichTextBox rtb, string source, string name)
        {
            rtb.Clear();

            rtb.AppendText("Source: " + source); rtb.AppendText(Environment.NewLine);
            rtb.AppendText("Name/Path: " + name); rtb.AppendText(Environment.NewLine); rtb.AppendText(Environment.NewLine);

            rtb.AppendText("Decription: " + s.Description); rtb.AppendText(Environment.NewLine);

            rtb.AppendText("Resources"); rtb.AppendText(Environment.NewLine);
            foreach (var resource in s.Resources)
            {                
                rtb.AppendText(tab1) ; rtb.AppendText(resource.LogicalId); rtb.AppendText(Environment.NewLine);
                rtb.AppendText(tab2); rtb.AppendText("Type: " + resource.Type); rtb.AppendText(Environment.NewLine);
                
                var properties = resource.Properties;
                string type = resource.Type;
                switch (type)
                {
                    case "AWS::EC2::SecurityGroup":
                        EC2SecurityGroup group = (EC2SecurityGroup)resource;
                        rtb.AppendText(tab3); rtb.AppendText("Group Description: " + group.Properties.GroupDescription); rtb.AppendText(Environment.NewLine);
                        var ingressRules = group.Properties.SecurityGroupIngress.OrderBy(a => a.IpProtocol).ThenBy(a => a.ToPort).ThenBy(a => a.FromPort).ThenBy(a => a.CidrIp).ThenBy(a => a.SourceSecurityGroupId);
                        foreach (var ingressRule in ingressRules)
                        {
                            if (ingressRule.State == null)
                            {
                                rtb.AppendText(tab4); rtb.AppendText("Protocol: " + ingressRule.IpProtocol + " | ");
                            }
                            else
                            {
                                rtb.AppendText(ingressRule.State); rtb.AppendText(tab3); rtb.AppendText("Protocol: " + ingressRule.IpProtocol + " | ");
                            }
                            if (ingressRule.FromPort.Equals(ingressRule.ToPort, StringComparison.Ordinal))
                            {
                                rtb.AppendText("Port Range: " + ingressRule.FromPort + " | ");
                            }
                            else
                            {
                                rtb.AppendText("Port Range: " + ingressRule.FromPort + "-" + ingressRule.ToPort + " | ");
                            }
                            
                            if (ingressRule.CidrIp != null)
                            {
                                rtb.AppendText("Source: " + ingressRule.CidrIp);
                            }
                            else
                            {
                                rtb.AppendText("Source: " + ingressRule.SourceSecurityGroupId);
                            }

                            rtb.AppendText(Environment.NewLine);
                        }
                        break;                    
                }

                rtb.AppendText(Environment.NewLine);
            }
            foreach (var item in rtb.Lines)
            {
                
            }
            
        }



        private bool inputsAreValid(ComboBox source, ComboBox profile, TextBox templateOrStack, Label validation)
        {
            bool result = false;

            if (source.Text == null || source.Text == "" || source.Text == "select")
            {
                validation.Text = "Please select a source";
                validation.ForeColor = System.Drawing.Color.Red;
                validation.Visible = true;
            }
            else
            {
                if ((validateTemplate_CB.Checked) && (profile.Text == null || profile.Text == "" || profile.Text == "select"))
                {
                    validation.Text = "Please select profile";
                    validation.ForeColor = System.Drawing.Color.Red;
                    validation.Visible = true;
                }
                else
                {                    
                    if (source.Text == "AWS")
                    {
                        if (templateOrStack.Text == null || templateOrStack.Text == "")
                        {
                            validation.Text = "Please enter a stack name";
                            validation.ForeColor = System.Drawing.Color.Red;
                            validation.Visible = true;
                        }
                        else
                        {
                            result = true;
                            validation1_LB.Text = "";
                        }
                    }
                    else
                    {
                        if (templateOrStack.Text == null || templateOrStack.Text == "")
                        {
                            validation.Text = "Please enter a template path";
                            validation.ForeColor = System.Drawing.Color.Red;
                            validation.Visible = true;
                        }
                        else
                        {
                            result = true;
                            validation1_LB.Text = "";
                        }
                    }
                }
            }

            return result;
        }

        private bool validateTemplate(string input, AmazonCloudFormationClient cfClient)
        {
            bool result = false;
            if (AWSUtils.IsTemplateValid(input, ref cfClient))
            {
                validation1_LB.Text = "Template is valid";
                validation1_LB.ForeColor = System.Drawing.Color.DarkGreen;
                validation1_LB.Visible = true;
                result = true;
            }
            else
            {
                validation1_LB.Text = "Template is not valid";
                validation1_LB.ForeColor = System.Drawing.Color.Red;
                validation1_LB.Visible = true;
            }
            return result;
        }




        private void compare_BTN_Click_1(object sender, EventArgs e)
        {        
           CompareStacks();
        }

        private void CompareStacks()
        {
            CompareStackDescription();
            CompareStackResources();

            WriteOutput(CompareStack1, richTextBox1, source1_CB.Text, templateOrStack1_TB.Text);
            WriteOutput(CompareStack2, richTextBox2, source2_CB.Text, templateOrStack2_TB.Text);
        }

        private void CompareStackDescription()
        {

        }

        private void CompareStackResources()
        {
            if (CompareStack1 == null || CompareStack2 == null) { return; }
            if (Stack1 == null || Stack2 == null) { return; }

            //var compareStack1Resources = CompareStack1.Resources;
                        
            foreach (var stack1Resource in Stack1.Resources)
            {
                var stack1LogicalId = stack1Resource.LogicalId;
                //Find matching resource in Stack2
                var stack2Resource = Stack2.Resources.Find(n => n != null && n.LogicalId == stack1LogicalId);
                //Find matching resource in CompareStack1
                var compareStack1Resource = CompareStack1.Resources.Find(n => n != null && n.LogicalId == stack1LogicalId);
                var compareStack2Resource = CompareStack2.Resources.Find(n => n != null && n.LogicalId == stack1LogicalId);
                if (stack2Resource != null) //Found Matching Resource
                {
                    switch ((String)stack1Resource.Type)
                    {
                        case "AWS::EC2::SecurityGroup":
                              
                            List<EC2SecurityGroupIngress> i1List = stack1Resource.Properties.SecurityGroupIngress;
                            List<EC2SecurityGroupIngress> i2List = stack2Resource.Properties.SecurityGroupIngress;
                            List<EC2SecurityGroupIngress> comparei1List = compareStack1Resource.Properties.SecurityGroupIngress;                            
                            CompareSecurityGroups(stack1Resource, stack2Resource, compareStack1Resource, CompareStack1);
                            CompareSecurityGroups(stack2Resource, stack1Resource, compareStack2Resource, CompareStack2);
                            break;
                    }
                }
                                
            }
        }

        private void CompareSecurityGroups(EC2SecurityGroup resource1, EC2SecurityGroup resource2, EC2SecurityGroup compareResource, CFStack compareStack)
        {
            List<EC2SecurityGroupIngress> i1List = resource1.Properties.SecurityGroupIngress;
            List<EC2SecurityGroupIngress> i2List = resource2.Properties.SecurityGroupIngress;
            List<EC2SecurityGroupIngress> compareList = compareResource.Properties.SecurityGroupIngress;

            foreach (EC2SecurityGroupIngress x in i1List)
            {               
                
                var y = i2List.Find(n => n != null && n.CidrIp == x.CidrIp && n.FromPort == x.FromPort && n.ToPort == x.ToPort && n.IpProtocol == x.IpProtocol && n.SourceSecurityGroupId == x.SourceSecurityGroupId);
                if (y == null)
                {
                    if (CompareRemoves) 
                    { 
                        x.State = "Removed";
                    }
                }
                else
                {
                    if (CompareRemoves == true) {
                        var z = compareList.Find(n => n != null && n.CidrIp == x.CidrIp && n.FromPort == x.FromPort && n.ToPort == x.ToPort && n.IpProtocol == x.IpProtocol && n.SourceSecurityGroupId == x.SourceSecurityGroupId);
                        if (z != null)
                        {
                            compareList.Remove(z);
                        }
                    }
                }
            }

            if (compareResource.Properties.SecurityGroupIngress.Count() == 0)
            {
                compareStack.Resources.Remove(compareResource);
            }

        }


    }

    public static class ExtensionMethods
    {
        // Deep clone
        public static T DeepClone<T>(this T a)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, a);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            }
        }
    }
}
